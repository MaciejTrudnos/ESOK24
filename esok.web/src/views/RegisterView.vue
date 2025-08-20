<template>
    <div class="hold-transition register-page">
        <div class="register-box">
            <LoginLogoComponent />

            <div class="card">
                <div class="card-body register-card-body">
                    <p class="login-box-msg">Rejestracja</p>
                    <div class="input-group">
                        <input type="text" v-bind:class="['form-control', { 'is-invalid': nameSurnameHasError }]" name="nameSurname" v-model="nameSurname" placeholder="Imię i nazwisko">

                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-user"></span>
                            </div>
                        </div>
                    </div>
                    <span class="text-red text-sm" v-if="nameSurnameHasError">
                        Imię i nazwisko jest wymagane
                    </span>

                    <div class="input-group mt-3">
                        <input type="email" v-bind:class="['form-control', { 'is-invalid': emailHasError }]" name="email" v-model="email" placeholder="E-mail">
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
                        Hasło powinno zawierać co najmniej 6 znaków, w tym cyfry
                    </span>

                    <div class="input-group mt-3">
                        <input type="text" class="form-control" name="phoneNumber" v-model="phoneNumber" v-maska data-maska="###-###-###" placeholder="Opcjonalnie numer telefonu">
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-phone-alt"></span>
                            </div>
                        </div>
                    </div>
                    <div class="input-group mt-3 text-sm">
                        <div class="icheck-primary">
                            <input type="checkbox" id="agreeTerms" name="terms" value="agree" v-model="agreeTerms">
                            <label for="agreeTerms" :class="{ 'text-red': agreeTermsHasError }">
                                Akceptuję
                                <a href="https://esok24.pl/assets/Regulamin.pdf" target="_blank">regulamin</a>
                                oraz
                                <a href="https://esok24.pl/assets/Polityka-prywatnosci.pdf" target="_blank">politykę prywatności</a>
                            </label>
                        </div>
                    </div>
                    <span class="text-red text-sm" v-if="agreeTermsHasError">
                        Musisz zaakceptować regulamin oraz politykę prywatności
                    </span>

                    <div class="row mt-3">
                        <SubmitButtonComponent text='Utwórz konto' :pending=pending :className="'btn btn-primary btn-block'" @click="signUp()" />
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
    import EmailValidator from 'email-validator';
    import { vMaska } from "maska"

    export default {
        directives: {
            maska: vMaska
        },
        data() {
            return {
                nameSurname: "",
                email: "",
                password: "",
                phoneNumber: "",
                agreeTerms: false,
                userNameHasError: false,
                emailHasError: false,
                passwordHasError: false,
                agreeTermsHasError: false,
                emailErrorText: 'E-mail jest wymagany',
                pending: false,
                registerSuccess: false
            }
        },
        components: {
            LoginLogoComponent,
            SubmitButtonComponent
        },
        methods: {
            async signUp() {
                let hasError = false;

                this.nameSurnameHasError = false;
                this.emailHasError = false;
                this.passwordHasError = false;
                this.agreeTermsHasError = false;

                if (this.nameSurname === '') {
                    this.nameSurnameHasError = true;
                    hasError = true;
                }

                if (this.email === '') {
                    this.emailErrorText = 'E-mail jest wymagany';
                    this.emailHasError = true;
                    hasError = true;
                } else if (!EmailValidator.validate(this.email)) {
                    this.emailErrorText = 'E-mail jest nieprawidłowy';
                    this.emailHasError = true;
                    hasError = true;
                }

                if (!AuthService.validatePassword(this.password)) {
                    this.passwordHasError = true;
                    hasError = true;
                }

                if (!this.agreeTerms) {
                    this.agreeTermsHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                await AuthService.signUp(this);
            }
        },
        watch: {
            userName(val) {
                if (val.length > 0) {
                    this.nameSurnameHasError = false;
                }
                else {
                    this.nameSurnameHasError = true
                }
            },
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
            },
            agreeTerms(val) {
                if (val) {
                    this.agreeTermsHasError = false;
                }
                else {
                    this.agreeTermsHasError = true
                }
            }
        }
    }
</script>

