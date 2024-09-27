const app = Vue.createApp({
    data() {
        return {
            posizioniLibere: [],
            selectedPosizione: null // Variabile per la posizione selezionata
        };
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
    }
});

app.mount('#nuovo-articolo');
