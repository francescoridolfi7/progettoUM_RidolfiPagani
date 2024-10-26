const app = Vue.createApp({
    data() {
        return {
            articolo: {},
            movimenti: [],
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
                this.articolo = data;
                this.movimenti = data.movimenti?.$values || [];

                // Formatta dataMovimento 
                this.movimenti.forEach(movimento => {
                    movimento.dataMovimento = new Date(movimento.dataMovimento).toLocaleString("it-IT", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit"
                    });
                });
            } catch (error) {
                this.errore = error.message;
                console.error('Errore durante il caricamento dei dettagli dell\'articolo:', error);
            }
        }
    }
});

app.mount('#dettagliArticolo');