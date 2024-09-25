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
        this.getAllArticoli();  
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
                    this.articoliInEsaurimento = data.$values.map(articolo => ({
                        ...articolo,
                        codicePosizione: articolo.posizione.codicePosizione
                    }));
                })
                .catch(error => console.error('Errore:', error));
        },
        getAllArticoli() {
            console.log('Fetching all articles');
            fetch('/Articoli/GetAllArticoli')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('Received articles', data);
                    // Accesso ai dati sotto la proprietà $values
                    this.articoli = data.$values.map(articolo => ({
                        ...articolo,
                        codicePosizione: articolo.posizione.codicePosizione 
                    }));
                })
                .catch(error => console.error('Errore:', error));
        }



    }
});

app.mount('#articoli');
