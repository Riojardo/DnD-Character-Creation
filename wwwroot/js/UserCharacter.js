
document.addEventListener("DOMContentLoaded", () => {

    const the_Option_Select = document.querySelector(".character-options-select");
    const the_CharacterList = document.querySelector(".character_list");
    const checkboxes = document.querySelectorAll('.character_list input[type="checkbox"]');
    const selectButton = document.querySelector(".select-all");
    const deleteButton = document.querySelector(".delete-all");


    selectButton.addEventListener("click", () => {

        if (selectButton.innerHTML == "Select All") {
            checkboxes.forEach(cb => {
                cb.checked = true;
            })
            selectButton.innerHTML = "Select None";
            return;
        }
        checkboxes.forEach(cb => {
            cb.checked = false;
        })
        selectButton.innerHTML = "Select All";
    });

    deleteButton.addEventListener("click", async () => {

        const selectedCheckboxes = document.querySelectorAll('.character_list input[type="checkbox"]:checked');

        if (selectedCheckboxes.length === 0) {
            alert("Please select at least one character to delete.");
            return;
        }

        if (confirm("Are you sure? Characters are deleted permanently")) {

            for (const cb of selectedCheckboxes) {
                const listItem = cb.closest('.character-item');
                const id = listItem.dataset.id;

                try {
                    let response = await fetch(`/Character/Erase/${id}`, { method: 'DELETE' });

                    if (response.ok) {
                        listItem.remove();
                    } else {
                        alert(`Could not delete character ${id}`);
                    }
                } catch (error) {
                    alert("Network error:", error);
                }
            }
        }
    });

    function changeOrder() {
        const items = Array.from(the_CharacterList.querySelectorAll(".character-item"));
        const sortBy = the_Option_Select.value;

        items.sort((a, b) => {

            const nameA = (a.dataset.name || "").toLowerCase();
            const nameB = (b.dataset.name || "").toLowerCase();

            const levelA = parseInt(a.dataset.level) || 0;
            const levelB = parseInt(b.dataset.level) || 0;

            const dateA = a.dataset.date ? new Date(a.dataset.date) : new Date(0);
            const dateB = b.dataset.date ? new Date(b.dataset.date) : new Date(0);

            switch (sortBy) {
                case "Alphabet_Down": return nameA.localeCompare(nameB);
                case "Alphabet_Up": return nameB.localeCompare(nameA);
                case "Level_Down": return levelB - levelA;
                case "Level_Up": return levelA - levelB;
                case "Date_Down": return dateB - dateA;
                case "Date_Up": return dateA - dateB;
                default: return 0;
            }
        });

        the_CharacterList.innerHTML = "";
        items.forEach(item => the_CharacterList.appendChild(item));
    }

    the_Option_Select.addEventListener("change", changeOrder);

    changeOrder();
});