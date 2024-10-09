const app = Vue.createApp({
    data() {
        return {
            articolo: {},  // Assicurati che sia inizializzato come oggetto vuoto
            movimenti: [],  // Assicurati che sia inizializzato come array vuoto
            errore: null
        };
    },
    mounted() {
        this.caricaDettagliArticolo();
    },
    methods: {
        async caricaDettagliArticolo() {
            const articoloId = window.location.pathname.split('/').pop();

            try {
                const response = await fetch(`/Articoli/GetArticoloById/${articoloId}`);
                if (!response.ok) {
                    throw new Error(`Errore durante il caricamento dell'articolo: ${response.statusText}`);
                }
                const data = await response.json();
                this.articolo = data;  // Assegna direttamente i dati ricevuti all'oggetto articolo
                this.movimenti = data.movimenti?.$values || [];  // Estrai i movimenti dall'oggetto $values
            } catch (error) {
                this.errore = error.message;
                console.error('Errore durante il caricamento dei dettagli dell\'articolo:', error);
            }
        }
    }
});

app.mount('#dettagliArticolo');
