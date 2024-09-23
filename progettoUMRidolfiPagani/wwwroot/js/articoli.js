const app = Vue.createApp({
    data() {
        return {
            codice: '',
            searchString: '',
            articoli: [],
            movimenti: [],
            totaleArticoli: 0,
            articoliDifettosi: 0,
            articoliInEsaurimento: []
        };
    },
    mounted() {
        this.getTotaleArticoli();
        this.getArticoliDifettosi();
        this.getArticoliInEsaurimento();
    },
    methods: {
        searchByCodice() {
            fetch(`/Articoli/GetByCodice?codice=${this.codice}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.articoli = [data];
                })
                .catch(error => console.error('Errore:', error));
        },
        searchArticoli() {
            fetch(`/Articoli/Search?searchString=${this.searchString}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.articoli = data;
                })
                .catch(error => console.error('Errore:', error));
        },
        getTotaleArticoli() {
            fetch('/Articoli/GetArticoliCount')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.totaleArticoli = data;
                })
                .catch(error => console.error('Errore:', error));
        },
        getArticoliDifettosi() {
            fetch('/Articoli/GetArticoliDifettosiCount')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.articoliDifettosi = data;
                })
                .catch(error => console.error('Errore:', error));
        },
        getArticoliInEsaurimento() {
            fetch('/Articoli/GetArticoliInEsaurimento')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.articoliInEsaurimento = data;
                })
                .catch(error => console.error('Errore:', error));
        },
        getMovimenti(articoloId) {
            fetch(`/Articoli/GetMovimentiByArticoloId?id=${articoloId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.movimenti = data;
                })
                .catch(error => console.error('Errore:', error));
        }
    }
});

app.mount('#articoli');
