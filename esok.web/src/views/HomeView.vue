<template>
    <BreadcrumbComponent pageName='Strona główna' />

    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm">

                    <div class="small-box bg-info">
                        <div class="inner">
                            <h3>{{activeRent.length}}</h3>
                            <p>Aktywne wypożyczenia</p>
                        </div>
                        <div class="icon">
                            <i class="fas fa-rocket"></i>
                        </div>
                    </div>
                </div>

                <div class="col-sm">

                    <div class="small-box bg-success">
                        <div class="inner">
                            <h3>{{finishedRent.length}}</h3>
                            <p>Zakończone wypożyczenia</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Aktywne wypożyczenia</h3>
                            <div class="card-tools">
                                <div class="input-group input-group-sm" style="min-width: 250px; padding-bottom: 6px; margin-top: 6px;">
                                    <input type="text" name="table_search" v-model="search" class="form-control float-right" placeholder="Szukaj">
                                    <div class="input-group-append">
                                        <span class="input-group-text">
                                            <i class="fas fa-search"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div v-if="getActiveRentPending" class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                    <label>{{activeRentMessage}}</label>
                                </div>
                            </div>
                        </div>

                        <div v-else-if="activeRent.length > 0" class="card-body table-responsive p-0" style="height: 500px;">
                            <table class="table table-head-fixed table-hover text-nowrap">
                                <thead>
                                    <tr>
                                        <th>Numer wypożyczenia</th>
                                        <th>Uwagi</th>
                                        <th>Data utworzenia</th>
                                        <th>Czas trwania</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="rent in activeRent" :key="rent.id">
                                        <td>{{rent.numberRentOfDay}}</td>
                                        <td>{{rent.comments}}</td>
                                        <td>{{computeDate(rent.createDate)}}</td>
                                        <td>{{rent.elapsedTime}}</td>
                                        <td>
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#modal-xl" @click="getDetails(rent)">Szczegóły</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div v-else class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <label>{{activeRentMessage}}</label>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Zakończone wypożyczenia</h3>
                            <div class="card-tools" style="min-width: 250px;">
                                <VueDatePicker v-model="date" :format="format" :enableTimePicker="false" locale="pl" cancelText="Anuluj" selectText="Ok" range />
                            </div>
                        </div>

                        <div v-if="getFinishedRentPending" class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                    <label>{{activeRentMessage}}</label>
                                </div>
                            </div>
                        </div>

                        <div v-else-if="finishedRent.length > 0" class="card-body table-responsive p-0" style="height: 500px;">
                            <table class="table table-head-fixed table-hover text-nowrap">
                                <thead>
                                    <tr>
                                        <th>Numer wypożyczenia</th>
                                        <th>Uwagi</th>
                                        <th>Data zakończenia</th>
                                        <th>Czas trwania</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="rent in finishedRent" :key="rent.id">
                                        <td>{{rent.numberRentOfDay}}</td>
                                        <td>{{rent.comments}}</td>
                                        <td>{{computeDate(rent.createDate)}}</td>
                                        <td>{{rent.elapsedTime}}</td>
                                        <td>
                                            <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#modal-rent-finished" @click="getFinishedDetails(rent)">Szczegóły</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div v-else class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <label>{{finishedRentMessage}}</label>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="modal fade" id="modal-xl" style="display: none;" aria-hidden="true">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Numer wypożyczenia: {{detailsRentNumber}}</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div v-if="getDetailsPending" class="card-body p-0">
                                <div class="text-center">
                                    <div class="card-body">
                                        <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                        <label>Wczytywanie danych...</label>
                                    </div>
                                </div>
                            </div>
                            <div v-else>
                                <div class="row">
                                    <div class="col-8">
                                        <strong>Dane wypożyczającego:</strong><br>
                                        <address v-if="rentDetails.customer">
                                            {{rentDetails.customer.nameSurname}}<br v-if="rentDetails.customer.nameSurname">
                                            {{rentDetails.customer.phoneNumber}}<br v-if="rentDetails.customer.phoneNumber">
                                            {{rentDetails.customer.email}}<br v-if="rentDetails.customer.email">
                                            {{rentDetails.customer.street}} <br v-if="rentDetails.customer.street">
                                            {{rentDetails.customer.zipCode}}<br v-if="rentDetails.customer.zipCode">
                                            {{rentDetails.customer.city}}<br v-if="rentDetails.customer.city">
                                        </address>
                                        <address v-else>
                                            Brak danych
                                        </address>
                                    </div>
                                    <div class="col-4">
                                        <strong>Czas trwania:</strong><p>{{detailsElapsedTime}}</p>
                                    </div>
                                </div>

                                <div v-if="rentDetails.product.length > 0" class="card-body table-responsive p-0" style="height: 300px;">
                                    <table class="table table-head-fixed table-hover text-nowrap">
                                        <thead>
                                            <tr>
                                                <th style="width: 10px">#</th>
                                                <th>Produkt</th>
                                                <th>Cena</th>
                                                <th class="text-wrap">Jednostka rozliczeniowa</th>
                                                <th class="text-wrap" style="min-width: 160px;">Liczba sztuk</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(product, index) in rentDetails.product" :key="product.id">
                                                <td>{{index + 1}}</td>
                                                <td>
                                                    <a>
                                                        {{product.name}}
                                                    </a>
                                                    <br>
                                                    <small v-if="product.description">
                                                        {{product.description}}
                                                    </small>
                                                </td>
                                                <td>
                                                    <a>
                                                        {{common.addZeroes(product.netPrice)}} zł netto
                                                    </a>
                                                    <br>
                                                    <small>
                                                        {{common.computeGrossPrice(product.netPrice)}} zł brutto
                                                    </small>
                                                </td>
                                                <td>{{common.translateUnit(product.unit)}}</td>
                                                <td>{{product.quanity}} szt.</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-default" data-dismiss="modal" id="close">Zamknij</button>
                            <SubmitButtonComponent v-if="disableStopButton" text='Zakończ' :pending=stopPending :className="'btn btn-danger'" disabled />
                            <SubmitButtonComponent v-else text='Zakończ' :pending=stopPending :className="'btn btn-danger'" @click="finishRent()" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="modal-rent-finished" style="display: none;" aria-hidden="true">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Rachunek wypożyczenia o numerze: {{detailsRentNumber}}</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div v-if="getFinishedDetailsPending" class="card-body p-0">
                                <div class="text-center">
                                    <div class="card-body">
                                        <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                        <label>Wczytywanie danych...</label>
                                    </div>
                                </div>
                            </div>
                            <div v-else>
                                <div class="row">
                                    <div class="col-8">
                                        <strong>Dane wypożyczającego:</strong><br>
                                        <address v-if="rentDetails.customer">
                                            {{rentDetails.customer.nameSurname}}<br v-if="rentDetails.customer.nameSurname">
                                            {{rentDetails.customer.phoneNumber}}<br v-if="rentDetails.customer.phoneNumber">
                                            {{rentDetails.customer.email}}<br v-if="rentDetails.customer.email">
                                            {{rentDetails.customer.street}} <br v-if="rentDetails.customer.street">
                                            {{rentDetails.customer.zipCode}}<br v-if="rentDetails.customer.zipCode">
                                            {{rentDetails.customer.city}}<br v-if="rentDetails.customer.city">
                                        </address>
                                        <address v-else>
                                            Brak danych
                                        </address>
                                    </div>
                                    <div class="col-4">
                                        <strong>Czas trwania:</strong><p>{{detailsElapsedTime}}</p>
                                    </div>
                                </div>

                                <div v-if="rentDetails.product.length > 0" class="card-body table-responsive p-0" style="height: 300px;">
                                    <table class="table table-head-fixed table-hover text-nowrap">
                                        <thead>
                                            <tr>
                                                <th style="width: 10px">#</th>
                                                <th>Produkt</th>
                                                <th>Cena</th>
                                                <th class="text-wrap">Jednostka rozliczeniowa</th>
                                                <th class="text-wrap" style="min-width: 160px;">Liczba sztuk</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(product, index) in rentDetails.product" :key="product.id">
                                                <td>{{index + 1}}</td>
                                                <td>
                                                    <a>
                                                        {{product.name}}
                                                    </a>
                                                    <br>
                                                    <small v-if="product.description">
                                                        {{product.description}}
                                                    </small>
                                                </td>
                                                <td>
                                                    <a>
                                                        {{common.addZeroes(product.netPrice)}} zł netto
                                                    </a>
                                                    <br>
                                                    <small>
                                                        {{common.computeGrossPrice(product.netPrice)}} zł brutto
                                                    </small>
                                                </td>
                                                <td>{{common.translateUnit(product.unit)}}</td>
                                                <td>{{product.quanity}} szt.</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <div>
                                    <div class="float-right m-3">
                                        <strong>Do zapłaty: </strong><br>
                                        <strong>{{common.addZeroes(netPrice)}} zł netto</strong><br>
                                        <strong>{{common.computeGrossPrice(netPrice)}} zł brutto</strong><br>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-default" data-dismiss="modal" id="close">Zamknij</button>
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
    import Swal from '@/common/sweetalert2';
    import { httpClient } from '@/common/httpClient';
    import moment from 'moment';
    import common from '@/common/common';
    import VueDatePicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css';

    export default {
        data() {
            return {
                fullListActiveRent: [],
                activeRent: [],
                finishedRent: [],
                getActiveRentPending: false,
                getFinishedRentPending: false,
                getDetailsPending: false,
                getFinishedDetailsPending: false,
                activeRentMessage: 'Wczytywanie danych...',
                finishedRentMessage: 'Wczytywanie danych...',
                search: '',
                todayRent: 0,
                moment: moment,
                common: common,
                stopPending: false,
                rentDetails: {
                    customer: {},
                    product: {},
                },
                detailsElapsedTime: '',
                detailsId: 0,
                detailsRentNumber: '',
                netPrice: 0.00,
                disableStopButton: false,
                date: [new Date(new Date().setDate(new Date().getDate() - 7)), new Date()],
                format: 'dd.MM.yyyy'
            }
        },
        methods: {
            async getActiveRent() {
                this.getActiveRentPending = true;
                this.activeRentMessage = "Wczytywanie danych...";
                await httpClient
                    .get('Product/GetActiveRent')
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.activeRentMessage = 'Brak aktywnych wypożyczeń';
                        }

                        result.data.forEach(x => {
                            x.numberRentOfDay += '/' + moment(x.createDate).format("DDMMYY");
                        });

                        this.activeRent = result.data;

                        this.fullListActiveRent = result.data;

                        this.todayRent += result.data.filter(x => {
                            return moment(x.createDate).format("DD.MM.YYYY") == moment(new Date()).format("DD.MM.YYYY")
                        }).length;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania danych';
                        Swal.toastError(msg);
                        this.activeRentMessage = msg;
                    })
                    .finally(() => {
                        this.getActiveRentPending = false;
                    });
            },
            getRemainingTime: function () {
                var self = this;

                var currrentDate = new Date();

                this.activeRent
                    .forEach(x => {
                        const createDate = new Date(x.createDate);

                        const diffTime = Math.abs(currrentDate.getTime() - createDate);

                        let days = Math.floor(diffTime / (1000 * 60 * 60 * 24));
                        let hours = Math.floor((diffTime / (1000 * 60 * 60)) % 24);
                        let minutes = Math.floor((diffTime / 1000 / 60) % 60);
                        let seconds = Math.floor((diffTime / 1000) % 60);

                        let elapsedTime = String(days).padStart(2, '0') + ":" +
                            String(hours).padStart(2, '0') + ":" +
                            String(minutes).padStart(2, '0') + ":" +
                            String(seconds).padStart(2, '0');

                        x.elapsedTime = elapsedTime;
                    }
                    );

                setTimeout(function () { self.getRemainingTime() }, 1000);
            },
            async getDetails(rent) {
                this.getDetailsPending = true;
                this.disableStopButton = true;
                await httpClient
                    .get('Product/Details?id=' + rent.id)
                    .then((result) => {
                        this.rentDetails = result.data;
                        this.disableStopButton = false;
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas pobierania danych');
                    })
                    .finally(() => {
                        this.getDetailsPending = false; 
                    });

                this.detailsId = rent.id;
                this.detailsRentNumber = rent.numberRentOfDay
                this.computeDetailsElapsedTime()
            },

            async getFinishedDetails(rent) {
                this.getFinishedDetailsPending = true;
                await httpClient
                    .get('Product/GetFinishedDetails?id=' + rent.id)
                    .then((result) => {
                        this.rentDetails = result.data;
                        this.netPrice = result.data.netPrice;
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas pobierania danych');
                    })
                    .finally(() => {
                        this.getFinishedDetailsPending = false;
                    });

                this.detailsId = rent.id;
                this.detailsRentNumber = rent.numberRentOfDay
                this.detailsElapsedTime = rent.elapsedTime;
            },
            async getRentFinished() {
                this.getFinishedRentPending = true;
                this.finishedRentMessage = "Wczytywanie danych...";

                let dateFrom = moment(this.date[0]).format("DD.MM.YYYY");
                let dateTo = moment(this.date[1]).format("DD.MM.YYYY");

                await httpClient
                    .get('Product/GetRentFinished?dateFrom=' + dateFrom + '&dateTo=' + dateTo)
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.finishedRentMessage = 'Brak zakończonych wypożyczeń';
                        }

                        result.data.forEach(x => {
                            x.numberRentOfDay += '/' + moment(x.createDate).format("DDMMYY");
                        });

                        this.finishedRent = result.data;

                        this.todayRent += result.data.filter(x => {
                            return moment(x.createDate).format("DD.MM.YYYY") == moment(new Date()).format("DD.MM.YYYY")
                        }).length;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania danych';
                        Swal.toastError(msg);
                        this.finishedRentMessage = msg;
                    })
                    .finally(() => {
                        this.getFinishedRentPending = false;
                    });
            },
            async finishRent() {
                this.stopPending = true

                await httpClient
                    .post('Product/FinishRent?id=' + this.detailsId)
                    .then((response) => {
                        Swal.toastSuccess('Pomyślnie zakończono wypożyczanie o numerze ' + this.detailsRentNumber);

                        const finishedRent = this.activeRent.filter((rent) => rent.id === this.detailsId);
                        finishedRent[0].createDate = new Date();
                        this.finishedRent.unshift(finishedRent[0]);

                        const rentWithIdIndex = this.activeRent.findIndex((rent) => rent.id === this.detailsId);

                        if (rentWithIdIndex > -1) {
                            this.activeRent.splice(rentWithIdIndex, 1);
                        }

                        this.detailsElapsedTime = response.data.elapsedTime;
                        this.netPrice = response.data.netPrice;

                        $('#close').click();
                        $('#modal-rent-finished').modal('show');
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas kończenia wypożyczania');
                    })
                    .finally(() => {
                        this.stopPending = false;

                        if (this.fullListActiveRent.length == 0) {
                            this.activeRentMessage = 'Brak aktywnych wypożyczeń';
                        }
                    });
            },
            computeDetailsElapsedTime() {
                let self = this;

                let rent = this.activeRent.filter(x => x.id == self.detailsId);

                if (rent.length <= 0)
                    return;

                self.detailsElapsedTime = rent[0].elapsedTime;

                if (($("#modal-xl").data('bs.modal') || {})._isShown) {
                    setTimeout(function () { self.computeDetailsElapsedTime() }, 1000);
                }
            },
            computeDate(createDate) {
                var dateTime = new Date(createDate)

                return moment(dateTime).format("DD.MM.YYYY HH:mm:ss");
            }
        },
        components: {
            BreadcrumbComponent,
            SubmitButtonComponent,
            VueDatePicker
        },
        mounted: async function () {

            let getActiveRent = new Promise(
                () => this.getActiveRent()
            );

            let getRentFinished = new Promise(
                () => this.getRentFinished()
            );

            let getRemainingTime = new Promise(
                () => this.getRemainingTime()
            );

            await Promise.all([getActiveRent, getRentFinished, getRemainingTime]);
        },
        watch: {
            search(val) {
                this.activeRentMessage = "Brak aktywnych wypożyczeń";

                this.activeRent = this.fullListActiveRent;

                let filteredRent = this.activeRent
                    .filter(x => String(x.numberRentOfDay).includes(val) || x.comments.toLowerCase().includes(val.toLowerCase()))

                this.activeRent = filteredRent;
            },
            async date() {
               await this.getRentFinished();
            }
        }
    }
</script>