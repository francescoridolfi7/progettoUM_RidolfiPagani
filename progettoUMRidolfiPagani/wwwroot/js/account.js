const app = Vue.createApp({
    data() {
        return {
            nome: '',
            cognome: '',
            email: '',
            username: '',
            password: '',
            registrazioneSuccesso: false,
            errorMessages: []
        };
    },
    methods: {
        registrati() {
            fetch('/Account/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    Nome: this.nome,
                    Cognome: this.cognome,
                    Email: this.email,
                    Username: this.username,
                    Password: this.password
                })
            })
                .then(response => {
                    if (response.redirected) {
                        // Se il server ha inviato un redirect, ricarica la pagina di destinazione
                        window.location.href = response.url;
                    } else if (!response.ok) {
                        return response.json().then(data => {
                            this.errorMessages = data.errors || ['Errore sconosciuto durante la registrazione.'];
                        });
                    } else {
                        this.registrazioneSuccesso = true;
                    }
                })
                .catch(error => {
                    console.error('Errore:', error);
                    this.errorMessages = ['Errore di connessione al server.'];
                });
        },
        /*login() {
            fetch('/Account/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    Username: this.username,
                    Password: this.password
                })
            })
                .then(response => {
                    if (!response.ok) {
                        return response.json().then(data => {
                            this.errorMessages = data.errors || ['Username o password non validi.'];
                        });
                    }
                    return response.json().then(data => {
                        window.location.href = data.redirectUrl; // Reindirizza alla home o articoli
                    });
                })
                .catch(error => {
                    console.error('Errore:', error);
                });
        }*/

        login() {
            fetch('/Account/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    Username: this.username,
                    Password: this.password
                })
            })
                .then(response => {
                    if (response.ok) {
                        return response.json();
                    } else {
                        return response.json().then(data => {
                            throw new Error(data.errors ? data.errors.join(', ') : 'Errore durante il login.');
                        });
                    }
                })
                .then(data => {
                    window.location.href = data.redirectUrl;
                })
                .catch(error => {
                    this.errorMessages = [error.message];
                    console.error('Errore:', error);
                });
        }
    
    }
});

app.mount('#registrazione');
