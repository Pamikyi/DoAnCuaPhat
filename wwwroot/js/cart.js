// Hàm gọi API lấy tổng số lượng trong giỏ và render lên icon
async function updateCartCount() {
    try {
        const response = await fetch("/Cart/Count");
        const count = await response.json();

        const badge = document.getElementById("cart-count");
        if (badge) {
            badge.textContent = count;

            badge.classList.add("cart-bounce");
            setTimeout(() => badge.classList.remove("cart-bounce"), 300);
        }
    } catch (error) {
        console.error("Lỗi khi lấy số lượng giỏ hàng:", error);
    }
}

// Gọi khi trang load
updateCartCount();

// HÀM NÀY LÀM VIỆC KHI BẠN CLICK "Thêm vào giỏ"
window.addToCart = async function (id) {
    try {
        const res = await fetch(`/Cart/Add/${id}`, {
            method: "POST"
        });

        const result = await res.json();
        if (result.success) {
            // sau khi thêm thành công → cập nhật lại số trên icon
            updateCartCount();
        } else {
            console.error("Thêm vào giỏ thất bại");
        }
    } catch (err) {
        console.error("Lỗi khi thêm vào giỏ:", err);
    }
};
