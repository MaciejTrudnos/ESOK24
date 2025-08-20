<template>
    <BreadcrumbComponent  pageName='Pracownicy' />
    
    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-6">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Dodaj pracownika</h3>
                        </div>
                        <div>
                            <div class="card-body">
                                <div class="form-group">
                                    <label>Imię i nazwisko</label>
                                    <input type="text" v-bind:class="['form-control', { 'is-invalid': nameSurnameHasError }]" name="nameSurname" v-model="nameSurname" placeholder="Imię i nazwisko">
                                    <span class="text-red text-sm" v-if="nameSurnameHasError">
                                        Imię i nazwisko jest wymagane
                                    </span>
                                </div>
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <input type="email" v-bind:class="['form-control', { 'is-invalid': emailHasError }]" name="email" v-model="email" placeholder="E-mail">
                                    <span class="text-red text-sm" v-if="emailHasError">
                                        {{emailErrorText}}
                                    </span>
                                </div>
                                <div class="form-group">
                                    <label>Numer telefonu</label>
                                    <input type="text" class="form-control" v-model="phoneNumber" v-maska data-maska="###-###-###" placeholder="Opcjonalnie numer telefonu">
                                </div>
                            </div>

                            <div class="card-footer">
                                <SubmitButtonComponent text='Dodaj' :className="'btn btn-primary'" :pending=pending @click="addEmployee()" />
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Pracownicy</h3>
                        </div>

                        <div v-if="getEmployeesPending" class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                    <label>{{employeeMessage}}</label>
                                </div>
                            </div>
                        </div>

                        <div v-else-if="employees.length > 0" class="card-body table-responsive p-0" style="height: 360px;">
                            <table class="table table-head-fixed table-hover text-nowrap">
                                <thead>
                                    <tr>
                                        <th style="width: 10px">#</th>
                                        <th>Imię i Nazwsiko</th>
                                        <th>E-mail</th>
                                        <th>Numer telefonu</th>
                                        <th>Status konta</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(employee, index) in employees" :key="employee.id">
                                        <td>{{ index + 1}}</td>
                                        <td>{{ employee.nameSurname}}</td>
                                        <td>{{ employee.email}}</td>

                                        <td v-if="employee.phoneNumber">
                                            {{ employee.phoneNumber}}
                                        </td>
                                        <td v-else>
                                            Brak danych
                                        </td>

                                        <td v-if="employee.confirmed">
                                            <span class="badge badge-success">Aktywne</span>
                                        </td>
                                        <td v-else>
                                            <span class="badge badge-warning">Nieaktywne</span>
                                        </td>
                                        <td>
                                            <DeleteButtonComponent text='Usuń' @click="deleteEmployee(employee.nameSurname, employee.id)" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div v-else class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <label>{{employeeMessage}}</label>
                                </div>
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
    import DeleteButtonComponent from '@/components/DeleteButtonComponent.vue';
    import EmailValidator from 'email-validator';
    import { httpClient } from '@/common/httpClient';
    import Swal from '@/common/sweetalert2';
    import { vMaska } from "maska"

    export default {
        directives: {
            maska: vMaska
        },
        data() {
            return {
                nameSurname: "",
                email: "",
                phoneNumber: "",
                nameSurnameHasError: false,
                emailHasError: false,
                emailErrorText: 'E-mail jest wymagany',
                pending: false,
                getEmployeesPending: false,
                employeeMessage: 'Brak pracowników',
                employees: [],
                resetForm: false
            }
        },
        components: {
            BreadcrumbComponent,
            SubmitButtonComponent,
            DeleteButtonComponent
        },
        methods: {
            async addEmployee() {
                let hasError = false;

                this.nameSurnameHasError = false;
                this.emailHasError = false;

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

                if (hasError)
                    return;

                this.pending = true;
                this.resetForm = true;

                await httpClient
                    .post('Account/AddEmployee', {
                        nameSurname: this.nameSurname,
                        email: this.email,
                        phoneNumber: this.phoneNumber
                    })
                    .then(() => {
                        Swal.showSuccessMessage('Pracownik został dodany prawidłowo.', 'Wysłaliśmy do niego e-mail z linkiem aktywacyjnym. Poproś go aby sprawdził swoją skrzynkę i aktywował konto');

                        let employee = {
                            nameSurname: this.nameSurname,
                            email: this.email,
                            phoneNumber: this.phoneNumber
                        }

                        this.employees.push(employee);

                        this.nameSurname = '';
                        this.email = '';
                        this.phoneNumber = '';
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas dodawania pracownika');
                    })
                    .finally(() => {
                        this.pending = false;
                        this.resetForm = false;
                    });
            },
            async getEmployess() {
                this.getEmployeesPending = true;
                this.employeeMessage = "Wczytywanie danych...";
                await httpClient
                    .get('Account/GetEmployees')
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.employeeMessage = 'Brak pracowników';
                        }

                        this.employees = result.data;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania pracowników';
                        Swal.toastError(msg);
                        this.employeeMessage = msg;
                    })
                    .finally(() => {
                        this.getEmployeesPending = false;
                    });
            },
            async deleteEmployee(nameSurname, employeeId) {
                const deleteRequest = async () => {
                    return httpClient
                        .post('Account/deleteEmployee?employeeId=' + employeeId)
                        .then(() => {
                            Swal.toastSuccess('Pracownik został usunięty');

                            const employeeWithIdIndex = this.employees.findIndex((employee) => employee.id === employeeId);

                            if (employeeWithIdIndex > -1) {
                                this.employees.splice(employeeWithIdIndex, 1);
                            }
                        })
                        .catch(() => {
                            Swal.toastError('Wystąpił błąd podczas usuwania pracownika');
                        })
                        .finally(() => {
                            this.employeeMessage = "Brak pracowników";
                        });
                };

                await Swal.deleteRequestDialog(deleteRequest, 'Czy na pewno chcesz usunąć pracownika?', nameSurname);
            }
        },
        mounted: function () {
            this.getEmployess();
        },
        watch: {
            nameSurname(val) {
                if (this.resetForm)
                    return;

                if (val.length > 0) {
                    this.nameSurnameHasError = false;
                }
                else {
                    this.nameSurnameHasError = true
                }
            },
            email(val) {
                if (this.resetForm)
                    return;

                if (val.length > 0) {
                    this.emailHasError = false;
                }
                else {
                    this.emailHasError = true
                }
            }
        }
    }
</script>