<template>
    <div class="hold-transition register-page">
        <div class="register-box">
            <LoginLogoComponent />
            <div class="card">
                <div class="card-body register-card-body">
                    <p class="login-box-msg">Ustaw hasło</p>

                    <div class="input-group mt-3">
                        <input type="password" v-bind:class="['form-control', { 'is-invalid': passwordHasError }]" name="password" v-model="password" placeholder="Hasło">
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-lock"></span>
                            </div>
                        </div>
                    </div>
                    <span class="text-red text-sm" v-if="passwordHasError">
                        Hasło powinno zawierać co najmniej 6 znaków, w tym cyfry
                    </span>

                    <div class="row mt-3">
                        <SubmitButtonComponent text='Zapisz' :className="'btn btn-primary btn-block'" :pending=pending @click="confirmEmployee()" />
                    </div>

                </div>
                <!-- /.form-box -->
            </div>
        </div>
    </div>
</template>

<script>
    import LoginLogoComponent from '@/components/LoginLogoComponent.vue';
    import SubmitButtonComponent from '@/components/SubmitButtonComponent.vue';
    import AuthService from '@/common/authService';

    export default {
        data() {
            return {
                pending: false,
                token: "",
                email: "",
                password: "",
                passwordHasError: false,
            }
        },
        async mounted() {
            this.email = this.$route.query.email;
            this.token = this.$route.query.token;
        },
        components: {
            LoginLogoComponent,
            SubmitButtonComponent
        },
        methods: {
            async confirmEmployee() {
                let hasError = false;

                this.passwordHasError = false;

                if (!AuthService.validatePassword(this.password)) {
                    this.passwordHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                await AuthService.confirmEmployee(this);
            }
        },
        watch: {
            password(val) {
                if (val.length > 0) {
                    this.passwordHasError = false;
                }
                else {
                    this.passwordHasError = true
                }
            }
        }
    }
</script>