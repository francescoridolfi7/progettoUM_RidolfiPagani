const app = Vue.createApp({
    data() {
        return {
            codice: '',
            posizione: '',
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
            console.log("Valore di codice:", this.codice);
            if (!this.codice) {
                console.error('Errore: il campo codice è vuoto');
                return;
            }
            fetch(`/Articoli/GetByCodice?codice=${this.codice}`)
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 404) {
                            console.warn('Nessun articolo trovato per il codice inserito.');
                            this.articoli = []; 
                            return null;
                        } else {
                            throw new Error('Errore nella richiesta: ' + response.status);
                        }
                    }
                    return response.json();
                })
                .then(data => {
                    if (data) {  // Se esiste un articolo restituito
                        this.articoli = [{
                            ...data,
                            codicePosizione: data.posizione ? data.posizione.codicePosizione : 'Posizione non disponibile'
                        }];
                    } else {
                        this.articoli = []; // Altrimenti restituiamo una lista vuota
                    }
                    console.log(this.articoli);
                })
                .catch(error => console.error('Errore:', error));
        },
        searchByPosizione() {
            console.log("Valore di posizione:", this.posizione);
            if (!this.posizione) {
                console.error('Errore: il campo posizione è vuoto');
                return;
            }
            fetch(`/Articoli/GetByPosizione?posizione=${this.posizione}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log("Risposta dal server:", data); // Log della risposta

                    // Accedi ai dati sotto la proprietà $values
                    this.articoli = data.$values.map(articolo => ({
                        ...articolo,
                        codicePosizione: articolo.posizione ? articolo.posizione.codicePosizione : 'Posizione non disponibile'
                    }));

                    console.log(this.articoli); // Verifica gli articoli processati
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
                        codicePosizione: articolo.posizione ? articolo.posizione.codicePosizione : 'Articolo consegnato direttamente al reparto'
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
                        codicePosizione: articolo.posizione ? articolo.posizione.codicePosizione : 'Articolo consegnato direttamente al reparto'
                    }));
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
                        this.articoli = [data];  // Restituiamo un singolo articolo in un array
                    } else {
                        this.articoli = [];  // Nessun articolo trovato
                    }
                })
                .catch(error => console.error('Errore:', error));
        }




    }
});

app.mount('#articoli');
