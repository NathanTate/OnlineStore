using API.Interfaces;
using API.Models.Cart;
using API.Models.Coupon;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<CartResponse>> GetCartAsync(string userId)
        {
            CartResponse cart = new()
            {
                CartHeader = _mapper.Map<CartHeaderResponse>(await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.UserId == userId))
            };

            if (cart.CartHeader == null)
            {
                return Result.Fail("This user has no cart");
            }

            var cartDetails = await _dbContext.CartDetails.AsNoTracking()
            .ProjectTo<CartDetailResponse>(_mapper.ConfigurationProvider)
            .Where(d => d.CartHeaderId == cart.CartHeader.Id).ToListAsync();


            cart.CartDetails = cartDetails;


            foreach (var cartDetail in cart.CartDetails)
            {
                cart.CartHeader.Total += cartDetail.Count * cartDetail.Product.OriginalPrice;
            }

            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                Coupon coupon = await _dbContext.Coupons.FindAsync(cart.CartHeader.CouponCode);
                if (coupon != null && coupon.MinPrice < cart.CartHeader.Total)
                {
                    cart.CartHeader.Total -= coupon.DiscountAmount;
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }

            return Result.Ok(cart);
        }

        public async Task<bool> CartExistsAsync(string userId)
        {
            return await _dbContext.CartHeaders.AnyAsync(c => c.UserId == userId);
        }

        public async Task<Result> ApplyCouponAsync(CartHeaderRequest model, string userId)
        {
            var cartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            var coupon = await _dbContext.Coupons.FindAsync(model.CouponCode);

            if (cartHeader == null)
            {
                return Result.Fail("Cart doesn't exist");
            }

            cartHeader.CouponCode = coupon.CouponCode;

            return Result.Ok();

        }

        public async Task<Result<CartResponse>> CreateCartAsync(string userId)
        {
            var cartHeaderDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.UserId == userId);
            if (cartHeaderDb != null)
            {
                return Result.Fail("Cart already exists");
            }

            var cartHeader = new CartHeader() { UserId = userId };

            await _dbContext.CartHeaders.AddAsync(cartHeader);

            return Result.Ok(_mapper.Map<CartResponse>(new CartResponse
            {
                CartHeader = _mapper.Map<CartHeaderResponse>(cartHeader),
                CartDetails = new List<CartDetailResponse>()
            }));
        }

        public async Task<Result> UpdateCartAsync(CartDetailRequest model, string userId)
        {
            var cartHeaderDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.UserId == userId);
            if (cartHeaderDb == null)
            {
                return Result.Fail("Cart doesn't exist");
            }

            var product = await _dbContext.Products.Include(p => p.Colors).FirstOrDefaultAsync(p => p.Id == model.ProductId);
            if (product == null)
            {
                return Result.Fail("Failed adding product to cart");
            }
            if(product.Quantity <= 0) 
            {
                return Result.Fail("Product is out of stock");
            }
            var color = product.Colors.FirstOrDefault(c => c.ColorId == model.ColorId);
            if(color == null)
            {
                return Result.Fail("Invalid color for this product");
            }

            var cartDetailsDb = await _dbContext.CartDetails
                .FirstOrDefaultAsync(d => d.CartHeaderId == cartHeaderDb.Id
                && d.ProductId == model.ProductId && d.ColorId == model.ColorId);
            if (cartDetailsDb == null)
            {
                model.CartHeaderId = cartHeaderDb.Id;
                await _dbContext.CartDetails.AddAsync(_mapper.Map<CartDetail>(model));
            }
            else if(model.CountUpdate == true)
            {
                cartDetailsDb.Count = model.Count;
            }
            else
            {
                cartDetailsDb.Count += model.Count;
            }

            return Result.Ok();

        }

        public async Task<Result> RemoveFromCartAsync(int cartDetailsId, bool removeAll, string userId)
        {
            var cartHeader = await _dbContext.CartHeaders.Where(c => c.UserId == userId).FirstOrDefaultAsync();

            if (removeAll)
            {

                if (cartHeader != null)
                {
                    var cartDetails = await _dbContext.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id).ToListAsync();
                    if (cartDetails != null)
                    {
                        _dbContext.CartDetails.RemoveRange(cartDetails);
                        return Result.Ok();
                    }
                    else
                    {
                        return Result.Fail("No items to remove");
                    }

                }
                return Result.Fail("Failed to remove all");
            }
            var cartDetail = await _dbContext.CartDetails.FirstOrDefaultAsync(d => d.Id == cartDetailsId && cartHeader.Id == d.CartHeaderId);
            if (cartDetail == null)
            {
                return Result.Fail("Cart Item doesn't exist");
            }

            _dbContext.CartDetails.Remove(cartDetail);

            _dbContext.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id);

            return Result.Ok();
        }

    }
}
