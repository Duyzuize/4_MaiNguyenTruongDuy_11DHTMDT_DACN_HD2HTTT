/* ============================
   REVEAL EFFECT
============================ */
(() => {
    const els = document.querySelectorAll('.reveal');
    if (!('IntersectionObserver' in window)) {
        els.forEach(el => el.classList.add('show'));
        return;
    }

    const io = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.classList.add('show');
                io.unobserve(e.target);
            }
        });
    }, { threshold: 0.15 });

    els.forEach(el => io.observe(el));
})();

/* ============================
   NAVBAR STICKY (fix class name)
============================ */
(() => {
    const nav = document.querySelector('.navbar-main');  // 🔥 FIXED: đúng class mới
    if (!nav) return;

    const toggle = () => {
        nav.classList.toggle('stuck', window.scrollY > 8);
    };

    toggle();
    window.addEventListener('scroll', toggle);
})();
