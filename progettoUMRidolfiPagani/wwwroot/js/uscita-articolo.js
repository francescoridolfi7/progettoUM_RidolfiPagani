const app = Vue.createApp({
    data() {
        return {
            codice: '',  // Codice dell'articolo
            descrizione: '',  // Descrizione dell'articolo
            quantita: 0,  // Quantità dell'articolo
            quantitaOriginale: 0,  // Quantità originale
            stato: '',  // Stato dell'articolo
            codicePosizioneCorrente: '',  // Codice della posizione corrente
            posizioneIdCorrente: null  // ID della posizione corrente
        };
    },
    mounted() {
        this.getArticolo();
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
                    this.quantitaOriginale = data.quantita;  // Memorizza la quantità originale
                    this.stato = data.stato;
                    this.codicePosizioneCorrente = data.codicePosizioneCorrente;
                    this.posizioneIdCorrente = data.posizioneId;
                })
                .catch(error => console.error('Errore nel recupero dell\'articolo:', error));
        },
        // Funzione per confermare l'uscita
        confermaUscita() {
            const articoloId = window.location.pathname.split('/').pop();
            fetch(`/Articoli/Delete/${articoloId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    quantita: this.quantita,  // Passa la quantità selezionata
                    posizioneIdCorrente: this.posizioneIdCorrente
                })
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/Articoli';  // Reindirizza alla lista degli articoli
                    } else {
                        console.error('Errore durante l\'uscita dell\'articolo:', response.status);
                    }
                })
                .catch(error => console.error('Errore durante l\'uscita dell\'articolo:', error));
        }
    }
});

app.mount('#uscita-articolo');
