function statistics() {
    const statisticsURL = "https://localhost:7071/api/statistics";
    let statButton = document.querySelector("#statistics-button");
    let statisticDivInputs = document.querySelector("#statistics");
    let housesCount = document.querySelector("#total-houses");
    let rentedCount = document.querySelector("#total-rents");
    statButton.addEventListener("click", getHouseInfo)

    async function getHouseInfo(e) {
        e.preventDefault();
        e.stopPropagation();

        const houseInfo = await ((await fetch(statisticsURL)).json());

        let totalHouses = houseInfo.totalHouses
        let totalRents = houseInfo.totalRents
        housesCount.textContent = `${totalHouses} Houses`;
        rentedCount.textContent = `${totalRents} Rents`;

        const isHidden = statisticDivInputs.classList.contains("d-none");

        statisticDivInputs.classList.toggle("d-none", !isHidden);
        statButton.textContent = isHidden ? "Show Statistics" : "Hide Statistics";
        statButton.classList.toggle("btn-primary", isHidden);
        statButton.classList.toggle("btn-danger", !isHidden);
    }
}