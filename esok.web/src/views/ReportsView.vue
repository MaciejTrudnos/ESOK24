<template>
    <BreadcrumbComponent pageName='Raporty' />

    <section class="content">
        <div class="container-fluid">


            <div class="row">
                <div class="col-lg-6">
                    <div class="card">
                        <div class="card-header border-0">
                            <div class="d-flex justify-content-between">
                                <h3 class="card-title">Wypożyczenia</h3>
                                <!--<a href="javascript:void(0);">View Report</a>-->
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="d-flex">
                                <p class="d-flex flex-column">
                                    <span class="text-bold text-lg"></span>
                                </p>
                                <p class="ml-auto d-flex flex-column text-right">
                                    <span class="text-muted">Łącznie {{totalLastTwoWeek}} wypożyczeń</span>
                                </p>
                            </div>
                            <div class="position-relative mb-4">
                                <div class="chartjs-size-monitor"><div class="chartjs-size-monitor-expand"><div class=""></div></div><div class="chartjs-size-monitor-shrink"><div class=""></div></div></div>
                                <canvas id="visitors-chart" height="200" style="display: block; width: 327px; height: 200px;" width="327" class="chartjs-render-monitor"></canvas>
                            </div>
                            <div class="d-flex flex-row justify-content-end">
                                <span class="mr-2">
                                    <i class="fas fa-square text-primary"></i> Bieżący tydzień
                                </span>
                                <span>
                                    <i class="fas fa-square text-gray"></i> Poprzedni tydzień
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header border-0">
                            <h3 class="card-title">Produkty</h3>
                        </div>

                        <div v-if="getProductsPending" class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                    <label>{{productMessage}}</label>
                                </div>
                            </div>
                        </div>

                        <div v-else-if="products.length > 0" class="card-body table-responsive p-0" style="height: 342px;">
                            <table class="table table-striped table-valign-middle table-head-fixed">
                                <thead>
                                    <tr>
                                        <th style="width: 10px">#</th>
                                        <th>Produkt</th>
                                        <th class="text-wrap" style="min-width: 120px;">Cena</th>
                                        <th class="text-wrap">Jednostka rozliczeniowa</th>
                                        <th class="text-wrap" style="min-width: 100px;">Ilość</th>
                                        <th class="text-wrap" style="min-width: 100px;">Wypożyczono</th>
                                        <th class="text-wrap" style="min-width: 100px;">Pozostało dostępnych</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(product, index) in products" :key="product.id">
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
                                                {{addZeroes(product.netPrice)}} zł netto
                                            </a>
                                            <br>
                                            <small>
                                                {{computeGrossPrice(product.netPrice)}} zł brutto
                                            </small>
                                        </td>
                                        <td>{{translateUnit(product.unit)}}</td>
                                        <td>{{product.total}} szt.</td>
                                        <td>{{product.total - product.quanity}} szt.</td>
                                        <td>{{product.quanity}} szt.</td>

                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div v-else class="card-body p-0">
                            <div>
                                <div class="card-body">
                                    <label>{{productMessage}}</label>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="col-lg-6">
                    <div class="card">
                        <div class="card-header border-0">
                            <div class="d-flex justify-content-between">
                                <h3 class="card-title">Przychód roczny</h3>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="d-flex">
                                <p class="d-flex flex-column">
                                    <span class="text-bold text-lg"></span>
                                </p>
                                <p class="ml-auto d-flex flex-column text-right">
                                    <span class="text-muted">Łącznie {{totalLastYearRevenue}} zł</span>
                                </p>
                            </div>
                            <div class="position-relative mb-4">
                                <div class="chartjs-size-monitor"><div class="chartjs-size-monitor-expand"><div class=""></div></div><div class="chartjs-size-monitor-shrink"><div class=""></div></div></div>
                                <canvas id="sales-chart" height="200" style="display: block; width: 327px; height: 200px;" width="327" class="chartjs-render-monitor"></canvas>
                            </div>
                            <div class="d-flex flex-row justify-content-end">
                                <span class="mr-2">
                                    <i class="fas fa-square text-primary"></i> Bieżący rok
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header border-0">
                            <h3 class="card-title">Podsumowanie bieżącego miesiąca</h3>
                        </div>
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center border-bottom mb-3">
                                <p class="text-black text-xl">
                                    <i class="ion ion-ios-cart-outline"></i>
                                </p>
                                <p class="d-flex flex-column text-right">
                                    <span class="text-muted">AKTYWNE WYPOŻYCZENIA</span>
                                    <span class="font-weight-bold text-black text-xl">
                                        {{activeRentsCount}} szt.
                                    </span>
                                </p>
                            </div>

                            <div class="d-flex justify-content-between align-items-center border-bottom mb-3">
                                <p class="text-black text-xl">
                                    <i class="ion ion-ios-checkmark-outline"></i>
                                </p>
                                <p class="d-flex flex-column text-right">
                                    <span class="text-muted">ZAKOŃCZONE WYPOŻYCZENIA</span>
                                    <span class="font-weight-bold text-black text-xl">
                                        {{rentsFinishedCount}} szt.
                                    </span>
                                </p>
                            </div>

                            <div class="d-flex justify-content-between align-items-center mb-0">
                                <p class="text-black text-xl">
                                    <i class="ion ion-social-usd-outline"></i>
                                </p>
                                <p class="d-flex flex-column text-right">
                                    <span class="text-muted">PRZYCHÓD Z ZAKOŃCZONYCH WYPOŻYCZEŃ</span>
                                    <span class="font-weight-bold text-black text-xl">
                                        {{finishedNetPrice}} zł
                                    </span>
                                </p>
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
    import { httpClient } from '@/common/httpClient';
    import Swal from '@/common/sweetalert2';
    import common from '@/common/common';

    export default {
        components: {
            BreadcrumbComponent
        },
        data() {
            return {
                thisWeek: [],
                lastWeek: [],
                lastYearRevenueStats : [],
                totalLastTwoWeek: "",
                totalLastYearRevenue: "",
                products: [],
                activeRentsCount: "0",
                rentsFinishedCount: "0",
                finishedNetPrice: "0",
                getProductsPending: false,
                productMessage: 'Brak produktów'
            }
        },
        methods: {
            async getRentFinished() {
                await httpClient
                    .get('Report/RentFinished')
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.productMessage = 'Brak produktów';
                        }

                        this.rentsFinishedCount = result.data.rentsFinishedCount;
                        this.finishedNetPrice = result.data.finishedNetPrice;
                        this.activeRentsCount = result.data.activeRentsCount;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania produktów';
                        Swal.toastError(msg);
                    });
            },
            async getProductsForRent() {
                this.getProductsPending = true;
                this.productMessage = "Wczytywanie danych...";
                await httpClient
                    .get('Report/GetProducts')
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.productMessage = 'Brak produktów';
                        }

                        this.products = result.data;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania produktów';
                        Swal.toastError(msg);
                        this.productMessage = msg;
                    })
                    .finally(() => {
                        this.getProductsPending = false;
                    });
            },
            async getLastTwoWeeksStats() {
                var thisWeek = [];
                var lastWeek = [];

                await httpClient
                    .get('Report/LastTwoWeeksStats')
                    .then((result) => {
                        thisWeek = result.data.thisWeek;
                        lastWeek = result.data.lastWeek;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania wypożyczeń';
                        Swal.toastError(msg);
                    });

                return {
                    thisWeek: thisWeek,
                    lastWeek: lastWeek,
                };
            },
            async getLastYearRevenueStats() {
                var lastYearRevenueStats = [];

                await httpClient
                    .get('Report/LastYearRevenueStats')
                    .then((result) => {
                        lastYearRevenueStats = result.data;
                    })
                    .catch(() => {
                        let msg = 'Wystąpił błąd podczas pobierania przychodów';
                        Swal.toastError(msg);
                    });

                return {
                    lastYearRevenueStats
                };
            },
            addZeroes(num) {
                return common.addZeroes(num);
            },
            computeGrossPrice(netPrice) {
                return common.computeGrossPrice(netPrice)
            },
            translateUnit(enumId) {
                return common.translateUnit(enumId);
            }
        },
        async mounted() {
            /* global Chart:false */

            await this.getProductsForRent();
            await this.getRentFinished();

            let lastTwoWeeksStats = await this.getLastTwoWeeksStats();
            let lastYearRevenue = await this.getLastYearRevenueStats();
            let months = ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'];

            let totalYear = lastYearRevenue.lastYearRevenueStats.reduce((a, b) => a + b, 0);
            this.totalLastYearRevenue = totalYear.toFixed(2);

            let sumThisWeek = lastTwoWeeksStats.thisWeek.reduce((a, b) => a + b, 0);
            let sumLastWeek = lastTwoWeeksStats.lastWeek.reduce((a, b) => a + b, 0);

            this.totalLastTwoWeek = sumThisWeek + sumLastWeek;

            $(function () {
                'use strict'

                var ticksStyle = {
                    fontColor: '#495057',
                    fontStyle: 'bold'
                }

                var mode = 'index'
                var intersect = true

                var $salesChart = $('#sales-chart')
                // eslint-disable-next-line no-unused-vars
                var salesChart = new Chart($salesChart, {
                    type: 'bar',
                    data: {
                        labels: months.slice(0, lastYearRevenue.lastYearRevenueStats.length),
                        datasets: [
                            {
                                backgroundColor: '#007bff',
                                borderColor: '#007bff',
                                data: lastYearRevenue.lastYearRevenueStats
                            }
                        ]
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            mode: mode,
                            intersect: intersect
                        },
                        hover: {
                            mode: mode,
                            intersect: intersect
                        },
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                // display: false,
                                gridLines: {
                                    display: true,
                                    lineWidth: '4px',
                                    color: 'rgba(0, 0, 0, .2)',
                                    zeroLineColor: 'transparent'
                                },
                                ticks: $.extend({
                                    beginAtZero: true,

                                    // Include a dollar sign in the ticks
                                    callback: function (value) {
                                        return value.toFixed(2) + ' zł'
                                    }
                                }, ticksStyle)
                            }],
                            xAxes: [{
                                display: true,
                                gridLines: {
                                    display: false
                                },
                                ticks: ticksStyle
                            }]
                        }
                    }
                })

                var $visitorsChart = $('#visitors-chart')
                // eslint-disable-next-line no-unused-vars
                var visitorsChart = new Chart($visitorsChart, {
                    data: {
                        labels: ['Poniedziałek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota', 'Niedziela'],
                        datasets: [{
                            type: 'line',
                            data: lastTwoWeeksStats.thisWeek,
                            backgroundColor: 'transparent',
                            borderColor: '#007bff',
                            pointBorderColor: '#007bff',
                            pointBackgroundColor: '#007bff',
                            fill: false
                            // pointHoverBackgroundColor: '#007bff',
                            // pointHoverBorderColor    : '#007bff'
                        },
                        {
                            type: 'line',
                            data: lastTwoWeeksStats.lastWeek,
                            backgroundColor: 'tansparent',
                            borderColor: '#ced4da',
                            pointBorderColor: '#ced4da',
                            pointBackgroundColor: '#ced4da',
                            fill: false
                            // pointHoverBackgroundColor: '#ced4da',
                            // pointHoverBorderColor    : '#ced4da'
                        }]
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            mode: mode,
                            intersect: intersect
                        },
                        hover: {
                            mode: mode,
                            intersect: intersect
                        },
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                // display: false,
                                gridLines: {
                                    display: true,
                                    lineWidth: '4px',
                                    color: 'rgba(0, 0, 0, .2)',
                                    zeroLineColor: 'transparent'
                                },
                                ticks: $.extend({
                                    beginAtZero: true,
                                    stepSize: 1,
                                    // Include a dollar sign in the ticks
                                    callback: function (value) {
                                        return value + ' szt.'
                                    }
                                }, ticksStyle)
                            }],
                            xAxes: [{
                                display: true,
                                gridLines: {
                                    display: false
                                },
                                ticks: ticksStyle
                            }]
                        },
                    }
                })
            })

            // lgtm [js/unused-local-variable]

        },
    }
</script>