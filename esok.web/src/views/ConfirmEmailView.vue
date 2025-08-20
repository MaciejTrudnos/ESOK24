<template>
    <div class="hold-transition register-page">
        <div class="register-box">
            <LoginLogoComponent />
             <div v-if="pending">
                 <div class="alert alert-default-info text-center">
                     <strong>W trakcie aktywacji</strong><br>
                     <span class="spinner-grow spinner-grow-sm"></span>
                 </div>
            </div>
            <div v-else-if="confirmSuccess">
                <div class="alert alert-success text-center">
                    <strong> Twoje konto zostało aktywowane!</strong>
                </div>
                <div class="card">
                    <div class="card-body register-card-body" align="center">
                        <router-link to="/login">
                            <p>
                                Zaloguj się
                            </p>
                        </router-link>
                    </div>
                </div>
            </div>
            <div v-else>
                <div class="alert alert-danger text-center">
                    <strong> Wystąpił błąd podczas próby aktywowania konta</strong>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import LoginLogoComponent from '@/components/LoginLogoComponent.vue';
    import AuthService from '@/common/authService';

    export default {
        data() {
            return {
                confirmSuccess: false,
                pending: false,
                token: "",
                email: ""
            }
        },
        async mounted() {
            this.token = this.$route.query.token;
            this.email = this.$route.query.email;

            await AuthService.confirmEmail(this);
        },
        components: {
            LoginLogoComponent
        }
    }
</script>