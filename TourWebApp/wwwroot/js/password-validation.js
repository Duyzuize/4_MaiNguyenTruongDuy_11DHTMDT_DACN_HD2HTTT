// ========== TOGGLE HIỂN / ẨN MẬT KHẨU ==========
function togglePassword(inputId, element) {
    const passInput = document.getElementById(inputId);
    const icon = element.querySelector("i");

    if (!passInput) return;

    if (passInput.type === "password") {
        passInput.type = "text";
        icon.classList.remove("fa-eye");
        icon.classList.add("fa-eye-slash");
    } else {
        passInput.type = "password";
        icon.classList.remove("fa-eye-slash");
        icon.classList.add("fa-eye");
    }
}


// ========== KIỂM TRA MẬT KHẨU MẠNH ==========
document.addEventListener("DOMContentLoaded", function () {
    const passInput = document.getElementById("MatKhau");
    const message = document.getElementById("passwordHelp");

    if (!passInput || !message) return;

    passInput.addEventListener("input", function () {
        const password = this.value;
        let errors = [];

        if (password.length < 8) errors.push("ít nhất 8 ký tự");
        if (!/[A-Z]/.test(password)) errors.push("1 chữ hoa");
        if (!/[a-z]/.test(password)) errors.push("1 chữ thường");
        if (!/[0-9]/.test(password)) errors.push("1 chữ số");
        if (!/[!@#$%^&*]/.test(password)) errors.push("1 ký tự đặc biệt");

        if (errors.length > 0) {
            message.innerHTML = "Mật khẩu cần: " + errors.join(", ");
            message.style.color = "#dc2626";
        } else {
            message.innerHTML = "✔ Mật khẩu mạnh";
            message.style.color = "green";
        }
    });
});


// ========== CHẶN SUBMIT NẾU MẬT KHẨU YẾU ==========
function validatePassword() {
    const pass = document.getElementById("MatKhau").value;

    if (pass.length < 8 ||
        !/[A-Z]/.test(pass) ||
        !/[a-z]/.test(pass) ||
        !/[0-9]/.test(pass) ||
        !/[!@#$%^&*]/.test(pass)) {

        alert("❌ Mật khẩu chưa đủ mạnh!");
        return false;
    }

    return true;
}
