document.addEventListener("DOMContentLoaded", function () {
    const dropZone = document.getElementById("drop-zone");
    const fileInput = document.getElementById("fileInput");

    let fileList = [];

    dropZone.addEventListener("click", () => fileInput.click());

    dropZone.addEventListener("dragover", e => {
        e.preventDefault();
        dropZone.classList.add("bg-light");
    });

    dropZone.addEventListener("dragleave", () => {
        dropZone.classList.remove("bg-light");
    });

    dropZone.addEventListener("drop", e => {
        e.preventDefault();
        dropZone.classList.remove("bg-light");

        addFiles(e.dataTransfer.files);
    });

    fileInput.addEventListener("change", () => {
        addFiles(fileInput.files);
    });

    function addFiles(files) {
        for (let file of files) {
            if (!file.type.startsWith("image/")) continue;
            fileList.push(file);
        }
        renderPreview();
        syncInputFiles();
    }

    function renderPreview() {
        dropZone.querySelectorAll(".preview-box").forEach(e => e.remove());

        fileList.forEach((file, index) => {
            const reader = new FileReader();

            reader.onload = e => {
                const box = document.createElement("div");
                box.className = "preview-box";
                box.draggable = true;
                box.dataset.index = index;

                box.innerHTML = `
                    <img src="${e.target.result}">
                    <span class="remove-btn">✖</span>
                `;

                // XÓA ẢNH
                box.querySelector(".remove-btn").onclick = () => {
                    fileList.splice(index, 1);
                    renderPreview();
                    syncInputFiles();
                };

                enableDragSort(box);
                dropZone.appendChild(box);
            };

            reader.readAsDataURL(file);
        });
    }

    function syncInputFiles() {
        const dataTransfer = new DataTransfer();
        fileList.forEach(file => dataTransfer.items.add(file));
        fileInput.files = dataTransfer.files;
    }

    // KÉO SẮP XẾP
    function enableDragSort(item) {
        item.addEventListener("dragstart", e => {
            e.dataTransfer.setData("text/plain", item.dataset.index);
        });

        item.addEventListener("dragover", e => e.preventDefault());

        item.addEventListener("drop", e => {
            e.preventDefault();
            const from = e.dataTransfer.getData("text/plain");
            const to = item.dataset.index;

            const moved = fileList.splice(from, 1)[0];
            fileList.splice(to, 0, moved);

            renderPreview();
            syncInputFiles();
        });
    }
});
