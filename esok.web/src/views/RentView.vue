<template>
    <BreadcrumbComponent  pageName='Wypożyczalnia' />
    
    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Formularz</h3>
                        </div>
                        <div class="card-body">
                            <div class="bs-stepper linear" id="stepper">
                                <div class="bs-stepper-header" role="tablist">
                                    <div class="row w-100 justify-content-center">
                                        <div class="col-sm d-flex flex-row justify-content-center p-0">
                                            <div class="step active" data-target="#logins-part">
                                                <button type="button" class="step-trigger" role="tab" aria-controls="logins-part" id="logins-part-trigger" aria-selected="true">
                                                    <span class="bs-stepper-circle">1</span>
                                                    <span class="bs-stepper-label">Wybierz produkty</span>
                                                </button>
                                            </div>
                                            <div class="line d-none d-md-block"></div>
                                        </div>
                                        <div class="col-sm d-flex flex-row justify-content-center p-0">
                                            <div class="line d-none d-md-block"></div>
                                            <div class="step" data-target="#information-part">
                                                <button type="button" class="step-trigger" role="tab" aria-controls="information-part" id="information-part-trigger" aria-selected="false" disabled="disabled">
                                                    <span class="bs-stepper-circle">2</span>
                                                    <span class="bs-stepper-label">Dane wypożyczającego</span>
                                                </button>
                                            </div>
                                            <div class="line d-none d-md-block"></div>
                                        </div>
                                        <div class="col-sm d-flex flex-row p-0">
                                            <div class="line d-none d-md-block"></div>
                                            <div class="step mx-auto mx-sm-0" data-target="#summary-part">
                                                <button type="button" class="step-trigger float-lg-right" role="tab" aria-controls="summary-part" id="summary-part-trigger" aria-selected="false" disabled="disabled">
                                                    <span class="bs-stepper-circle">3</span>
                                                    <span class="bs-stepper-label">Podsumowanie</span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bs-stepper-content">
                                    <div id="logins-part" class="content active dstepper-block" role="tabpanel" aria-labelledby="logins-part-trigger">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="card">
                                                    <div class="card-header">
                                                        <h3 class="card-title">Magazyn</h3>
                                                        <div class="card-tools">
                                                            <div class="input-group input-group-sm" style="width: 150px;">
                                                                <input type="text" name="table_search" v-model="search" class="form-control float-right" placeholder="Szukaj">
                                                                <div class="input-group-append">
                                                                    <span class="input-group-text">
                                                                        <i class="fas fa-search"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div v-if="getProductsPending" class="card-body p-0">
                                                        <div>
                                                            <div class="card-body">
                                                                <span class="spinner-grow spinner-grow-sm"></span>&nbsp;
                                                                <label>{{productMessage}}</label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div v-else-if="products.length > 0" class="card-body table-responsive p-0" style="height: 300px;">
                                                        <table class="table table-head-fixed table-hover text-nowrap">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 10px">#</th>
                                                                    <th>Produkt</th>
                                                                    <th>Cena</th>
                                                                    <th class="text-wrap">Jednostka rozliczeniowa</th>
                                                                    <th class="text-wrap">Dostępna ilość</th>
                                                                    <th></th>
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
                                                                    <td>{{product.quanity}} szt.</td>
                                                                    <td>
                                                                        <button type="button" class="btn btn-primary btn-sm" @click="addProductsForRent(product)">Dodaj</button>
                                                                    </td>
                                                                    <td>
                                                                        <button v-if="pendingId == product.id" class="btn btn btn-success btn-sm" disabled>
                                                                            <span class="spinner-grow spinner-grow-sm"></span>
                                                                        </button>

                                                                        <button v-else type="submit" class="btn btn btn-success btn-sm" @click="fastRent(product.id)">Wypożycz</button>
                                                                    </td>
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
                                            <div class="col-md-6">
                                                <div class="card">
                                                    <div class="card-header">
                                                        <h3 class="card-title">Wybrane</h3>
                                                    </div>
                                                    <div v-if="productsForRent.length > 0" class="card-body table-responsive p-0" style="height: 300px;">
                                                        <table class="table table-head-fixed table-hover text-nowrap">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 10px">#</th>
                                                                    <th>Produkt</th>
                                                                    <th>Cena</th>
                                                                    <th class="text-wrap">Jednostka rozliczeniowa</th>
                                                                    <th class="text-wrap" style="min-width: 160px;">Liczba sztuk</th>
                                                                    <th></th>
                                                                    <th></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr v-for="(product, index) in productsForRent" :key="product.id">
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
                                                                    <td>
                                                                        <input class="form-control form-control-sm" type="number" min="1" :max="product.quanity" v-model="product.quanityRent">
                                                                    </td>
                                                                    <td>z {{product.quanity}} szt.</td>
                                                                    <td>
                                                                        <button type="submit" class="btn btn-danger btn-sm" @click="removeProductsForRent(product)">
                                                                            <i class="fas fa-trash">
                                                                            </i>
                                                                            Usuń
                                                                        </button>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>

                                                    <div v-else class="card-body p-0">
                                                        <div>
                                                            <div class="card-body">
                                                                <label>Brak produktów</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <button class="btn btn-primary" v-if="productsForRent.length > 0" onclick="stepper.next()">Dalej</button>
                                        <button class="btn btn-primary" v-else disabled >Dalej</button>
                                    </div>

                                    <div id="information-part" class="content" role="tabpanel" aria-labelledby="information-part-trigger">
                                        <div class="row nowrap">
                                            <div class="col-sm">
                                                <div class="form-group">
                                                    <label>Imię i nazwisko</label>
                                                    <input class="form-control" type="text" v-model="nameSurname" placeholder="Imię i nazwisko">
                                                </div>
                                                <div class="form-group">
                                                    <label>Numer telefonu</label>
                                                    <input class="form-control" type="text"  v-model="phoneNumber" v-maska data-maska="###-###-###" placeholder="Numer telefonu">
                                                </div>
                                                <div class="form-group">
                                                    <label>E-mail</label>
                                                    <input class="form-control" type="email" v-model="email" placeholder="E-mail">
                                                </div>
                                            </div>
                                            <div class="col-sm">
                                                <div class="form-group">
                                                    <label>Ulica i numer</label>
                                                    <input class="form-control" type="text" v-model="street" placeholder="Ulica i numer">
                                                </div>
                                                <div class="form-group">
                                                    <label>Kod pocztowy</label>
                                                    <input class="form-control" type="text" v-model="zipCode" v-maska data-maska="##-###" placeholder="Kod pocztowy">
                                                </div>
                                                <div class="form-group">
                                                    <label>Miejscowość</label>
                                                    <input class="form-control" type="text" v-model="city" placeholder="Miejscowość">
                                                </div>
                                            </div>
                                        </div>
                                        <button class="btn btn-primary mr-1" onclick="stepper.previous()">Wstecz</button>
                                        <button class="btn btn-primary mr-1" onclick="stepper.next()">Dalej</button>
                                    </div>

                                    <div id="summary-part" class="content" role="tabpanel" aria-labelledby="summary-part-trigger">
                                        <div class="invoice border-0 mb-3">
                                            <div class="row">
                                                <div class="col-md">
                                                    <div class="row invoice-info">
                                                        <div class="col invoice-col">
                                                            <strong>Dane wypożyczającego:</strong><br>
                                                            <address>
                                                                {{nameSurname}}<br v-if="nameSurname">
                                                                {{phoneNumber}}<br v-if="phoneNumber">
                                                                {{email}}<br v-if="email">
                                                                {{street}} <br v-if="street">
                                                                {{zipCode}}<br v-if="zipCode">
                                                                {{city}}<br v-if="city">
                                                            </address>
                                                            
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md">
                                                    <div class="form-group">
                                                        <label>Uwagi:</label>
                                                        <textarea class="form-control" rows="4" v-model="comments" placeholder="Uwagi..."></textarea>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div v-if="productsForRent.length > 0" class="card-body table-responsive p-0" style="height: 300px;">
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
                                                                <tr v-for="(product, index) in productsForRent" :key="product.id">
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
                                                                    <td>{{product.quanityRent}} szt.</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <button class="btn btn-primary mr-1" onclick="stepper.previous()">Wstecz</button>
                                        
                                        <SubmitButtonComponent v-if="productsForRent.length > 0" text='Wypożycz' :className="'btn btn-success'" :pending=pending @click="rent()" />

                                        <button v-else type="submit" class="btn btn-success" disabled>Wypożycz</button>
                                    </div>
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
    import Stepper from 'bs-stepper'
    import { httpClient } from '@/common/httpClient';
    import common from '@/common/common';
    import Swal from '@/common/sweetalert2';
    import router from '../router';
    import { vMaska } from "maska"
    import moment from 'moment';

    export default {
        directives: {
            maska: vMaska
        },
        data() {
            return {
                fullListProducts: [],
                search: "",
                products: [],
                productsForRent: [],
                getProductsPending: false,
                productMessage: 'Brak produktów',
                nameSurname: "",
                phoneNumber: "",
                email: "",
                street: "",
                zipCode: "",
                city: "",
                comments: "",
                pending: false,
                pendingId: 0,
            }
        },
        components: {
            BreadcrumbComponent,
            SubmitButtonComponent
        },
        mounted: function () {
            $('.select2').select2()

            window.stepper = new Stepper(document.querySelector('.bs-stepper'));

            this.getProductsForRent();
        },
        methods: {
            async getProductsForRent() {
                this.getProductsPending = true;
                this.productMessage = "Wczytywanie danych...";
                await httpClient
                    .get('Product/GetProductsForRent')
                    .then((result) => {
                        if (result.data.length == 0) {
                            this.productMessage = 'Brak produktów';
                        }

                        this.products = result.data;
                        this.fullListProducts = result.data;
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
            addProductsForRent(product) {
                product["quanityRent"] = 1;

                this.productsForRent.push(product);

                const productWithIdIndex = this.products.findIndex((p) => p.id === product.id);

                if (productWithIdIndex > -1) {
                    this.products.splice(productWithIdIndex, 1);
                }

                const fullListWithIdIndex = this.fullListProducts.findIndex((p) => p.id === product.id);

                if (fullListWithIdIndex> -1) {
                    this.fullListProducts.splice(fullListWithIdIndex, 1);
                }

                if (this.products.length == 0) {
                    this.productMessage = 'Brak produktów';
                }

                this.search = "";
            },
            removeProductsForRent(product) {
                this.fullListProducts.push(product);

                const productWithIdIndex = this.productsForRent.findIndex((p) => p.id === product.id);

                if (productWithIdIndex > -1) {
                    this.productsForRent.splice(productWithIdIndex, 1);
                }

                if (this.products.length == 0) {
                    this.productMessage = 'Brak produktów';
                }

                this.search = "";
            },
            async rent() {
                this.pending = true
               
                await httpClient
                    .post('Product/Rent', {
                        comments: this.comments,
                        customer: {
                            nameSurname: this.nameSurname,
                            phoneNumber: this.phoneNumber,
                            email: this.email,
                            street: this.street,
                            zipCode: this.zipCode,
                            city: this.city
                        },
                        products: this.productsForRent.map(x => ({ id: x.id, quanity: x.quanityRent }))
                    })
                    .then((result) => {
                        Swal.toastSuccessWithStopTimer('Pomyślnie wypożyczono produkt. Numer wypożyczenia to: ' + result.data + '/' + moment(new Date()).format("DDMMYY"));

                        router.push(
                            { name: "home" }
                        );
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas wypożyczania produktów');
                    })
                    .finally(() => {
                        this.pending = false;
                    });
            },
            async fastRent(productId) {
                this.pendingId = productId

                let products = [];
                products.push({ id: productId, quanity: 1 });

                await httpClient
                    .post('Product/Rent', {
                        comments: null,
                        customer: {
                            nameSurname: null,
                            phoneNumber: null,
                            email: null,
                            street: null,
                            zipCode: null,
                            city: null
                        },
                        products: products
                    })
                    .then((result) => {
                        Swal.toastSuccessWithStopTimer('Pomyślnie wypożyczono produkt. Numer wypożyczenia to: ' + result.data + '/' + moment(new Date()).format("DDMMYY"));

                        router.push(
                            { name: "home" }
                        );
                    })
                    .catch(() => {
                        Swal.toastError('Wystąpił błąd podczas wypożyczania produktu');
                    })
                    .finally(() => {
                        this.pendingId = 0;
                    });
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
        watch: {
            productsForRent: {
                handler: function (product) {
                    product.forEach((value) => {
                        if (value.quanityRent > value.quanity) {
                            value.quanityRent = value.quanity;
                        }

                        if (value.quanityRent <= 0) {
                            value.quanityRent = 1;
                        }
                    });
                },
                deep: true,
            },
            search(val) {
                this.productMessage = "Brak produktów";

                this.products = this.fullListProducts;

                let filteredPorducts = this.products
                    .filter(x => x.name.toLowerCase().includes(val.toLowerCase()) || x.description.toLowerCase().includes(val.toLowerCase()))

                this.products = filteredPorducts;
            }
        }
    }
</script>