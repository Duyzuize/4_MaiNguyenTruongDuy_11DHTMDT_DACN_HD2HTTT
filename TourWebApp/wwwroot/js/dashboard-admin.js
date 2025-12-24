// LOADING FAKE
window.addEventListener("load", () => {
    setTimeout(() => {
        document.getElementById("loadingScreen").style.display = "none";
    }, 700);
});

// ANIMATION ĐẾM SỐ
document.querySelectorAll('.counter').forEach(counter => {
    const target = +counter.dataset.count;
    let current = 0;
    const step = Math.ceil(target / 60);

    const run = setInterval(() => {
        current += step;
        if (current >= target) {
            counter.textContent = target;
            clearInterval(run);
        } else {
            counter.textContent = current;
        }
    }, 20);
});

// BIỂU ĐỒ TRẠNG THÁI
const chartEl = document.getElementById('chartTrangThai');

if (chartEl) {
    const dangBan = chartEl.dataset.dangban;
    const ngungBan = chartEl.dataset.ngungban;

    new Chart(chartEl, {
        type: 'doughnut',
        data: {
            labels: ['Đang bán', 'Ngưng bán'],
            datasets: [{
                data: [dangBan, ngungBan],
                backgroundColor: ['#2ecc71','#e74c3c'],
                borderWidth: 0
            }]
        },
        options: {
            cutout: '70%',
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}
// ================= BIỂU ĐỒ CỘT: ĐƠN THEO TRẠNG THÁI THANH TOÁN =================
const ctxDonTrangThai = document.getElementById('chartDonTrangThai');

if (ctxDonTrangThai) {
    new Chart(ctxDonTrangThai, {
        type: 'bar',
        data: {
            labels: ['Đã thanh toán', 'Chưa thanh toán', 'Hết hạn'],
            datasets: [{
                label: 'Số đơn',
                data: [
                    window.soDonDaThanhToan || 0,
                    window.soDonChuaThanhToan || 0,
                    window.soDonHetHan || 0
                ],
                backgroundColor: ['#28a745', '#ffc107', '#6c757d']
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { precision: 0 }
                }
            }
        }
    });
}


// ================= BIỂU ĐỒ SÓNG: ĐƠN THEO NGÀY =================
const ctxDonTheoNgay = document.getElementById('chartDonTheoNgay');

if (ctxDonTheoNgay) {
    new Chart(ctxDonTheoNgay, {
        type: 'line',
        data: {
            labels: window.labelNgay || [],
            datasets: [{
                label: 'Số đơn',
                data: window.dataNgay || [],
                borderColor: '#0d6efd',
                backgroundColor: 'rgba(13,110,253,0.15)',
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { precision: 0 }
                }
            }
        }
    });
}
    let doanhThuChart;

document.addEventListener("DOMContentLoaded", function () {
    const el = document.getElementById("chartDoanhThuNgay");
    if (!el) return;

    const labels = window.labelDoanhThuNgay || [];
    const data = window.dataDoanhThuNgay || [];

    if (doanhThuChart) doanhThuChart.destroy();

    doanhThuChart = new Chart(el, {
        type: "line",
        data: {
            labels,
            datasets: [{
                label: "Doanh thu (đ)",
                data,
                tension: 0.3,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: true } },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: (value) => Number(value).toLocaleString("vi-VN") + " đ"
                    }
                }
            }
        }
    });
});
