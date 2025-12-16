document.addEventListener("DOMContentLoaded", function () {

    const giaGocInput = document.getElementById("giaGoc");
    const phanTramInput = document.getElementById("phanTramGiam");
    const giaKhuyenMaiInput = document.getElementById("giaKhuyenMai");

    function formatMoney(value) {
        if (!value) return "";
        return Number(value).toLocaleString("vi-VN") + " đ";
    }

    function unformatMoney(value) {
        return value.replace(/[^\d]/g, "");
    }

    function tinhGiaKhuyenMai() {
        let giaGoc = parseInt(unformatMoney(giaGocInput.value)) || 0;
        let phanTram = parseInt(phanTramInput.value) || 0;

        let giaMoi = giaGoc;

        if (phanTram > 0) {
            giaMoi = giaGoc - (giaGoc * phanTram / 100);
        }

        giaKhuyenMaiInput.value = formatMoney(Math.round(giaMoi));
    }

    giaGocInput.addEventListener("input", function () {
        this.value = formatMoney(unformatMoney(this.value));
        tinhGiaKhuyenMai();
    });

    phanTramInput.addEventListener("input", tinhGiaKhuyenMai);
});
