using API.Interfaces;
using API.Models.Cart;
using API.Models.Coupon;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using AutoMapper;
using FluentResults;
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

            var cartDetails = await _dbContext.CartDetails.AsNoTracking().Where(d => d.CartHeaderId == cart.CartHeader.Id).Include(d => d.Product).ToListAsync();

            cart.CartDetails = _mapper.Map<IEnumerable<CartDetailResponse>>(cartDetails);


            foreach(var cartDetail in cart.CartDetails)
            {
                cart.CartHeader.Total += (cartDetail.Count *  cartDetail.Product.OriginalPrice);
            }

            if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                Coupon coupon = await _dbContext.Coupons.FindAsync(cart.CartHeader.CouponCode);
                if(coupon != null && coupon.MinPrice < cart.CartHeader.Total) 
                { 
                    cart.CartHeader.Total -= coupon.DiscountAmount;
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }

            return Result.Ok(cart);     
        }

        public Task<Result> ApplyCouponAsync(CartHeaderRequest model, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<CartResponse>> CreateCartAsync(string userId)
        {
            var cartHeaderDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(h => h.UserId == userId);
            if(cartHeaderDb != null)
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
            if(cartHeaderDb == null)
            {
                return Result.Fail("Cart doesn't exist");
            }

            var product = await _dbContext.Products.FindAsync(model.ProductId);
            if(product == null)
            {
                return Result.Fail("Failed adding product to cart");
            }

            var cartDetailsDb = await _dbContext.CartDetails
                .FirstOrDefaultAsync(d => d.CartHeaderId == cartHeaderDb.Id 
                && d.ProductId == model.ProductId);
            if(cartDetailsDb == null) 
            {
                model.CartHeaderId = cartHeaderDb.Id;
                await _dbContext.CartDetails.AddAsync(_mapper.Map<CartDetail>(model));
            }
            else
            {
                cartDetailsDb.Count += model.Count;
            }

            return Result.Ok();

        }

        public async Task<Result> DeleteCartAsync(int cartDetailsId, string userId)
        {
            var cartDetail = await _dbContext.CartDetails.FirstOrDefaultAsync(d => d.Id == cartDetailsId);
            if(cartDetail == null)
            {
                return Result.Fail("Cart Item doesn't exist");
            }
            int totalCountOfItems = _dbContext.CartDetails.Where(d => d.CartHeaderId == cartDetail.CartHeaderId).Count();
            _dbContext.CartDetails.Remove(cartDetail);
            //if(totalCountOfItems == 1) 
            //{
            //    var cartHeaderToRemove = await _dbContext.CartHeaders.FirstOrDefaultAsync(h => h.Id == cartDetail.CartHeaderId);

            //    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
            //}

            return Result.Ok();
        }

    }
}
