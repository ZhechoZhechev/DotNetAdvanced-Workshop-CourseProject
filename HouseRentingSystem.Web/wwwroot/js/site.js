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

        if (statisticDivInputs.classList.contains("d-none")) {

            statisticDivInputs.classList.remove("d-none");
            housesCount.textContent = `${totalHouses} Houses`;
            rentedCount.textContent = `${totalRents} Rents`;
            statButton.textContent = "Hide Statistics"
            statButton.classList.remove("btn-primary")
            statButton.classList.add("btn-danger")
        }
        else {
            statisticDivInputs.classList.add("d-none");
            statButton.textContent = "Show Statistics"
            statButton.classList.remove("btn-danger")
            statButton.classList.add("btn-primary")
        }
    }
}