// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {  

    // =============================
    // VARIABLES
    // =============================
    const maxLevel = 20;

    const rows = [
        { classe: "classe_first", level: "level_first", subclasse: "subclasse_first" , row: "row_first" },
        { classe: "classe_second", level: "level_second", subclasse: "subclasse_second", row: "row_second" },
        { classe: "classe_third", level: "level_third", subclasse: "subclasse_third", row: "row_third" }
    ];

    // =============================
    // FUNCTIONS
    // =============================

    // --- Populate level selects based on maxLevel ---
    function populateLevels() {
        const visibleRows = rows.filter(r => {
            const rowEl = document.getElementById(r.row);
            return rowEl && rowEl.classList.contains("visible");
        });

        const selectedLevels = visibleRows.map(r => parseInt(document.getElementById(r.level).value) || 0);
        const totalSelected = selectedLevels.reduce((a, b) => a + b, 0);

        rows.forEach(r => {
            const sel = document.getElementById(r.level);
            const rowEl = document.getElementById(r.row);

            if (!rowEl.classList.contains("visible")) {
                sel.innerHTML = '<option value=""></option>';
                return;
            }

            const currentValue = parseInt(sel.value);
            sel.innerHTML = "";
            const remaining = maxLevel - (totalSelected - (currentValue || 0));

            for (let i = 1; i <= remaining; i++) {
                const opt = document.createElement("option");
                opt.value = i;
                opt.textContent = i;
               
                if (currentValue && i === currentValue) opt.selected = true;
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
    function updateAvailableSubClasses(row) {
        const classSelect = document.getElementById(row.classe);
        const levelSelect = document.getElementById(row.level);
        const subSelect = document.getElementById(row.subclasse);

        subSelect.innerHTML = '';

        const classId = classSelect.value;
        const level = parseInt(levelSelect.value) || 0;

        if (classId && subClassesDict[classId]) {
            subClassesDict[classId].forEach(sub => {
                if (level >= sub.LevelToStart) {
                    const opt = document.createElement("option");
                    opt.value = sub.SubclassId;
                    opt.textContent = sub.SubName;
                    subSelect.appendChild(opt);
                }
            });
        }
    }

    function updateLevelIfClassNull(row) {
        const classSelect = document.getElementById(row.classe);
        const levelSelect = document.getElementById(row.level);

        if (classSelect.innerHTML === "") {
            levelSelect.innerHTML = "";
            levelSelect.textContent = "";
        }
    }
    // --- Remove a row ---
    function removeRow(rowId) {
        const row = document.getElementById(rowId);
        const selects = row.querySelectorAll("select");
        selects.forEach(s => s.value = ""); 
        updateAvailableClasses();
        //
        //populateLevels();
    }

    // ----- Show Subspecies

    function updateAvailableSubRace() {
        const RaceSelect = document.getElementById("species");
        const subRaceSelect = document.getElementById("subspecies");

        const RaceId = RaceSelect.value;

        subRaceSelect.innerHTML = "";

        if (RaceId && subRacesDict[RaceId]) {
            subRacesDict[RaceId].forEach(sub => {
                const opt = document.createElement("option");
                opt.value = sub.SubRaceId;
                opt.textContent = sub.SubName;
                subRaceSelect.appendChild(opt);
            });
        }
    }

    // --- abilities --
    document.addEventListener("DOMContentLoaded", () => {
        const abilities = ["STR", "DEX", "CON", "INT", "WIS", "CHA"];

        abilities.forEach(a => {
            const span = document.getElementById(`total-${a}`);
            const input = document.getElementById(`input-total-${a}`);

            if (span && input) {
                input.value = span.innerText;
            } 
        });
    });
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
            const ability = span.id.split('-')[1];
            const input = document.getElementById(`total_${ability}`);

            let totalValue = 0;

            if (select.value !== "") {
                totalValue += parseInt(select.value);
            }

            checkboxes.forEach(cb => {
                if (cb.checked) {
                    if (cb.id.includes('-1-')) totalValue += 1;
                    else if (cb.id.includes('-2-')) totalValue += 2;
                }
            })
            if (totalValue >= 3) {
                span.textContent = totalValue < 10 ? "0" + totalValue : String(totalValue);
            } else {
                span.textContent = "";
            }
            input.value = input.value > 20 ? 20 : input.value;
            input.value = span.textContent;
            statsInConsole(span);
        });
    }

    // limt skills to 4
    function limitSkillsTo4() {
        const divSkills = document.querySelectorAll('.skills');
        const background = document.getElementById('background');

        divSkills.forEach(container => {
            container.addEventListener('change', () => {
                const checkedBoxes = container.querySelectorAll('input[type="checkbox"]:checked');
                let counter = checkedBoxes.length;

                if ((counter >= 2 && background.value == "") || (counter >= 4 && background.value != ""))
                {
                    const unchecked = container.querySelectorAll('input[type="checkbox"]:not(:checked)');
                    unchecked.forEach(input => {
                        input.disabled = true;
                    });
                }
                else {
                    const allInputs = container.querySelectorAll('input[type="checkbox"]');
                    allInputs.forEach(input => {
                        input.disabled = false;
                    });
                }
            });
        });
    }
    // update skills in background

    let ancientBackgroundText = "";

    function backgroundSkillsUpdate() {
        const selectedOptionText = this.options[this.selectedIndex].text;

        const extract = (text) => {
            if (!text || !text.includes('(')) return [];
            const rawPart = text.split('(')[1].split(')')[0];
            return rawPart.split('/').map(skill =>
                skill.trim().replace(/\s+/g, '').toLowerCase()
            );
        };

        const oldSkills = extract(ancientBackgroundText);
        const newSkills = extract(selectedOptionText);
        const checkboxes = document.querySelectorAll('input[type="checkbox"]');

        checkboxes.forEach(checkbox => {
            const cleanId = checkbox.id.trim().replace(/\s+/g, '').toLowerCase();

            if (oldSkills.includes(cleanId)) {
                checkbox.checked = false;
            }
            if (newSkills.includes(cleanId)) {
                checkbox.checked = true;
            }
        });

        ancientBackgroundText = selectedOptionText;
        limitSkillsTo4();
    }

    limitSkillsTo4();


    // put stat in console
    function statsInConsole(span) {
        console.log(span.textContent);
    }

        function pickRandomClass(selectId) {
        const selectElem = document.getElementById(selectId);
        selectElem.value = "";

            console.log(selectElem);
        let options = selectElem.dataset.options.map(o => {
            const [value, text] = o.split(":");
            return { value, text };
        });
        const selected = rows
            .filter(r => r.classe !== selectId) 
            .map(r => document.getElementById(r.classe).value)
            .filter(v => v);
        let availableOptions = options.filter(o => !selected.includes(o.value));

        if (availableOptions.length === 0) return;
        const randomIndex = Math.floor(Math.random() * availableOptions.length);
        selectElem.value = availableOptions[randomIndex].value;
    }


    // FEATS dialog /////////////

    const dialog = document.getElementById('featDialog');

    const menu = document.getElementById('view-menu');
    const statsView = document.getElementById('view-stats');
    const featsView = document.getElementById('view-feats');
    const allCheckboxes = featsView.querySelectorAll('input[type="checkbox"]');
    const allStatsSelect = statsView.querySelectorAll('input[type="label"]');
    let totalSlots = 0;
    function getTotalFeatsSlot() {
       

        if (getSpentSlots() < 0) {
            allCheckboxes.checked = false;
            allStatsSelect.values = 0;
        }
        const lvl1 = document.getElementById('classe_first')?.value !== "" ? (parseInt(document.getElementById('level_first')?.value) || 0) : 0;
        const lvl2 = document.getElementById('classe_second')?.value !== "" ? (parseInt(document.getElementById('level_second')?.value) || 0) : 0;
        const lvl3 = document.getElementById('classe_third')?.value !== "" ? (parseInt(document.getElementById('level_third')?.value) || 0) : 0;
        totalSlots = Math.floor(lvl1 / 4) + Math.floor(lvl2 / 4) + Math.floor(lvl3 / 4);
        return totalSlots;

    }

    function verifyFeatsSlotValue() {
        const totalPointsAllowed = getTotalFeatsSlot() * 2;

        const featpoints = document.querySelectorAll('.feat-cb:checked').length * 2;


        let statpoints = 0;
        const statInputs = document.querySelectorAll('.stat-increment');
        statInputs.forEach(input => {
            statpoints += parseInt(input.value) || 0;
        });

        if ((totalPointsAllowed - featpoints - statpoints) < 0) {
           


            document.querySelectorAll('.feat-cb').forEach(cb => {
                cb.checked = false;
            });

            statInputs.forEach(input => {
                input.value = 0;
            });
            //updateRemainingSlotsDisplay();
        }
    }
    function updateLimitsStatsBonus() {
            
       const totalPointsAllowed = getTotalFeatsSlot() * 2;
       const featpoints = document.querySelectorAll('.feat-cb:checked').length * 2;

    let statpoints = 0;
    document.querySelectorAll('.stat-increment').forEach(input => {
        statpoints += parseInt(input.value) || 0;
    });



    document.querySelectorAll('.stat-increment').forEach(input => {
        const currentval = parseInt(input.value) || 0;
        input.max = currentval + (totalPointsAllowed - featpoints - statpoints);
        input.min = 0;
    });
        document.getElementById('slots-remaining').innerText = Math.floor((totalPointsAllowed - featpoints - statpoints) / 2);
    }

        // Function to switch views
    function showView(viewName) {
        menu.style.display = 'none';
        statsView.style.display = 'none';
        featsView.style.display = 'none';

        if (viewName === 'menu') menu.style.display = 'block';
        if (viewName === 'stats') statsView.style.display = 'block';
        if (viewName === 'feats') featsView.style.display = 'block';
    }

    function getSpentSlots() {
        const checkboxFeats = document.querySelectorAll('.feat-cb');
        const checkedFeats = document.querySelectorAll('.feat-cb:checked').length;

        let statPoints = 0;
        document.querySelectorAll('.stat-increment').forEach(input => {
            statPoints += parseInt(input.value) || 0;
        });

        return checkedFeats + Math.ceil(statPoints / 2);
    }

    function syncFeatState() {
        const spent = getSpentSlots();
        const isFull = (spent >= getTotalFeatsSlot());
        const allCheckboxes = featsView.querySelectorAll('input[type="checkbox"]');
        const allStatsSelect = statsView.querySelectorAll('input[type="label"]');
  
        
        allCheckboxes.forEach(input => {
            if (!input.checked) {
                input.disabled = isFull;
            }
        });

    }
  
    dialog.addEventListener('change', () => {
        totalSlots = getTotalFeatsSlot() / 2;
        syncFeatState();
    });

    document.getElementById('stats-options').addEventListener('input', updateLimitsStatsBonus);

    document.getElementById('openFeats').addEventListener('click', () => {
        totalSlots = getTotalFeatsSlot() / 2;
        verifyFeatsSlotValue();
        getTotalFeatsSlot();
        updateLimitsStatsBonus();
        syncFeatState();  
        showView('menu');
        dialog.showModal();
    });

    document.getElementById('btn-to-stats').addEventListener('click', () => showView('stats'));
    document.getElementById('btn-to-feats').addEventListener('click', () => showView('feats'));
    document.querySelectorAll('.btn-back').forEach(btn => {
        btn.addEventListener('click', () => {
            totalSlots = getTotalFeatsSlot() / 2;
            verifyFeatsSlotValue();
            getTotalFeatsSlot();
            updateLimitsStatsBonus();
            syncFeatState();
            showView('menu');
        });
    });

    // =============================
    // EVENT LISTENERS
    // =============================

    // --- Stats button ---
    const buttonStats = document.getElementById('button-stats');
    const displayStats = document.getElementById('stats-display');

    buttonStats?.addEventListener('click', () => {

        const totalArray = createStats();
        const totalSpan = document.getElementById('totalSpan');
        totalSpan.textContent = `Total: ${totalArray.reduce((acc, curr) => acc + curr, 0) }`;
        displayStats.textContent = totalArray.join(', ');

        document.querySelectorAll(".ability-row select").forEach((select, index) => {
            select.innerHTML = "<option value=''>00</option>";
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

    // feats finish

    document.getElementById('confirmFeats').addEventListener('click', () => {
        const spent = getSpentSlots();

        if (spent > totalSlots) {
            alert(`You have used ${spent} slots, but you only have ${totalSlots} available!`);
            return;
        }

        const checkedFeats = Array.from(document.querySelectorAll('.feat-cb:checked'))
            .map(cb => cb.value);

        const statIncreases = {};
        document.querySelectorAll('.stat-increment').forEach(input => {
            if (parseInt(input.value) > 0) {
                statIncreases[input.name] = parseInt(input.value);
            }
        });

        const finalData = {
            feats: checkedFeats,
            stats: statIncreases
        };

        const hiddenInput = document.getElementById('chosenFeats');
        hiddenInput.value = JSON.stringify(finalData);

        console.log("Saving data:", finalData);
        dialog.close();
    });

    // ---- species&Subspecies ---

    const SelectRace = document.getElementById("species");
    const SelectSubRace = document.getElementById("subspecies");

    if (!SelectRace || !SelectSubRace) {
        console.error("IDs introuvables : #species ou #subspecies");
        return;
    }

    SelectRace.addEventListener("change", () => {
        SelectSubRace.disabled = false;
        updateAvailableSubRace();
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
                const type = cb.id.split('-')[1]; 
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

        // When class changes
        classeSel.addEventListener("change", () => {
            updateAvailableClasses(); 
            updateAvailableSubClasses(r);
            updateLevelIfClassNull(r)
            populateLevels();
            syncFeatState()
        });

        // When level changes
        levelSel.addEventListener("change", () => {
            populateLevels();
            updateAvailableSubClasses(r);
            updateLimitsStatsBonus();
            syncFeatState()
        });
    });

    // --- Show extra rows ---
      document.getElementById("classe2_button").addEventListener("click", () => {
        const rowSecond = document.getElementById("row_second");
        rowSecond.classList.add("visible");
        //pickRandomClass("classe_second");
    });

    document.getElementById("remove_second").addEventListener("click", () => {
        const rowSecond = document.getElementById("row_second");
        rowSecond.classList.remove("visible"); 
        removeRow("row_second");
        populateLevels();
    });

    document.getElementById("classe3_button").addEventListener("click", () => {
        const rowThird = document.getElementById("row_third");
        rowThird.classList.add("visible");
        //pickRandomClass("classe_third");
    });

    document.getElementById("remove_third").addEventListener("click", () => {
        const rowThird = document.getElementById("row_third");
        rowThird.classList.remove("visible");
        removeRow("row_third");
        populateLevels();
    });

    // enable background skills

    document.getElementById("background").addEventListener("change", backgroundSkillsUpdate);

    
    // --- Initial setup ---
    //populateLevels();


    updateAvailableClasses();
    updateAvailableStats();
    updateAvailableSubRace();



    // =============================
    // SLIDE JS CODE 
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

            // Calculate if the slide is currently centered
            const slideCenter = rect.left + rect.width / 2;
            const distanceFromCenter = Math.abs(slideCenter - centerX);

            // If more than 10% off center, consider it "flipped"
            const isOffCenter = distanceFromCenter > (window.innerWidth * 0.1);

            if (btnLeft) btnLeft.classList.toggle("flipped", isOffCenter);
            if (btnRight) btnRight.classList.toggle("flipped", isOffCenter);
        });
    }

    // Navigation for Right Button
    document.querySelectorAll(".slide-btn-right").forEach(btn => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            const current = btn.closest(".form-box");
            const next = current.nextElementSibling;

            if (next && next.classList.contains("form-box")) {
                next.scrollIntoView({
                    behavior: "smooth",
                    inline: "center",
                    block: "nearest"
                });
            }
        });
    });

    // Navigation for Left Button
    document.querySelectorAll(".slide-btn-left").forEach(btn => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            const current = btn.closest(".form-box");
            const prev = current.previousElementSibling;

            if (prev && prev.classList.contains("form-box")) {
                prev.scrollIntoView({
                    behavior: "smooth",
                    inline: "center",
                    block: "nearest"
                });
            }
        });
    });

    // --- Update flip state on scroll/resize ---
    slider.addEventListener("scroll", updateButtonFlip);
    window.addEventListener("resize", updateButtonFlip);
    updateButtonFlip();
});   
