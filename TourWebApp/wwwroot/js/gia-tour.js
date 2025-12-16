const giaNguoiLon = document.getElementById("giaNguoiLon");
const phanTram = document.getElementById("phanTramTreEm");
const percentLabel = document.getElementById("percentLabel");
const giaTrePreview = document.getElementById("giaTreEmPreview");
const coPhiEmBe = document.getElementById("coPhiEmBe");
const giaEmBeInput = document.getElementById("giaEmBe");

function tinhGiaTre() {
    let gia = parseFloat(giaNguoiLon.value || 0);
    let phanTramValue = parseInt(phanTram.value);

    percentLabel.innerText = phanTramValue + "%";

    let giaTre = gia - (gia * phanTramValue / 100);
    giaTrePreview.value = Math.round(giaTre).toLocaleString() + " đ";
}

phanTram.addEventListener("input", tinhGiaTre);
giaNguoiLon.addEventListener("input", tinhGiaTre);

coPhiEmBe.addEventListener("change", function () {
    if (this.value == "1") {
        giaEmBeInput.removeAttribute("readonly");
    } else {
        giaEmBeInput.value = 0;
        giaEmBeInput.setAttribute("readonly", true);
    }
});
