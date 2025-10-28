// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {   // 🟢 ← START main DOMContentLoaded

    // =============================
    // VARIABLES
    // =============================
    const maxLevel = 20;

    const rows = [
        { classe: "classe_first", level: "level_first", row: "row_first" },
        { classe: "classe_second", level: "level_second", row: "row_second" },
        { classe: "classe_third", level: "level_third", row: "row_third" }
    ];

    // =============================
    // FUNCTIONS
    // =============================

    // --- Populate level selects based on maxLevel ---
    function populateLevels() {
        const selectedLevels = rows.map(r => parseInt(document.getElementById(r.level).value) || 0);
        const totalSelected = selectedLevels.reduce((a, b) => a + b, 0);

        rows.forEach(r => {
            const sel = document.getElementById(r.level);
            const currentValue = parseInt(sel.value) || 0;
            sel.innerHTML = "";
            const remaining = maxLevel - (totalSelected - currentValue);

            for (let i = 1; i <= remaining; i++) {
                const opt = document.createElement("option");
                opt.value = i;
                opt.textContent = i;
                if (i === currentValue) opt.selected = true;
                sel.appendChild(opt);
            }
        });
    }

    // --- Disable already chosen classes in other selects ---
    function updateAvailableClasses() {
        const selected = rows.map(r => document.getElementById(r.classe).value).filter(v => v);

        rows.forEach(r => {
            const sel = document.getElementById(r.classe);
            const options = sel.querySelectorAll("option");
            options.forEach(opt => {
                if (selected.includes(opt.value) && opt.value !== sel.value) {
                    opt.disabled = true;
                } else {
                    opt.disabled = false;
                }
            });
        });
    }

    // --- Remove a row ---
    function removeRow(rowId) {
        const row = document.getElementById(rowId);
        const selects = row.querySelectorAll("select");
        selects.forEach(s => s.value = ""); // reset select values
        row.hidden = true;
        updateAvailableClasses();
        populateLevels();
    }

    // --- Roll stats ---
    function createStats() {
        const totalArray = [];
        for (let i = 0; i < 6; i++) {
            const inputArray = [];
            for (let j = 0; j < 4; j++) {
                inputArray.push(Math.floor(Math.random() * 6) + 1);
            }
            inputArray.sort((a, b) => a - b);
            const sum = inputArray.slice(1).reduce((acc, curr) => acc + curr, 0);
            totalArray[i] = sum;
        }
        return totalArray;
    }

    // --- Prevent duplicate stats in selects ---
    function updateAvailableStats() {
        const selects = document.querySelectorAll(".ability-row select");
        if (selects.length === 0) return;

        const rolledValues = Array.from(selects[0].options)
            .map(opt => opt.value)
            .filter(v => v !== "");

        const rolledCount = {};
        rolledValues.forEach(v => {
            if (!rolledCount[v]) rolledCount[v] = 0;
            rolledCount[v]++;
        });

        const chosenCount = {};
        selects.forEach(select => {
            if (select.value !== "") {
                if (!chosenCount[select.value]) chosenCount[select.value] = 0;
                chosenCount[select.value]++;
            }
        });

        selects.forEach(select => {
            const currentValue = select.value;
            Array.from(select.options).forEach(opt => {
                if (opt.value === "") {
                    opt.disabled = false;
                    return;
                }
                const totalAvailable = rolledCount[opt.value] || 0;
                const alreadyChosen = chosenCount[opt.value] || 0;
                opt.disabled = !(opt.value === currentValue || alreadyChosen < totalAvailable);
            });
        });
        updateTotalStats();
    }

    function updateTotalStats() {
        const rows = document.querySelectorAll('.ability-row');

        rows.forEach(row => {
            const select = row.querySelector('select');
            const checkboxes = row.querySelectorAll('input[type="checkbox"]');
            const span = row.querySelector('span');

            let totalValue = 0;

            if (select.value !== "") {
                totalValue += parseInt(select.value);
            }

            checkboxes.forEach(cb => {
                if (cb.checked) {
                    if (cb.id.includes('-1-')) totalValue += 1;
                    else if (cb.id.includes('-2-')) totalValue += 2;
                }
            });

            span.textContent = totalValue >= 3 ? totalValue : "";
        });
    }

    // =============================
    // EVENT LISTENERS
    // =============================

    // --- Stats button ---
    const buttonStats = document.getElementById('button-stats');
    const displayStats = document.getElementById('stats-display');

    buttonStats.addEventListener('click', () => {
        const totalArray = createStats();
        displayStats.textContent = totalArray.join(', ');

        document.querySelectorAll(".ability-row select").forEach((select, index) => {
            select.innerHTML = "<option value=''></option>";
            totalArray.forEach(value => {
                const option = document.createElement("option");
                option.value = value;
                option.textContent = value;
                select.appendChild(option);
            });
            select.addEventListener("change", updateAvailableStats);
        });
        updateAvailableStats();
    });

    // --- Checkboxes +1/+2 per row ---
    const abilityRows = document.querySelectorAll('.ability-row');
    abilityRows.forEach(row => {
        const checkboxes = row.querySelectorAll('.scales');
        checkboxes.forEach(cb => {
            cb.addEventListener('change', () => {
                if (cb.checked) {
                    checkboxes.forEach(other => {
                        if (other !== cb) other.checked = false;
                    });
                }
                updateTotalStats();
            });
        });
    });

    const checkboxes = document.querySelectorAll('.scales');
    checkboxes.forEach(cb => {
        cb.addEventListener('change', () => {
            if (cb.checked) {
                const type = cb.id.split('-')[1]; // "1" or "2"
                checkboxes.forEach(other => {
                    if (other !== cb && other.id.includes(`scales-${type}-`)) {
                        other.checked = false;
                    }
                });
            }
            updateTotalStats();
        });
    });

    // --- Class & level event listeners ---
    rows.forEach(r => {
        const classeSel = document.getElementById(r.classe);
        const levelSel = document.getElementById(r.level);
        classeSel.addEventListener("change", updateAvailableClasses);
        levelSel.addEventListener("change", populateLevels);
    });

    // --- Show extra rows ---
    document.getElementById("classe2_button").addEventListener("click", () => {
        //alert("hgfhgfg");
        const rowSecond = document.getElementById("row_second");
        rowSecond.classList.toggle("visible");
        populateLevels();
    });

    document.getElementById("classe3_button").addEventListener("click", () => {
        const rowThird = document.getElementById("row_third");
        rowThird.classList.toggle("visible");
        populateLevels();
    });

    // --- Remove rows ---
    document.getElementById("remove_second").addEventListener("click", () => {
        document.getElementById("row_second").classList.remove("visible");
        populateLevels();
    });

    document.getElementById("remove_third").addEventListener("click", () => {
        document.getElementById("row_third").classList.remove("visible");
        populateLevels();
    });

    // --- Initial setup ---
    populateLevels();
    updateAvailableClasses();
    updateAvailableStats();


    // =============================
    // SLIDE JS CODE (moved inside same DOMContentLoaded)
    // =============================

    const slider = document.querySelector(".form-slider");
    const slides = document.querySelectorAll(".form-box");

    // --- Flip buttons depending on position ---
    function updateButtonFlip() {
        slides.forEach(slide => {
            const rect = slide.getBoundingClientRect();
            const centerX = window.innerWidth / 2;

            const btnLeft = slide.querySelector(".slide-btn-left");
            const btnRight = slide.querySelector(".slide-btn-right");

            const distanceFromCenter = Math.abs(rect.left + rect.width / 2 - centerX);
            const isOffCenter = distanceFromCenter > rect.width / 2;

            if (btnLeft) btnLeft.classList.toggle("flipped", isOffCenter);
            if (btnRight) btnRight.classList.toggle("flipped", isOffCenter);
        });
    }

    // --- Move to next/previous slide ---
    document.querySelectorAll(".slide-btn-right").forEach(btn => {
        btn.addEventListener("click", () => {
            const current = btn.closest(".form-box");
            const next = current.nextElementSibling;
            if (next) {
                slider.scrollTo({
                    left: next.offsetLeft,
                    behavior: "smooth" // 🔹 animation
                });
            }
        });
    });

    document.querySelectorAll(".slide-btn-left").forEach(btn => {
        btn.addEventListener("click", () => {
            const current = btn.closest(".form-box");
            const prev = current.previousElementSibling;
            if (prev) {
                slider.scrollTo({
                    left: prev.offsetLeft,
                    behavior: "smooth"
                });
            }
        });
    });

    // --- Update flip state on scroll/resize ---
    slider.addEventListener("scroll", updateButtonFlip);
    window.addEventListener("resize", updateButtonFlip);
    updateButtonFlip();

});   
