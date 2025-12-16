document.addEventListener("DOMContentLoaded", function () {

    flatpickr("#ngayThem", {
        dateFormat: "Y-m-d",
        minDate: "today"
    });

    flatpickr("#gioThem", {
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true
    });
});

function khoiTaoSua() {
    flatpickr("#ngaySua", {
        dateFormat: "Y-m-d",
        minDate: "today"
    });

    flatpickr("#gioSua", {
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true
    });
}

function moModalSua(id, ngay, gio, soCho) {

    document.getElementById("editId").value = id;
    document.getElementById("ngaySua").value = ngay;
    document.getElementById("gioSua").value = gio;
    document.getElementById("soChoSua").value = soCho;

    khoiTaoSua();

    var modal = new bootstrap.Modal(document.getElementById('modalSuaLich'));
    modal.show();
}
