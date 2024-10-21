const app = Vue.createApp({
    data() {
        return {
            codice: '',
            posizione: '',
            articoli: [],
            movimenti: [],
            totaleArticoli: 0,
            articoliDifettosi: 0,
            articoliInEsaurimento: [],
            articoloPiuVecchio: null,
            articoloTrovatoPerCodice: null,
            articoloTrovatoPerPosizione: null  
        };
    },
    mounted() {
        this.getArticoliInEsaurimento();
        this.getAllArticoli();  
    },
    methods: {
        
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
        riparaArticolo(id) {
            fetch(`/Articoli/Ripara/${id}`, {
                method: 'POST'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore durante la riparazione dell\'articolo');
                    }

                    // Aggiorna lo stato dell'articolo nella lista degli articoli
                    const articoloRiparato = this.articoli.find(articolo => articolo.id === id);
                    if (articoloRiparato) {
                        articoloRiparato.stato = 'In Magazzino'; // Cambia lo stato in "In Magazzino"
                    }

                    // Aggiorna la lista degli articoli in esaurimento
                    this.articoliInEsaurimento = this.articoliInEsaurimento.filter(articolo => articolo.id !== id);

                    console.log('Articolo riparato con successo!');
                })
                .catch(error => {
                    console.error('Errore:', error);
                });
        }
    

    }
});

app.mount('#articoli');
