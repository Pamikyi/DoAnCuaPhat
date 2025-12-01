using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SneakerShop.SessionModels;

namespace SneakerShop.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _http;

        public CartService(IHttpContextAccessor http)
        {
            _http = http;
        }

        private ISession Session => _http.HttpContext!.Session;
        private const string CART_KEY = "CART_SESSION";

        // ============================
        // LẤY GIỎ HÀNG
        // ============================
        public List<CartItem> GetCart()
        {
            var data = Session.GetString(CART_KEY);

            return string.IsNullOrEmpty(data)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(data) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            Session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }

        // ============================
        // THÊM SẢN PHẨM VÀO GIỎ
        // ============================
        public void AddToCart(int id, string name, string image, decimal price)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ShoeId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ShoeId = id,
                    Name = name,
                    Image = image,
                    UnitPrice = price,
                    Quantity = 1
                });
            }

            SaveCart(cart);
        }

        // ============================
        // XÓA 1 SẢN PHẨM
        // ============================
        public void Remove(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ShoeId == id);
            if (item != null)
                cart.Remove(item);

            SaveCart(cart);
        }

        // ============================
        // CẬP NHẬT SỐ LƯỢNG
        // ============================
        public void UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ShoeId == id);
            if (item != null)
            {
                item.Quantity = quantity < 1 ? 1 : quantity;
            }

            SaveCart(cart);
        }

        // ============================
        // XÓA TOÀN BỘ GIỎ
        // ============================
        public void ClearCart()
        {
            Session.Remove(CART_KEY);
        }
    }
}
