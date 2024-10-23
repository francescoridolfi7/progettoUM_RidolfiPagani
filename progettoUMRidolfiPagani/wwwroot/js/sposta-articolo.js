const app = Vue.createApp({
    data() {
        return {
            codice: '',  
            descrizione: '',  
            quantita: 0,  
            stato: '',  
            codicePosizioneCorrente: '',  //Codice della posizione corrente
            selectedPosizione: null,  //Posizione selezionata per lo spostamento
            posizioniLibere: [],  //Lista delle posizioni libere
            idPosizioneCorrente: null
        };
    },
    mounted() {
        this.getArticolo();
        this.getPosizioniLibere();
    },
    methods: {
        getArticolo() {
            const articoloId = window.location.pathname.split('/').pop();
            fetch(`/Articoli/GetArticoloById/${articoloId}`)
                .then(response => response.json())
                .then(data => {
                    this.codice = data.codice;
                    this.descrizione = data.descrizione;
                    this.quantita = data.quantita;  
                    this.stato = data.stato;
                    this.codicePosizioneCorrente = data.codicePosizioneCorrente;
                    /*this.selectedPosizione = data.posizioneId;  //Imposta la posizione corrente come selezionata*/
                    this.idPosizioneCorrente = data.posizioneId;
                })
                .catch(error => console.error('Errore nel recupero dell\'articolo:', error));
        },
        getPosizioniLibere() {
            fetch('/Articoli/GetPosizioniLibere')
                .then(response => response.json())
                .then(data => {
                    this.posizioniLibere = data.$values.map(posizione => ({
                        id: posizione.id,
                        codicePosizione: posizione.codicePosizione
                    }));
                })
                .catch(error => console.error('Errore nella richiesta delle posizioni:', error));
        },
        updateArticolo() {
            const articoloId = window.location.pathname.split('/').pop();

            if (!this.selectedPosizione) {
                console.error('Posizione non selezionata.');
                return;
            }

            const articoloData = {
                id: articoloId,
                quantita: this.quantita,  
                posizioneId: this.selectedPosizione, 
                posizioneIdCorrente: this.idPosizioneCorrente
            };

            console.log(articoloData);

            fetch(`/Articoli/Edit/${articoloId}`, {
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
                        console.error('Errore durante l\'aggiornamento dell\'articolo:', response.status);
                    }
                })
                .catch(error => console.error('Errore durante l\'aggiornamento dell\'articolo:', error));
        }
    }
});

app.mount('#sposta-articolo');
