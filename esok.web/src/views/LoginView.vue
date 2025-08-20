<template>
    <div class="hold-transition login-page">
        <div class="login-box">
            <LoginLogoComponent />
            <!-- /.login-logo -->
            <div class="card">
                <div class="card-body login-card-body">
                    <p class="login-box-msg">Logowanie</p>
                    <form @submit.prevent>
                        <div class="input-group">
                            <input type="text" v-bind:class="['form-control', { 'is-invalid': emailHasError }]" name="email" v-model="email" placeholder="E-mail">
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-envelope"></span>
                                </div>
                            </div>
                        </div>
                        <span class="text-red text-sm" v-if="emailHasError">
                            {{emailErrorText}}
                        </span>

                        <div class="input-group mt-3">
                            <input type="password" v-bind:class="['form-control', { 'is-invalid': passwordHasError }]" name="password" v-model="password" placeholder="Hasło">
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock"></span>
                                </div>
                            </div>
                        </div>
                        <span class="text-red text-sm" v-if="passwordHasError">
                            Hasło jest wymagane
                        </span>

                        <div class="row mt-3">
                            <SubmitButtonComponent text='Zaloguj' :className="'btn btn-primary btn-block'" :pending=pending @click="signIn()" @keyup.enter="signIn()" />
                        </div>
                    </form>

                    <div class="row mt-4 text-sm">
                        <div class="col">
                            <router-link to="/register" style="color: inherit;">Nie masz jeszcze konta?<br>Załóż teraz!</router-link>
                        </div>
                        <!-- /.col -->
                        <div class="col text-right">
                            <router-link to="/forgotpassword" style="color: inherit;">Nie pamiętasz hasła?</router-link>
                        </div>
                        <!-- /.col -->
                    </div>
                </div>
                <!-- /.login-card-body -->
            </div>
        </div>
    </div>
    <!-- /.login-box -->
</template>

<script>
    import LoginLogoComponent from '@/components/LoginLogoComponent.vue';
    import SubmitButtonComponent from '@/components/SubmitButtonComponent.vue';
    import AuthService from '@/common/authService';
    import EmailValidator from 'email-validator';

    export default {
        data() {
            return {
                email: "",
                password: "",
                pending: false,
                emailHasError: false,
                passwordHasError: false,
                emailErrorText: 'E-mail jest wymagany',
            }
        },
        components: {
            LoginLogoComponent,
            SubmitButtonComponent
        },
        methods: {
            async signIn() {
                let hasError = false;

                this.emailHasError = false;
                this.passwordHasError = false;

                if (this.email === '') {
                    this.emailErrorText = 'E-mail jest wymagany';
                    this.emailHasError = true;
                    hasError = true;
                } else if (!EmailValidator.validate(this.email)) {
                    this.emailErrorText = 'E-mail jest nieprawidłowy';
                    this.emailHasError = true;
                    hasError = true;
                }

                if (this.password === '') {
                    this.passwordHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                await AuthService.signIn(this);
            }
        },
        watch: {
            email(val) {
                if (val.length > 0) {
                    this.emailHasError = false;
                }
                else {
                    this.emailHasError = true
                }
            },
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