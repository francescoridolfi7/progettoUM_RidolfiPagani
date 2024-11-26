const app = Vue.createApp({
    data() {
        return {
            codice: '',
            posizione: '',
            totaleArticoli: 0,
            articoliDifettosi: 0,
            articoloPiuVecchio: null,
            articoloTrovatoPerCodice: null,
            articoloTrovatoPerPosizione: null,
            isSearchByCodiceActive: false, 
            isSearchByPosizioneActive: false 
        };
    },
    mounted() {
        this.getTotaleArticoli();
        this.getArticoliDifettosi();
    },
    methods: {
        searchByCodice() {
            this.isSearchByCodiceActive = true; // Attiva la ricerca
            this.isSearchByPosizioneActive = false; //Disabilita la ricerca per posizione
            console.log("Valore di codice:", this.codice);
            if (!this.codice) {
                this.articoloTrovatoPerCodice = null; // Nessuna ricerca valida
                return;
            }
            fetch(`/Articoli/GetByCodice?codice=${this.codice}`)
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 404) {
                            this.articoloTrovatoPerCodice = null;
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
                        this.articoloTrovatoPerCodice = null;
                    }
                })
                .catch(error => {
                    console.error('Errore:', error);
                    this.articoloTrovatoPerCodice = null;
                });
        },
        searchByPosizione() {
            this.isSearchByPosizioneActive = true; // Attiva la ricerca
            this.isSearchByCodiceActive = false; // Disabilita la ricerca per codice
            console.log("Valore di posizione:", this.posizione);
            if (!this.posizione) {
                this.articoloTrovatoPerPosizione = null; // Nessuna ricerca valida
                return;
            }
            fetch(`/Articoli/GetByPosizione?posizione=${this.posizione}`)
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 404) {
                            this.articoloTrovatoPerPosizione = null;
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
                        this.articoloTrovatoPerPosizione = null;
                    }
                })
                .catch(error => {
                    console.error('Errore:', error);
                    this.articoloTrovatoPerPosizione = null;
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
        },
        riparaArticolo(id) {
            fetch(`/Articoli/Ripara/${id}`, {
                method: 'POST'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore durante la riparazione dell\'articolo');
                    }
                    console.log('Articolo riparato con successo!');

                    // Aggiorna lo stato dell'articolo trovato per codice
                    if (this.articoloTrovatoPerCodice && this.articoloTrovatoPerCodice.id === id) {
                        this.articoloTrovatoPerCodice.stato = 'In Magazzino'; // Cambia lo stato
                    }

                    // Aggiorna la lista degli articoli tramite una nuova ricerca
                    this.searchByCodice();
                    this.searchByPosizione();
                    this.searchArticoloPiuVecchio();
                })
                .catch(error => {
                    console.error('Errore:', error);
                });
        }
    }
});

app.mount('#ricerca-articolo');
