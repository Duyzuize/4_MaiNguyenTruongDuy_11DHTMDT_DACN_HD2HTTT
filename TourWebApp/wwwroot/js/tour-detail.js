document.addEventListener("DOMContentLoaded", function () {

    // ====== TAB + SECTION ======
    const tabs = document.querySelectorAll(".tour-tab-nav li");
    const sections = document.querySelectorAll(".tour-section");

    if (sections.length > 0 && tabs.length > 0) {
        sections.forEach(s => s.style.display = "none");

        const firstId = tabs[0].getAttribute("data-target");
        const firstSection = document.getElementById(firstId);
        if (firstSection) {
            firstSection.style.display = "block";
        }

        tabs.forEach(tab => {
            tab.addEventListener("click", function () {
                const targetId = this.getAttribute("data-target");
                const target = document.getElementById(targetId);
                if (!target) return;

                tabs.forEach(t => t.classList.remove("active"));
                this.classList.add("active");

                sections.forEach(s => s.style.display = "none");
                target.style.display = "block";

                window.scrollTo({
                    top: target.offsetTop - 100,
                    behavior: "smooth"
                });
            });
        });
    }

    // ====== CLICK CHỌN LỊCH KHỞI HÀNH — FIX TRÙNG ======

    const rows = document.querySelectorAll(".lich-row");
    const hiddenInput = document.getElementById("selectedLich");
    const soNgayInput = document.getElementById("songay");
    const soNgay = parseInt(document.getElementById("songay")?.value || 0);

rows.forEach(row => {
    row.addEventListener("click", () => {

        // highlight
        rows.forEach(r => r.classList.remove("lich-selected"));
        row.classList.add("lich-selected");

        // set hidden input
        document.getElementById("selectedLich").value = row.dataset.idlich;

        // update info
        document.getElementById("thoigian_ngay").innerText = row.dataset.ngay;
        document.getElementById("thoigian_gio").innerText = row.dataset.gio;
        document.getElementById("thoigian_trangthai").innerText = row.dataset.trangthai;
        document.getElementById("thoigian_socho").innerText = row.dataset.socho;

        // ⭐ TÍNH NGÀY KẾT THÚC ⭐
        if (soNgay > 0) {
            const p = row.dataset.ngay.split("/"); // dd/MM/yyyy
            const start = new Date(p[2], p[1] - 1, p[0]);
            const end = new Date(start);
            end.setDate(start.getDate() + soNgay - 1);

            document.getElementById("thoigian_ketthuc").innerText =
                end.toLocaleDateString("vi-VN");
        }
    });
});


    // ====== NÚT SCROLL ======
    document.querySelectorAll(".scroll-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            const targetId = btn.getAttribute("data-target");
            const tab = document.querySelector(`.tour-tab-nav li[data-target='${targetId}']`);
            if (tab) tab.click();
        });
    });

    // ====== ĐỔI ẢNH THUMBNAIL ======
    document.querySelectorAll(".tour-thumb").forEach(img => {
        img.addEventListener("click", () => {
            const main = document.getElementById("main-img");
            if (main) {
                main.src = img.src;
            }
        });
    });

    // ====== TÍNH GIÁ TOUR ======
    const calcBox = document.querySelector(".tour-calc-box");
    if (calcBox) {
        const adultPrice = parseFloat(calcBox.dataset.adult) || 0;
        const childPrice = parseFloat(calcBox.dataset.child) || 0;
        const babyPrice  = parseFloat(calcBox.dataset.baby) || 0;

        const adultInput = document.getElementById("adultQty");
        const childInput = document.getElementById("childQty");
        const babyInput  = document.getElementById("babyQty");

        const totalAmountEl = document.getElementById("totalAmount");
        const totalGuestEl  = document.getElementById("totalGuest");

        function formatMoney(v) {
            return v.toLocaleString("vi-VN") + " đ";
        }

        function recalc() {
            const a = parseInt(adultInput.value) || 0;
            const c = parseInt(childInput.value) || 0;
            const b = parseInt(babyInput.value) || 0;

            const total = a * adultPrice + c * childPrice + b * babyPrice;

            if (totalAmountEl) totalAmountEl.textContent = formatMoney(total);
            if (totalGuestEl) totalGuestEl.textContent = (a + c + b).toString();
        }

        function bindQty(minusId, inputId, plusId) {
            const minusBtn = document.getElementById(minusId);
            const input    = document.getElementById(inputId);
            const plusBtn  = document.getElementById(plusId);

            if (!input) return;

            if (minusBtn) {
                minusBtn.addEventListener("click", () => {
                    let v = parseInt(input.value) || 0;
                    if (v > 0) {
                        input.value = v - 1;
                        recalc();
                    }
                });
            }

            if (plusBtn) {
                plusBtn.addEventListener("click", () => {
                    let v = parseInt(input.value) || 0;
                    input.value = v + 1;
                    recalc();
                });
            }

            input.addEventListener("change", recalc);
        }

        bindQty("adultMinus", "adultQty", "adultPlus");
        bindQty("childMinus", "childQty", "childPlus");
        bindQty("babyMinus", "babyQty", "babyPlus");

        recalc();
    }

}); // END DOMContentLoaded


// ===================================================
// GO TO BOOKING — HÀM CHUẨN, KHÔNG TRÙNG!!
// ===================================================
function goToBooking() {

    let idLich = document.getElementById("selectedLich").value;
    let calcBox = document.querySelector(".tour-calc-box");

    let idTour = calcBox.dataset.idtour;
    let user   = calcBox.dataset.user;

    // Số lượng
    let adult = parseInt(document.getElementById("adultQty").value) || 0;
    let child = parseInt(document.getElementById("childQty").value) || 0;
    let baby  = parseInt(document.getElementById("babyQty").value)  || 0;

    // ⭐ LẤY GIÁ TỪ DATASET — CHUẨN NHẤT
    let adultPrice = parseInt(calcBox.dataset.adult) || 0;
    let childPrice = parseInt(calcBox.dataset.child) || 0;
    let babyPrice  = parseInt(calcBox.dataset.baby)  || 0;

    // ⭐ TÍNH TOTAL CHUẨN — KHÔNG BAO GIỜ BỊ 0
    let total = adult * adultPrice +
                child * childPrice +
                baby  * babyPrice;

    if (!idLich) {
        alert("Vui lòng chọn lịch khởi hành!");
        return;
    }

    // Nếu chưa login
    if (!user) {
        const loginPopup = new bootstrap.Modal(document.getElementById("loginWarningModal"));
        document.getElementById("btnGoLogin").href =
            `/TaiKhoan/DangNhap?returnUrl=/DatTour/NhapThongTin?idTour=${idTour}&idLich=${idLich}&adult=${adult}&child=${child}&baby=${baby}&total=${total}`;
        loginPopup.show();
        return;
    }

    // Login rồi → đi tiếp
    window.location.href =
        `/DatTour/NhapThongTin?idTour=${idTour}&idLich=${idLich}` +
        `&adult=${adult}&child=${child}&baby=${baby}&total=${total}`;
}
function changeMainImage(src) {
    const mainImg = document.getElementById("main-img");
    mainImg.src = src;
}
