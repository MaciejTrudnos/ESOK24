<template>
    <BreadcrumbComponent pageName='Konto' />

    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-6">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Zmień hasło</h3>
                        </div>
                        <div>
                            <div class="card-body">
                                <div class="form-group">
                                    <label>Obecne hasło</label>
                                    <input type="password" v-bind:class="['form-control', { 'is-invalid': currentPasswordHasError }]" v-model="currentPassword" placeholder="Obecne hasło">
                                    <span class="text-red text-sm" v-if="currentPasswordHasError">
                                        Obecne hasło jest wymagane
                                    </span>
                                </div>

                                <div class="form-group">
                                    <label>Nowe hasło</label>
                                    <input type="password" v-bind:class="['form-control', { 'is-invalid': newPasswordHasError }]" v-model="newPassword" placeholder="Nowe hasło">
                                    <span class="text-red text-sm" v-if="newPasswordHasError">
                                        Hasło powinno zawierać co najmniej 6 znaków, w tym cyfry
                                    </span>
                                </div>
                            </div>

                            <div class="card-footer">
                                <SubmitButtonComponent text='Zapisz' :className="'btn btn-primary'" :pending=pending @click="changePassword()" />
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

</template>

<script>
    import BreadcrumbComponent from '@/components/BreadcrumbComponent.vue';
    import SubmitButtonComponent from '@/components/SubmitButtonComponent.vue';
    import AuthService from '@/common/authService';

    export default {
        data() {
            return {
                currentPassword: "",
                newPassword: "",
                currentPasswordHasError: false,
                newPasswordHasError: false,
                pending: false
            }
        },
        components: {
            BreadcrumbComponent,
            SubmitButtonComponent
        },
        methods: {
            async changePassword() {
                let hasError = false;

                if (this.currentPassword === '') {
                    this.currentPasswordHasError = true;
                    hasError = true;
                }

                if (!AuthService.validatePassword(this.newPassword)) {
                    this.newPasswordHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                await AuthService.changePassword(this);
            }
        }
    }
</script>