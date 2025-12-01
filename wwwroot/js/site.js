document.addEventListener("DOMContentLoaded", function () {
  const tabs = document.querySelectorAll(".tab");
  const contents = document.querySelectorAll(".tab-content");

  // Mặc định load ALL
  loadAll();

  tabs.forEach((tab) => {
    tab.addEventListener("click", function () {
      // Active style
      tabs.forEach((t) => t.classList.remove("active"));
      this.classList.add("active");

      const tabId = this.dataset.tab;
      const category = this.dataset.category;

      // Ẩn tất cả nội dung
      contents.forEach((c) => c.classList.remove("active"));

      // Hiện đúng tab-content
      document.getElementById(tabId).classList.add("active");

      // Nếu tab là ALL ⇒ load tất cả
      if (tabId === "all") {
        loadAll();
      } else {
        // category = 1/2/3
        loadCategory(category);
      }
    });
  });
});

function loadAll() {
  fetch("/Home/GetAll")
    .then((res) => res.json())
    .then((data) => renderShoes("all", data))
    .catch((err) => console.error(err));
}

function loadCategory(id) {
  fetch(`/Home/GetByCategory?id=${id}`)
    .then((res) => res.json())
    .then((data) => {
      const tabName = id == 1 ? "running" : id == 2 ? "training" : "lifestyle";

      renderShoes(tabName, data);
    })
    .catch((err) => console.error(err));
}

function renderShoes(tabId, shoes) {
  const box = document.getElementById(tabId);
  box.innerHTML = "";

  if (!shoes || shoes.length === 0) {
    box.innerHTML = "<p>No products.</p>";
    return;
  }

  shoes.forEach((item) => {
    box.innerHTML += `
            <div class="card product-card shadow-sm">
                <img src="${item.imageUrl}" class="card-img-top" />
                <div class="card-body">
                    <h5>${item.name}</h5>
                    <p class="fw-bold text-danger">${item.price.toLocaleString()} ₫</p>
                    <div class="btn-row mt-auto">
                        <a href="/Product/Detail/${
                          item.id
                        }" class="btn-view flex-fill">Chi tiết</a>
                        <a href="/Cart/Add/${
                          item.id
                        }" class="btn-cart flex-fill">Thêm</a>
                    </div>
                </div>
            </div>
        `;
  });
}

// animation for image
function changeImage(url) {
  const mainImg = document.getElementById("mainImage");

  // Hiệu ứng fade-out
  mainImg.style.opacity = 0;

  setTimeout(() => {
    mainImg.src = url;
    mainImg.style.opacity = 1; // fade-in
  }, 200);
}

// add hover for img
document.querySelectorAll(".thumb").forEach((img) => {
  img.addEventListener("mouseover", () => (img.style.opacity = 0.7));
  img.addEventListener("mouseout", () => (img.style.opacity = 1));
});
function changeImage(url) {
  document.getElementById("mainImage").src = url;
}

// Gọi ngay khi load trang
updateCartCount();


// Gửi request thêm vào giỏ
async function addToCart(id) {
    try {
        const response = await fetch(`/Cart/Add/${id}`, { method: "POST" });
        const result = await response.json();

        if (result.success) {
            updateCartCount(); // ⬅ Gọi hàm của bạn
        }
    } catch (error) {
        console.error("Lỗi thêm giỏ hàng:", error);
    }
}
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".add-to-cart").forEach(btn => {
        btn.addEventListener("click", async function () {
            const id = this.dataset.id;

            const response = await fetch(`/Cart/Add/${id}`, {
                method: "POST"
            });

            const result = await response.json();
            if (result.success) {
                updateCartCount(); // ⬅ GỌI HÀM UPDATE ICON
            }
        });
    });
});
function updateQuantity(id, quantity) {
    fetch("/Cart/UpdateQuantity", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: `id=${id}&quantity=${quantity}`
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            location.reload(); // reload lại trang để cập nhật tổng tiền
        }
    });
}
function buyNow(id) {
    fetch("/Cart/BuyNow/" + id, {
        method: "POST"
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            window.location.href = data.redirect;
        }
    });
}