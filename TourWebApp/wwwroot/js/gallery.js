function openImage(src) {
    document.getElementById("imgModal").style.display = "flex";
    document.getElementById("modalImg").src = src;
}

function closeImage() {
    document.getElementById("imgModal").style.display = "none";
}

document.getElementById("imgModal").onclick = function (e) {
    if (e.target === this) closeImage();
}

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") closeImage();
});
