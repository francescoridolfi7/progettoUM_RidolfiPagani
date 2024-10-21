const app = Vue.createApp({
    data() {
        return {
            codice: '',  
            descrizione: '',  
            quantita: 0,  
            quantitaOriginale: 0,  
            stato: '',  
            codicePosizioneCorrente: '', 
            posizioneIdCorrente: null  
        };
    },
    mounted() {
        this.getArticolo();
    },
    methods: {
        //Recupera i dettagli dell'articolo corrente
        getArticolo() {
            const articoloId = window.location.pathname.split('/').pop();
            fetch(`/Articoli/GetArticoloById/${articoloId}`)
                .then(response => response.json())
                .then(data => {
                    this.codice = data.codice;
                    this.descrizione = data.descrizione;
                    this.quantita = data.quantita;
                    this.quantitaOriginale = data.quantita;  //Memorizza la quantità originale
                    this.stato = data.stato;
                    this.codicePosizioneCorrente = data.codicePosizioneCorrente;
                    this.posizioneIdCorrente = data.posizioneId;
                })
                .catch(error => console.error('Errore nel recupero dell\'articolo:', error));
        },
        //Funzione per confermare l'uscita
        confermaUscita() {
            const articoloId = window.location.pathname.split('/').pop();
            fetch(`/Articoli/Delete/${articoloId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    quantita: this.quantita,  //Passiamo la quantità selezionata
                    posizioneIdCorrente: this.posizioneIdCorrente
                })
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/Articoli';  //Reindirizza alla lista degli articoli
                    } else {
                        console.error('Errore durante l\'uscita dell\'articolo:', response.status);
                    }
                })
                .catch(error => console.error('Errore durante l\'uscita dell\'articolo:', error));
        }
    }
});

app.mount('#uscita-articolo');
