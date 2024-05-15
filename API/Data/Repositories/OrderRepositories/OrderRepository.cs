using API.Helpers;
using API.Helpers.OrderParameters;
using API.Interfaces;
using API.Models;
using API.Models.DTO.Cart.CartResponses;
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

        public async Task<Result<string>> CheckoutAsync(CartResponse cart)
        {
            OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cart.CartHeader);
            orderHeaderDto.Id = 0;
            orderHeaderDto.FirstName = "Mac";
            orderHeaderDto.LastName = "Donalds";
            orderHeaderDto.Phone = "15548461";
            orderHeaderDto.Email = "email@gmail.com";
            orderHeaderDto.AddressId = 1;
            orderHeaderDto.OrderDate = DateTime.UtcNow;
            orderHeaderDto.OrderStatus = nameof(SD.OrderStatus.PENDING);
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDto>>(cart.CartDetails);
            orderHeaderDto.OrderDetails.ForEach(x => x.Id = 0);

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = "https://localhost:4200",
                CancelUrl = "https://localhost:4200"
            };

            var discounts = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions
                {
                    Coupon = orderHeaderDto.CouponCode
                }
            };

            foreach(var item in orderHeaderDto.OrderDetails)
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

            if(orderHeaderDto.Discount > 0)
            {
                options.Discounts = discounts;
            }

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            orderHeaderDto.StripeSessionId = session.Id;
            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);
            await _dbContext.OrderHeaders.AddAsync(orderHeader);

            return Result.Ok(session.Url);
        }

        public async Task<Result> VerifyStripeSessionAsync(int orderHeaderId)
        {
            var orderHeader = await _dbContext.OrderHeaders.FindAsync(orderHeaderId);
            if(orderHeader == null)
            {
                return Result.Fail("Order doesn't exists");
            }

            var service = new SessionService();
            Session session = await service.GetAsync(orderHeader.StripeSessionId);

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);
            
            if(paymentIntent.Status == "succeeded") 
            {
                orderHeader.PaymentIntentId = paymentIntent.Id;
                orderHeader.OrderStatus = nameof(OrderStatus.APPROVED);
            }

            return Result.Ok();
        }

        public async Task<PagedList<OrderHeaderDto>> GetOrdersAsync(OrderParams orderParams, string userId)
        {
            IQueryable<OrderHeaderDto> ordersQuery;
            var user = await _dbContext.Users.FindAsync(userId);
            if(await _userManager.IsInRoleAsync(user, nameof(UserRoles.ADMIN)))
            {
                ordersQuery = _dbContext.OrderHeaders.AsNoTracking()
                    .Include(h => h.OrderDetails).ProjectTo<OrderHeaderDto>(_mapper.ConfigurationProvider);
            }
            else
            {
                ordersQuery = _dbContext.OrderHeaders.AsNoTracking()
                    .Where(h => h.UserId == userId).Include(h => h.OrderDetails)
                    .ProjectTo<OrderHeaderDto>(_mapper.ConfigurationProvider);
            }

            return await PagedList<OrderHeaderDto>.CreateAsync(ordersQuery, orderParams.Page, orderParams.PageSize);
        }

        public async Task<Result<OrderHeaderDto>> GetOrderAsync(int orderHeaderId, string userId)
        {
            var orderDb = await _dbContext.OrderHeaders.Where(h => h.UserId == userId).AsNoTracking()
                .Include(h => h.OrderDetails).FirstOrDefaultAsync(h => h.Id == orderHeaderId);

            if(orderDb == null)
            {
                return Result.Fail("Order doesn't exist");
            }

            var order = _mapper.Map<OrderHeaderDto>(orderDb);

            return Result.Ok(order);    
        }

        public async Task<Result> UpdateOrderAsync(OrderUpdateRequest model)
        {
            var orderHeader = await _dbContext.OrderHeaders.FindAsync(model.orderHeaderId);

            if(orderHeader == null)
            {
                return Result.Fail("Order doesn't exist");
            }

            if(model.OrderStatus == OrderStatus.CANCELED)
            {
                var refundOptions = new RefundCreateOptions
                {
                    Reason = SD.REQUESTED_BY_CUSTOMER,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = await service.CreateAsync(refundOptions);
            }

            orderHeader.OrderStatus = nameof(model.OrderStatus);

            return Result.Ok();
        }
    }
}
