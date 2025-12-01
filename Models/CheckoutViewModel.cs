using SneakerShop.SessionModels;

namespace SneakerShop.Models
{
    public class CheckoutViewModel
    {
        public bool IsBuyNow { get; set; }

        // Sản phẩm mua ngay (chỉ 1)
        public CartItem? BuyNowItem { get; set; }

        // Giỏ hàng (nhiều sản phẩm)
        public List<CartItem>? CartItems { get; set; }
    }
}