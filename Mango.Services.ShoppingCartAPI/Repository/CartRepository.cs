using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Model;
using Mango.Services.ShoppingCartAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> ClearCartAsync(string userId)
        {
            var cartHeaderFromDb = _context.CartHeaders.FirstOrDefault(x => x.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                _context.CartDetails.RemoveRange(_context.CartDetails.Where(u=>u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _context.CartHeaders.Remove(cartHeaderFromDb);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDTO> CreateUpdateCartAsync(CartDTO cartDTO)
        {
            Cart cart = _mapper.Map<Cart>(cartDTO);

            //check if product exists in database, if not create it!
            var prodInDb = await _context.Products
                .FirstOrDefaultAsync(u => u.ProductId == cartDTO.CartDetails.FirstOrDefault()
                .ProductId);
            if (prodInDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }


            //check if header is null
            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                //create header and details
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    //create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //update the count / cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDTO>(cart);

        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
            };
            cart.CartDetails = _context.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(u=>u.Product);
            return _mapper.Map<CartDTO>(cart);
            
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _context.CartDetails.FirstOrDefault(u => u.CartDetailsId == cartDetailsId);
                int totlCountOfCartItems = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetails);
                if (totlCountOfCartItems == 1)
                {
                    var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeader);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var cartFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            cartFromDb.CouponCode = couponCode;
            _context.CartHeaders.Update(cartFromDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cartFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            cartFromDb.CouponCode = String.Empty;
            _context.CartHeaders.Update(cartFromDb);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
