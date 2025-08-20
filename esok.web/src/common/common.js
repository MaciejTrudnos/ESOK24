const common = {
    addZeroes(num) {
        let value = Number(num);
        let res = String(num).split(".");

        if (res.length == 1 || res[1].length < 3) {
            value = value.toFixed(2);
        }

        return value;
    },
    computeGrossPrice(netPrice) {
        let taxRate = 23;

        let finalPrice = netPrice * (1 + (taxRate / 100));
        let roundedFinalPrice = +(finalPrice.toFixed(2));

        return this.addZeroes(roundedFinalPrice);
    },
    computeNetPrice(grossPrice) {
        let taxRate = 23;

        let finalPrice = grossPrice / (1 + (taxRate / 100));
        let roundedFinalPrice = +(finalPrice.toFixed(2));

        return this.addZeroes(roundedFinalPrice);
    },
    translateUnit(enumId) {
        switch (enumId) {
            case "Minute":
                return "Minuta"
            case "Hour":
                return "Godzina"
            case "Day":
                return "Dzień"
            case "Month":
                return "Miesiąc"
            case "Year":
                return "Rok"
            default:
                return "Brak danych"
        }
    }
}

export default common