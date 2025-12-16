// Toggle sidebar trên mobile
document.addEventListener("DOMContentLoaded", function () {
    const btnToggle = document.getElementById("btnToggleSidebar");
    const sidebar = document.querySelector(".admin-sidebar");

    if (btnToggle && sidebar) {
        btnToggle.addEventListener("click", function () {
            sidebar.classList.toggle("show");
        });
    }

    // Active menu theo URL
    const links = document.querySelectorAll(".admin-menu a[data-menu]");
    const path = window.location.pathname.toLowerCase();

    links.forEach(link => {
        const section = link.getAttribute("data-menu");
        if (!section) return;

        if (path.includes(section)) {
            link.classList.add("active");
        }
    });
});
