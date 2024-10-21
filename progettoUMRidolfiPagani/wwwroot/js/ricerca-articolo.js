const app = Vue.createApp({
    data() {
        return {
            codice: '',
            posizione: '',
            totaleArticoli: 0,
            articoliDifettosi: 0,
            articoloPiuVecchio: null,
            articoloTrovatoPerCodice: null,
            articoloTrovatoPerPosizione: null
        };
    },
    mounted() {
        this.getTotaleArticoli();
        this.getArticoliDifettosi();
    },
    methods: {
        searchByCodice() {
            console.log("Valore di codice:", this.codice);
            if (!this.codice) {
                console.error('Errore: il campo codice è vuoto');
                this.articoloTrovatoPerCodice = 'Nessun articolo trovato'; 
                return;
            }
            fetch(`/Articoli/GetByCodice?codice=${this.codice}`)
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 404) {
                            console.warn('Nessun articolo trovato per il codice inserito.');
                            this.articoloTrovatoPerCodice = 'Nessun articolo trovato'; 
                            return null;
                        } else {
                            throw new Error('Errore nella richiesta: ' + response.status);
                        }
                    }
                    return response.json();
                })
                .then(data => {
                    if (data) {
                        this.articoloTrovatoPerCodice = {
                            ...data,
                            codicePosizione: data.posizione ? data.posizione.codicePosizione : 'Posizione non disponibile'
                        };
                    } else {
                        this.articoloTrovatoPerCodice = 'Nessun articolo trovato';  
                    }
                    console.log(this.articoloTrovatoPerCodice);
                })
                .catch(error => {
                    console.error('Errore:', error);
                    this.articoloTrovatoPerCodice = 'Errore durante la ricerca';  
                });
        },
        searchByPosizione() {
            console.log("Valore di posizione:", this.posizione);
            if (!this.posizione) {
                console.error('Errore: il campo posizione è vuoto');
                this.articoloTrovatoPerPosizione = 'Nessun articolo trovato'; 
                return;
            }
            fetch(`/Articoli/GetByPosizione?posizione=${this.posizione}`)
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 404) {
                            console.warn('Nessun articolo trovato per la posizione inserita.');
                            this.articoloTrovatoPerPosizione = 'Nessun articolo trovato';  
                            return null;
                        } else {
                            throw new Error('Errore nella richiesta: ' + response.status);
                        }
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.$values.length > 0) {
                        const articolo = data.$values[0];
                        this.articoloTrovatoPerPosizione = {
                            ...articolo,
                            codicePosizione: articolo.posizione ? articolo.posizione.codicePosizione : 'Posizione non disponibile'
                        };
                    } else {
                        this.articoloTrovatoPerPosizione = 'Nessun articolo trovato';  
                    }
                    console.log(this.articoloTrovatoPerPosizione);
                })
                .catch(error => {
                    console.error('Errore:', error);
                    this.articoloTrovatoPerPosizione = 'Errore durante la ricerca';  
                });
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
        searchArticoloPiuVecchio() {
            fetch('/Articoli/GetArticoloPiuVecchio')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    if (data) {
                        this.articoloPiuVecchio = data;  
                    } else {
                        this.articoloPiuVecchio = null;  
                    }
                })
                .catch(error => console.error('Errore:', error));
        }
    }
});

app.mount('#ricerca-articolo');
