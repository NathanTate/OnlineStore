using System.Linq.Expressions;
using API.Helpers;
using API.Helpers.OrderParameters;
using API.Interfaces;
using API.Models;
using API.Models.DTO.Order;
using API.Models.DTO.Order.Requests;
using API.Models.Order;
using API.Utility;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentEmail.Core;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using static API.Utility.SD;

namespace API.Data.Repositories.OrderRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderRepository(ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<string>> CheckoutAsync(OrderCheckoutRequest model, string userId)
        {
            OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(model);
            var addressDb = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.ApplicationUserId == userId);
            if (addressDb != null)
            {
                addressDb.Country = model.Address.Country;
                addressDb.City = model.Address.City;
                addressDb.State = model.Address.State;
                addressDb.Street = model.Address.Street;
                addressDb.HouseNumber = model.Address.HouseNumber;
                addressDb.ZipCode = model.Address.ZipCode;
                orderHeaderDto.AddressId = addressDb.Id;
            }
            else
            {
                Models.Address address = _mapper.Map<Models.Address>(model.Address);
                address.ApplicationUserId = userId;
                _dbContext.Add(address);
                await _dbContext.SaveChangesAsync();
                orderHeaderDto.AddressId = address.Id;
            }
            orderHeaderDto.UserId = userId;
            orderHeaderDto.OrderDate = DateTime.UtcNow;
            orderHeaderDto.OrderStatus = nameof(SD.OrderStatus.PENDING);
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(model.CartResponse.CartDetails);
            orderHeaderDto.OrderDetails.ForEach(x => x.Id = 0);
            orderHeaderDto.Id = 0;

            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);
            _dbContext.OrderHeaders.Add(orderHeader);
            await _dbContext.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"https://localhost:4200/validate-order/{orderHeader.Id}",
                CancelUrl = "https://localhost:4200/OrderCancell"
            };

            var discounts = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions
                {
                    Coupon = orderHeaderDto.CouponCode
                }
            };

            foreach (var item in orderHeaderDto.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.ProductPrice * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            if (orderHeaderDto.Discount > 0)
            {
                options.Discounts = discounts;
            }

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            orderHeader.StripeSessionId = session.Id;
            return Result.Ok(session.Url);
        }

        public async Task<Result> VerifyStripeSessionAsync(int orderHeaderId)
        {
            var orderHeader = await _dbContext.OrderHeaders.FindAsync(orderHeaderId);
            if (orderHeader == null)
            {
                return Result.Fail("Order doesn't exists");
            }

            var service = new SessionService();
            Session session = await service.GetAsync(orderHeader.StripeSessionId);

            if (session.PaymentIntentId == null)
            {
                return Result.Fail("Payment Intent doesn't exist");
            }

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                orderHeader.PaymentIntentId = paymentIntent.Id;
                orderHeader.OrderStatus = nameof(OrderStatus.APPROVED);
            }

            return Result.Ok();
        }

        public async Task<PagedList<OrderHeaderDto>> GetOrdersAsync(OrderParams orderParams, string userId)
        {
            IQueryable<OrderHeader> ordersQuery;
            var user = await _dbContext.Users.FindAsync(userId);
            if (await _userManager.IsInRoleAsync(user, nameof(UserRoles.ADMIN)))
            {
                ordersQuery = _dbContext.OrderHeaders.AsNoTracking();
            }
            else
            {
                ordersQuery = _dbContext.OrderHeaders.AsNoTracking()
                    .Where(h => h.UserId == userId);
            }

            ordersQuery = FilterOrders(ordersQuery, orderParams);

            if(orderParams.SortBy?.ToLower() == "desc")
            {
                ordersQuery = ordersQuery.OrderByDescending(GetSortProperty(orderParams));
            }
            else
            {
                ordersQuery = ordersQuery.OrderBy(GetSortProperty(orderParams));
            }
            
            var result = await PagedList<OrderHeaderDto>.CreateAsync(
                ordersQuery.ProjectTo<OrderHeaderDto>(_mapper.ConfigurationProvider),
                orderParams.Page,
                orderParams.PageSize);
            
            return result;
        }

        public async Task<Result<OrderHeaderDto>> GetOrderAsync(int orderHeaderId, string userId)
        {
            var orderDb = await _dbContext.OrderHeaders.Where(h => h.UserId == userId).AsNoTracking()
                .Include(h => h.OrderDetails).FirstOrDefaultAsync(h => h.Id == orderHeaderId);

            if (orderDb == null)
            {
                return Result.Fail("Order doesn't exist");
            }

            var order = _mapper.Map<OrderHeaderDto>(orderDb);

            return Result.Ok(order);
        }

        public async Task<Result> UpdateOrderAsync(OrderUpdateRequest model)
        {
            var orderHeader = await _dbContext.OrderHeaders.FindAsync(model.orderHeaderId);

            if (orderHeader == null)
            {
                return Result.Fail("Order doesn't exist");
            } 
            else if (orderHeader.OrderStatus == nameof(OrderStatus.CANCELED))
            {   
                return Result.Fail("Order has already been refunded");
            }
            

            if (model.OrderStatus == OrderStatus.CANCELED)
            {
                if (orderHeader.PaymentIntentId == null)
                {
                    return Result.Fail("Payment Intent Id is missing");
                }
                var refundOptions = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = await service.CreateAsync(refundOptions);
            }

            orderHeader.OrderStatus = model.OrderStatus.ToString();

            return Result.Ok();
        }

        public async Task<Result> DeleteOrderAsync(int orderHeaderId) {
            var orderHeader = await _dbContext.OrderHeaders.FindAsync(orderHeaderId);

            if(orderHeader == null) 
            {
                return Result.Fail("Order doesn't exist");
            }

            _dbContext.OrderHeaders.Remove(orderHeader);
            
            return Result.Ok();
        }

        private static Expression<Func<OrderHeader, object>> GetSortProperty(OrderParams sortColumn)
        {
            Expression<Func<OrderHeader, object>> keySelector = sortColumn.SortColumn?.ToLower() switch
            {
                "orderdate" => order => order.OrderDate,
                "ordertotal" => order => order.OrderTotal,
                "email" => order => order.Email,
                "phone" => order => order.Phone,
                _ => review => review.Id
            };

            return keySelector;
        }

        private static IQueryable<OrderHeader> FilterOrders(IQueryable<OrderHeader> ordersQuery, OrderParams orderParams) 
        {
            if (!string.IsNullOrWhiteSpace(orderParams.OrderStatus)) 
            {
                ordersQuery = ordersQuery.Where(o => o.OrderStatus == orderParams.OrderStatus.ToUpper());
            }
            if (!string.IsNullOrWhiteSpace(orderParams.SearchTerm)) 
            {
                ordersQuery = ordersQuery
                .Where(o => o.Email.ToLower().Contains(orderParams.SearchTerm.ToLower()) ||
                o.Id.ToString().StartsWith(orderParams.SearchTerm) ||
                o.Phone.Contains(orderParams.SearchTerm));
            }

            return ordersQuery;
        }
    }
}
