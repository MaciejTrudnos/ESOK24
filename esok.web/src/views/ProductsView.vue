<template>
    <BreadcrumbComponent pageName='Magazyn produktów' />

    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-6">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Dodaj produkt</h3>
                        </div>
                        <div>
                            <div class="card-body">
                                <div class="form-group">
                                    <label>Nazwa</label>
                                    <input type="text" v-bind:class="['form-control', { 'is-invalid': nameHasError }]" v-model="name" placeholder="Nazwa">
                                    <span class="text-red text-sm" v-if="nameHasError">
                                        Nazwa jest wymagana
                                    </span>
                                </div>
                                <div class="form-group">
                                    <label>Opis</label>
                                    <input type="text" class="form-control" v-model="description" placeholder="Opcjonalnie opis">
                                </div>
                                <div class="form-group">
                                    <label>Cena</label>
                                    <div class="row">
                                        <div class="col-sm">
                                            <div class="input-group">
                                                <input type="number" v-bind:class="['form-control', { 'is-invalid': netPriceHasError }]" v-model="netPrice" @focus="priceFocus = 'netPrice'" placeholder="0,00" @input="handlePriceInput">
                                                <div class="input-group-append">
                                                    <span class="input-group-text">zł netto</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm">
                                            <div class="input-group">
                                                <input type="number" v-bind:class="['form-control', { 'is-invalid': grossPriceHasError }]" v-model="grossPrice" @focus="priceFocus = 'grossPrice'" placeholder="0,00" @input="handlePriceInput">
                                                <div class="input-group-append">
                                                    <span class="input-group-text">zł brutto</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm">
                                            <select v-model="unit" class="form-control">
                                                <option value="Minute">Minuta</option>
                                                <option value="Hour">Godzina</option>
                                                <option value="Day">Dzień</option>
                                            </select>
                                        </div>
                                    </div>

                                    <span class="text-red text-sm" v-if="netPriceHasError">
                                        Cena jest wymagana
                                    </span>
                                </div>
                                <div class="form-group">
                                    <label>Ilość w magazynie</label>
                                    <input type="number" v-bind:class="['form-control', { 'is-invalid': quanityHasError }]" v-model="quanity" placeholder="0">
                                    <span class="text-red text-sm" v-if="quanityHasError">
                                        Ilość jest wymagana
                                    </span>
                                </div>
                            </div>

                            <div class="card-footer">
                                <SubmitButtonComponent text='Dodaj' :pending=pending :className="'btn btn-primary'" @click="addProduct()" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card card-primary">
                        <div class="card-header">
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

                        <div v-else-if="products.length > 0" class="card-body table-responsive p-0" style="height: 446px;">
                            <table class="table table-head-fixed table-hover text-nowrap">
                                <thead>
                                    <tr>
                                        <th style="width: 10px">#</th>
                                        <th>Produkt</th>
                                        <th>Cena</th>
                                        <th class="text-wrap">Jednostka rozliczeniowa</th>
                                        <th class="text-wrap">Ilość w magazynie</th>
                                        <th></th>
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
                                            <button type="submit" class="btn btn-info btn-block" data-toggle="modal" data-target="#modal-xl" @click="getProduct(product)">Edytuj</button>
                                        </td>
                                        <td>
                                            <DeleteButtonComponent text='Usuń' @click="deleteProduct(product.id, product.name)" />
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
            </div>
        </div>
    </section>

    <div class="modal fade" id="modal-xl" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edytuj</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Nazwa</label>
                        <input type="text" v-bind:class="['form-control', { 'is-invalid': editNameHasError }]" v-model="editName" placeholder="Nazwa">
                        <span class="text-red text-sm" v-if="editNameHasError">
                            Nazwa jest wymagana
                        </span>
                    </div>
                    <div class="form-group">
                        <label>Opis</label>
                        <input type="text" class="form-control" v-model="editDescription" placeholder="Opcjonalnie opis">
                    </div>
                    <div class="form-group">
                        <label>Cena</label>
                        <div class="row">
                            <div class="col-sm">
                                <div class="input-group">
                                    <input type="number" v-bind:class="['form-control', { 'is-invalid': editNetPriceHasError }]" v-model="editNetPrice" @focus="editPriceFocus='netPrice'" placeholder="0,00" @input="handlePriceInput">
                                    <div class="input-group-append">
                                        <span class="input-group-text">zł netto</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm">
                                <div class="input-group">
                                    <input type="number" v-bind:class="['form-control', { 'is-invalid': editGrossPriceHasError }]" v-model="editGrossPrice" @focus="editPriceFocus='grossPrice'" placeholder="0,00" @input="handlePriceInput">
                                    <div class="input-group-append">
                                        <span class="input-group-text">zł brutto</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm">
                                <select v-model="editUnit" class="form-control">
                                    <option value="Minute">Minuta</option>
                                    <option value="Hour">Godzina</option>
                                    <option value="Day">Dzień</option>
                                </select>
                            </div>
                        </div>

                        <span class="text-red text-sm" v-if="editNetPriceHasError">
                            Cena jest wymagana
                        </span>
                    </div>
                    <div class="form-group">
                        <label>Ilość w magazynie</label>
                        <input type="number" v-bind:class="['form-control', { 'is-invalid': editQuanityHasError }]" v-model="editQuanity" placeholder="0">
                        <span class="text-red text-sm" v-if="editQuanityHasError">
                            Ilość jest wymagana
                        </span>
                    </div>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="close">Zamknij</button>
                    <SubmitButtonComponent text='Zapisz' :pending=updatePending :className="'btn btn-primary'" @click="updateProduct()" />
                </div>
            </div>

        </div>

    </div>

