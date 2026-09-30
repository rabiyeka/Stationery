(() => {
    const picker = document.querySelector("[data-location-picker]");
    if (!picker) return;

    const selectsContainer = picker.querySelector("[data-location-selects]");
    const manualContainer = picker.querySelector("[data-location-manual]");
    const provinceSelect = picker.querySelector("[data-location-province]");
    const districtSelect = picker.querySelector("[data-location-district]");
    const neighborhoodSelect = picker.querySelector("[data-location-neighborhood]");
    const manualInputs = Array.from(manualContainer.querySelectorAll("input"));
    const modeToggle = picker.querySelector("[data-location-toggle]");
    const status = picker.querySelector("[data-location-status]");

    const setStatus = message => {
        status.textContent = message;
    };

    const setMode = manual => {
        selectsContainer.classList.toggle("d-none", manual);
        manualContainer.classList.toggle("d-none", !manual);
        provinceSelect.disabled = manual;
        districtSelect.disabled = manual || !provinceSelect.value;
        neighborhoodSelect.disabled = manual || !districtSelect.value;
        provinceSelect.required = !manual;
        districtSelect.required = !manual;
        neighborhoodSelect.required = !manual;
        manualInputs.forEach(input => {
            input.disabled = !manual;
        });
        modeToggle.setAttribute("aria-expanded", String(manual));
        modeToggle.textContent = manual ? "Listeden seç" : "Konum bilgilerini elle gir";
    };

    const fetchLocations = async (url, select, placeholder, selectedName = "") => {
        const response = await fetch(url, {
            headers: { Accept: "application/json" },
            credentials: "same-origin"
        });
        const locations = await response.json();
        if (!response.ok) {
            throw new Error(locations.message || "Konum listesi alınamadı.");
        }

        select.replaceChildren(new Option(placeholder, ""));
        for (const location of locations) {
            const option = new Option(location.name, location.name);
            option.dataset.id = location.id;
            select.add(option);
        }
        select.disabled = false;

        if (selectedName) {
            select.value = selectedName;
            if (select.value !== selectedName) {
                throw new Error(`“${selectedName}” konum listesinde bulunamadı.`);
            }
        }
    };

    const getSelectedId = select => select.selectedOptions[0]?.dataset.id;

    const loadDistricts = async selectedName => {
        districtSelect.disabled = true;
        neighborhoodSelect.disabled = true;
        districtSelect.replaceChildren(new Option("İlçe seçin", ""));
        neighborhoodSelect.replaceChildren(new Option("Önce ilçe seçin", ""));
        const provinceId = getSelectedId(provinceSelect);
        if (!provinceId) return;

        setStatus("İlçeler yükleniyor...");
        const url = picker.dataset.districtsUrl.replace("__id__", provinceId);
        await fetchLocations(url, districtSelect, "İlçe seçin", selectedName);
    };

    const loadNeighborhoods = async selectedName => {
        neighborhoodSelect.disabled = true;
        neighborhoodSelect.replaceChildren(new Option("Mahalle seçin", ""));
        const districtId = getSelectedId(districtSelect);
        if (!districtId) return;

        setStatus("Mahalleler yükleniyor...");
        const url = picker.dataset.neighborhoodsUrl.replace("__id__", districtId);
        await fetchLocations(url, neighborhoodSelect, "Mahalle seçin", selectedName);
    };

    provinceSelect.addEventListener("change", async () => {
        try {
            await loadDistricts();
            setStatus(provinceSelect.value ? "İlçe seçin." : "İl seçerek başlayın.");
        } catch (error) {
            setStatus(`${error.message} Konumları elle girebilirsiniz.`);
            setMode(true);
        }
    });

    districtSelect.addEventListener("change", async () => {
        try {
            await loadNeighborhoods();
            setStatus(districtSelect.value ? "Mahalle seçin." : "");
        } catch (error) {
            setStatus(`${error.message} Konumları elle girebilirsiniz.`);
            setMode(true);
        }
    });

    modeToggle.addEventListener("click", () => {
        const useManualEntry = modeToggle.getAttribute("aria-expanded") !== "true";
        setMode(useManualEntry);
        setStatus(useManualEntry ? "İl, ilçe ve mahalleyi elle girebilirsiniz." : "İller yükleniyor...");
    });

    const initialize = async () => {
        const selectedCity = picker.dataset.selectedCity || "";
        const selectedDistrict = picker.dataset.selectedDistrict || "";
        const selectedNeighborhood = picker.dataset.selectedNeighborhood || "";

        try {
            setStatus("İller yükleniyor...");
            await fetchLocations(picker.dataset.provincesUrl, provinceSelect, "İl seçin", selectedCity);
            if (!selectedCity) {
                setStatus("İl seçerek başlayın.");
                return;
            }

            await loadDistricts(selectedDistrict);
            if (selectedDistrict && districtSelect.value !== selectedDistrict) {
                throw new Error(`“${selectedDistrict}” ilçe listesinde bulunamadı.`);
            }

            await loadNeighborhoods(selectedNeighborhood);
            if (selectedNeighborhood && neighborhoodSelect.value !== selectedNeighborhood) {
                throw new Error(`“${selectedNeighborhood}” mahalle listesinde bulunamadı.`);
            }
            setStatus("");
        } catch (error) {
            setMode(true);
            setStatus(`${error.message} Konumları elle girebilirsiniz.`);
        }
    };

    initialize();
})();