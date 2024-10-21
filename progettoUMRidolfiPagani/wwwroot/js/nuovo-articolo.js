const app = Vue.createApp({
    data() {
        return {
            codice: '',
            descrizione: '',
            quantita: 0,
            stato: '',
            selectedPosizione: null, //Variabile per tracciare la posizione selezionata
            posizioniLibere: [],
            showPosizione: true //Variabile per controllare la visibilità del campo Posizione
        };
    },
    watch: {
        stato(newVal) {
            if (newVal === 'Reparto') {
                this.showPosizione = false;
                this.selectedPosizione = null;
            } else {
                this.showPosizione = true;
            }
        }
    },
    mounted() {
        this.getPosizioniLibere();
    },
    methods: {
        getPosizioniLibere() {
            fetch('/Articoli/GetPosizioniLibere')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta delle posizioni: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.posizioniLibere = data.$values.map(posizione => ({
                        id: posizione.id,
                        codicePosizione: posizione.codicePosizione
                    }));
                })
                .catch(error => console.error('Errore:', error));
        },
        createArticolo() {
            console.log("Codice:", this.codice);
            console.log("Descrizione:", this.descrizione);
            console.log("Stato:", this.stato);
            console.log("Posizione ID:", this.selectedPosizione);
            if (!this.codice || !this.descrizione || this.quantita <= 0 || !this.stato) {
                console.error('Errore: tutti i campi devono essere compilati correttamente.');
                return;
            }

            const articoloData = {
                codice: this.codice,
                descrizione: this.descrizione,
                quantita: this.quantita,
                stato: this.stato,
                posizioneId: this.stato === 'Reparto' ? null : this.selectedPosizione //PosizioneId è null se lo stato è "Reparto"
            };

            fetch('/Articoli/Create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(articoloData)
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/Articoli';
                    } else {
                        console.error('Errore durante la creazione dell\'articolo:', response.status);
                    }
                })
                .catch(error => console.error('Errore:', error));
        }
    }
});

app.mount('#nuovo-articolo');