</template>

<script>
    import BreadcrumbComponent from '@/components/BreadcrumbComponent.vue';
    import SubmitButtonComponent from '@/components/SubmitButtonComponent.vue';
    import DeleteButtonComponent from '@/components/DeleteButtonComponent.vue';
    import { httpClient } from '@/common/httpClient';
    import Swal from '@/common/sweetalert2';
    import common from '@/common/common';

    export default {
        data() {
            return {
                btnClass: 'btn btn-primary',
                name: "",
                description: "",
                netPrice: "",
                grossPrice: "",
                quanity: "",
                unit: "Hour",
                nameHasError: false,
                netPriceHasError: false,
                grossPriceHasError: false,
                quanityHasError: false,
                pending: false,
                priceFocus: "",
                getProductsPending: false,
                productMessage: 'Brak produktów',
                products: [],
                editId: "",
                editName: "",
                editDescription: "",
                editNetPrice: "",
                editGrossPrice: "",
                editQuanity: "",
                editUnit: "",
                editNameHasError: false,
                editNetPriceHasError: false,
                editGrossPriceHasError: false,
                editQuanityHasError: false,
                updatePending: false,
                editPriceFocus: "",
                originalName: "",
                originalDescription: "",
                originalNetPrice: "",
                originalGrossPrice: "",
                originalQuanity: "",
                originalUnit: "",
                resetForm: false
            }
        },
        components: {
            BreadcrumbComponent,
            SubmitButtonComponent,
            DeleteButtonComponent
        },
        methods: {
            async addProduct() {
                let hasError = false;

                this.nameHasError = false;
                this.netPriceHasError = false;
                this.grossPriceHasError = false;
                this.quanityHasError = false;

                if (this.name === '') {
                    this.nameHasError = true;
                    hasError = true;
                }

                if (this.netPrice === '') {
                    this.netPriceHasError = true;
                    hasError = true;
                }

                if (this.grossPrice === '') {
                    this.grossPriceHasError = true;
                    hasError = true;
                }

                if (this.quanity === '') {
                    this.quanityHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                if (this.products.find(x => x.name.toLowerCase() == this.name.toLowerCase())) {
                    Swal.toastError('Produkt o takiej nazwie już istnieje');
                    return;
                }

                this.pending = true;
                this.resetForm = true;
                
                await httpClient
                    .post('Product/AddProduct', {
                        name: this.name,
                        description: this.description,
                        netPrice: this.netPrice,
                        quanity: this.quanity,
                        unit: this.unit
                    })
                    .then((result) => {
                        Swal.toastSuccess('Produkt został dodany prawidłowo');

                        let product = {
                            id: result.data,
                            name: this.name,
                            description: this.description,
                            netPrice: this.netPrice,
                            quanity: this.quanity,
                            unit: this.unit
                        }

                        this.products.push(product);

                        this.name = '';
                        this.description = ''
                        this.netPrice = '';
                        this.grossPrice = '';
                        this.quanity = '';
                        this.unit = 'Hour';
                    })
                    .catch((ex) => {
                        if (ex.response.status == 409) {
                            Swal.toastError('Produkt o takiej nazwie już istnieje');
                            return;
                        }

                        Swal.toastError('Wystąpił błąd podczas dodawania produktu');
                    })
                    .finally(() => {
                        this.pending = false;
                        this.resetForm = false;
                    });      
            },
            async getProducts() {
                this.getProductsPending = true;
                this.productMessage = "Wczytywanie danych...";
                await httpClient
                    .get('Product/GetProducts')
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
            async deleteProduct(productId, name) {
                const deleteRequest = async () => {
                    return httpClient
                        .post('Product/deleteProduct?productId=' + productId)
                        .then(() => {
                            Swal.toastSuccess('Produkt został usunięty');

                            const productWithIdIndex = this.products.findIndex((product) => product.id === productId);

                            if (productWithIdIndex > -1) {
                                this.products.splice(productWithIdIndex, 1);
                            }
                        })
                        .catch(() => {
                            Swal.toastError('Wystąpił błąd podczas usuwania produktu');
                        })
                        .finally(() => {
                            this.productMessage = "Brak produktów";
                        });
                };

                await Swal.deleteRequestDialog(deleteRequest, 'Czy na pewno chcesz usunąć produkt?', name);
            },
            async updateProduct() {
                let hasError = false;

                this.editNameHasError = false;
                this.editNetPriceHasError = false;
                this.editGrossPriceHasError = false;
                this.editQuanityHasError = false;

                if (this.editName === '') {
                    this.editNameHasError = true;
                    hasError = true;
                }

                if (this.editNetPrice === '') {
                    this.editNetPriceHasError = true;
                    hasError = true;
                }

                if (this.editGrossPrice === '') {
                    this.editGrossPriceHasError = true;
                    hasError = true;
                }

                if (this.editQuanity === '') {
                    this.editQuanityHasError = true;
                    hasError = true;
                }

                if (hasError)
                    return;

                if (this.originalName == this.editName &&
                    this.originalDescription == this.editDescription &&
                    this.originalNetPrice == this.editNetPrice &&
                    this.originalQuanity == this.editQuanity &&
                    this.originalUnit == this.editUnit)
                {
                    Swal.toastSuccess('Produkt został zaktualizowany prawidłowo');
                    document.getElementById('close').click();
                    return;
                }

                this.updatePending = true;

                await httpClient
                    .put('Product/UpdateProduct', {
                        id: this.editId,
                        name: this.editName,
                        description: this.editDescription,
                        netPrice: this.editNetPrice,
                        quanity: this.editQuanity,
                        unit: this.editUnit
                    })
                    .then((response) => {
                        Swal.toastSuccess('Produkt został zaktualizowany prawidłowo');

                        const productWithIdIndex = this.products.findIndex((product) => product.id === this.editId);

                        this.products[productWithIdIndex].id = response.data;
                        this.products[productWithIdIndex].name = this.editName;
                        this.products[productWithIdIndex].description = this.editDescription;
                        this.products[productWithIdIndex].netPrice = this.editNetPrice;
                        this.products[productWithIdIndex].quanity = this.editQuanity;
                        this.products[productWithIdIndex].unit = this.editUnit;

                        document.getElementById('close').click();
                    })
                    .catch((ex) => {
                        if (ex.response.status == 409) {
                            Swal.toastError('Produkt o takiej nazwie już istnieje');
                            return;
                        }

                        Swal.toastError('Wystąpił błąd podczas aktualizacji produktu');
                    })
                    .finally(() => {
                        this.updatePending = false;
                    });
            },
            getProduct(product) {
                this.editId = product.id;
                this.editName = product.name;
                this.editDescription = product.description;
                this.editNetPrice = product.netPrice;
                this.editQuanity = product.quanity;
                this.editUnit = product.unit;

                this.originalName = product.name;
                this.originalDescription = product.description;
                this.originalNetPrice = product.netPrice;
                this.originalQuanity = product.quanity;
                this.originalUnit = product.unit;
            },
            addZeroes(num) {
                return common.addZeroes(num);
            },
            handlePriceInput(e) {
                let stringValue = e.target.value.toString()
                let regex = /^\d*(\.\d{1,2})?$/

                if (this.priceFocus === "grossPrice") {
                    if (!stringValue.match(regex) && this.grossPrice !== '') {
                        this.grossPrice = this.previousPrice
                    }
                    this.previousPrice = this.grossPrice
                } else {
                    if (!stringValue.match(regex) && this.netPrice !== '') {
                        this.netPrice = this.previousPrice
                    }
                    this.previousPrice = this.netPrice
                }
            },
            computeGrossPrice(netPrice) {
                return common.computeGrossPrice(netPrice)
            },
            computeNetPrice(grossPrice) {
                return common.computeNetPrice(grossPrice)
            },
            translateUnit(enumId) {
                return common.translateUnit(enumId);
            }
        },
        mounted: function () {
            this.getProducts();
        },
        watch: {
            name(val) {
                if (this.resetForm) 
                    return;
                    
                if (val.length > 0) {
                    this.nameHasError = false;
                }
                else {
                    this.nameHasError = true
                }
            },
            netPrice(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.netPriceHasError = false;
                }
                else {
                    this.netPriceHasError = true
                }

                if (this.priceFocus === "grossPrice")
                    return;

                this.grossPrice = this.computeGrossPrice(val);
            },
            grossPrice(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.grossPriceHasError = false;
                }
                else {
                    this.grossPriceHasError = true
                }

                if (this.priceFocus === "netPrice")
                    return;

                this.netPrice = this.computeNetPrice(val);
            },
            quanity(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.quanityHasError = false;
                }
                else {
                    this.quanityHasError = true
                }
            },
            editName(val) {
                if (this.resetForm)
                    return;

                if (val.length > 0) {
                    this.editNameHasError = false;
                }
                else {
                    this.editNameHasError = true
                }
            },
            editNetPrice(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.editNetPriceHasError = false;
                }
                else {
                    this.editNetPriceHasError = true
                }

                if (this.editPriceFocus === "grossPrice")
                    return;

                this.editGrossPrice = this.computeGrossPrice(val);
            },
            editGrossPrice(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.editGrossPriceHasError = false;
                }
                else {
                    this.editGrossPriceHasError = true
                }

                if (this.editPriceFocus === "netPrice")
                    return;

                this.editNetPrice = this.computeNetPrice(val);
            },
            editQuanity(val) {
                if (this.resetForm)
                    return;

                if (val > 0) {
                    this.editQuanityHasError = false;
                }
                else {
                    this.editQuanityHasError = true
                }
            }
        }
    }
</script>