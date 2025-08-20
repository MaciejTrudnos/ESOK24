<template>
    <div class="hold-transition register-page">
        <div class="register-box">
            <LoginLogoComponent />
            <div class="alert alert-success text-center">
                <strong> Twoje konto zostało utworzone!</strong>
            </div>
            <div class="card">
                <div class="card-body register-card-body text-center">
                    <p class="box">Wysłaliśmy do Ciebie e-mail z linkiem aktywacyjnym.</p>
                    <p class="box">Sprawdź swoją skrzynkę i aktywuj konto.</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import LoginLogoComponent from '@/components/LoginLogoComponent.vue';

export default {
    components: {
        LoginLogoComponent
    }
}
</script>