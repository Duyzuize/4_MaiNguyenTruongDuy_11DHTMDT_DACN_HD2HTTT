const rangeMin = document.querySelector(".range-min");
const rangeMax = document.querySelector(".range-max");
const track = document.querySelector(".slider-track");

const inputMin = document.getElementById("giaminInput");
const inputMax = document.getElementById("giamaxInput");

function updateSlider() {
    let minVal = parseInt(rangeMin.value);
    let maxVal = parseInt(rangeMax.value);

    inputMin.value = minVal.toLocaleString("vi-VN") + " đ";
    inputMax.value = maxVal.toLocaleString("vi-VN") + " đ";

    let minPercent = (minVal / rangeMin.max) * 100;
    let maxPercent = (maxVal / rangeMax.max) * 100;

    track.style.background = `linear-gradient(
        to right,
        #ddd ${minPercent}%,
        #0d6efd ${minPercent}%,
        #0d6efd ${maxPercent}%,
        #ddd ${maxPercent}%
    )`;
}

rangeMin.oninput = () => {
    if (+rangeMin.value > +rangeMax.value) rangeMin.value = rangeMax.value;
    updateSlider();
};

rangeMax.oninput = () => {
    if (+rangeMax.value < +rangeMin.value) rangeMax.value = rangeMin.value;
    updateSlider();
};

// Format khi submit form
document.querySelector("form").onsubmit = () => {
    inputMin.value = inputMin.value.replace(/\D/g, "");
    inputMax.value = inputMax.value.replace(/\D/g, "");
};

updateSlider();

const input = document.getElementById("diadiemInput");
const box = document.getElementById("goiYBox");

input.addEventListener("input", function () {
    let keyword = input.value.trim();

    if (keyword.length < 1) {
        box.style.display = "none";
        return;
    }

    fetch(`/Tour/GoiyDiaDiem?keyword=${encodeURIComponent(keyword)}`)
        .then(res => res.json())
        .then(data => {
            if (!data || data.length === 0) {
                box.style.display = "none";
                return;
            }

            box.innerHTML = "";

            data.forEach(item => {
                let div = document.createElement("div");
                div.classList.add("autocomplete-item");
                div.textContent = item;

                div.onclick = function () {
                    input.value = item;
                    box.style.display = "none";
                };

                box.appendChild(div);
            });

            box.style.display = "block";
        });
});

// Ấn bên ngoài → ẩn box
document.addEventListener("click", function (e) {
    if (!input.contains(e.target) && !box.contains(e.target)) {
        box.style.display = "none";
    }
});
