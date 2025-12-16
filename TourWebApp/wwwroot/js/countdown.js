function startCountdown(expireTime, elementId) {
    const endTime = new Date(expireTime).getTime();

    const timer = setInterval(function () {
        const now = new Date().getTime();
        const distance = endTime - now;

        if (distance <= 0) {
            clearInterval(timer);

            document.getElementById(elementId).innerHTML = "00:00";

            // ✅ TỰ ĐỘNG RELOAD KHI HẾT GIỜ
            alert("Đơn đã hết thời gian thanh toán và sẽ bị hủy!");
            location.reload();   // gọi lại server => trigger hủy đơn
            return;
        }

        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

        document.getElementById(elementId).innerHTML =
            (minutes < 10 ? "0" : "") + minutes + ":" +
            (seconds < 10 ? "0" : "") + seconds;
    }, 1000);
}
