
const app = Vue.createApp({
    data() {
        return {
            numeroTotaleArticoli: 0,
            articoliInMagazzino: 0,
            numeroTotaleMovimenti: 0,
            numeroPosizioniDisponibili: 0,
            mediaGiorniPermanenza: 0,
            articoliDifettosi: 0,
            movimentiPerTipo: [],
            conteggiPerTipoMovimento: [],
        };
    },
    mounted() {
        this.caricaDatiDashboard();
    },
    methods: {
        caricaDatiDashboard() {
            fetch('/Dashboard/GetDashboardData')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Errore nella richiesta: ' + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    this.numeroTotaleArticoli = data.numeroTotaleArticoli;
                    this.articoliInMagazzino = data.articoliInMagazzino;
                    this.numeroTotaleMovimenti = data.numeroTotaleMovimenti;
                    this.numeroPosizioniDisponibili = data.numeroPosizioniDisponibili;
                    this.mediaGiorniPermanenza = data.mediaGiorniPermanenza;
                    this.articoliDifettosi = data.articoliDifettosi;
                    this.movimentiPerTipo = Object.entries(data.movimentiPerTipo || {}).filter(([tipo]) => tipo !== '$id');
                    console.log(this.movimentiPerTipo);
                    this.conteggiPerTipoMovimento = data.conteggiPerTipoMovimento ? data.conteggiPerTipoMovimento.$values : [];
                    this.inizializzaGraficoMediaGiorniPermanenza();
                })
                .catch(error => console.error('Errore durante il caricamento dei dati della dashboard:', error));
        },
        inizializzaGraficoMediaGiorniPermanenza() {
            const ctx = document.getElementById('graficoMediaGiorniPermanenza').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Media Giorni Permanenza'],
                    datasets: [{
                        label: 'Giorni',
                        data: [this.mediaGiorniPermanenza],
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    }
});

app.mount('#dashboard');
