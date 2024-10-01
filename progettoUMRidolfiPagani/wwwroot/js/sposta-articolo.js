const app = Vue.createApp({
    data() {
        return {
            codice: '',  // Codice dell'articolo
            descrizione: '',  // Descrizione dell'articolo
            quantita: 0,  // Quantità dell'articolo
            stato: '',  // Stato dell'articolo
            codicePosizioneCorrente: '',  // Codice della posizione corrente
            selectedPosizione: null,  // Posizione selezionata per lo spostamento
            posizioniLibere: []  // Lista delle posizioni libere
        };
    },
    mounted() {
        this.getArticolo();
        this.getPosizioniLibere();
    },
    methods: {
        // Recupera i dettagli dell'articolo corrente
        getArticolo() {
            const articoloId = window.location.pathname.split('/').pop();
            fetch(`/Articoli/GetArticoloById/${articoloId}`)
                .then(response => response.json())
                .then(data => {
                    this.codice = data.codice;
                    this.descrizione = data.descrizione;
                    this.quantita = data.quantita;
                    this.quantitaOriginale = data.quantita;  // Salva la quantità originale
                    this.stato = data.stato;
                    this.codicePosizioneCorrente = data.codicePosizioneCorrente;
                    this.posizioneIdCorrente = data.posizioneId;  // Salva la posizione originale
                    this.selectedPosizione = data.posizioneId;  // Imposta la posizione corrente come selezionata
                })
                .catch(error => console.error('Errore nel recupero dell\'articolo:', error));
        },
        // Recupera la lista delle posizioni libere
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
        // Aggiorna l'articolo e sposta la sua posizione
        updateArticolo() {
            const articoloId = window.location.pathname.split('/').pop(); // Estrai l'ID dalla URL corrente

            // Assicurati che la quantità sia un numero valido
            if (this.quantita <= 0) {
                console.error('Quantità non valida.');
                return;
            }

            if (!this.selectedPosizione) {
                console.error('Posizione non selezionata.');
                return;
            }

            console.log("Quantità:", this.quantita);  // Debug quantità
            console.log("Posizione selezionata:", this.selectedPosizione);  // Debug posizione

            // Passiamo i valori originali separatamente
            const articoloData = {
                id: articoloId,  // ID dell'articolo
                quantita: this.quantita,  // La nuova quantità selezionata
                posizioneId: this.selectedPosizione,  // La nuova posizione selezionata
                quantitaOriginale: this.quantitaOriginale,  // Quantità originale prima della modifica
                posizioneIdCorrente: this.posizioneIdCorrente  // Posizione originale
            };

            fetch(`/Articoli/Edit/${articoloId}`, {  // Aggiungi l'ID dell'articolo nell'URL
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(articoloData)
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/Articoli'; // Reindirizza alla lista degli articoli dopo il successo
                    } else {
                        console.error('Errore durante l\'aggiornamento dell\'articolo:', response.status);
                    }
                })
                .catch(error => console.error('Errore durante l\'aggiornamento dell\'articolo:', error));
        }

    }
});

app.mount('#sposta-articolo');
