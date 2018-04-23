import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import Datepicker from 'vuejs-datepicker';
import { ClientTable } from "vue-tables-2";
import moment from 'moment';
import Multiselect from 'vue-multiselect';
import VueResource from 'vue-resource';
import MultiselectCss from 'vue-multiselect/dist/vue-multiselect.min.css';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);
Vue.use(VueResource);

Validator.localize('pl', VeeValidatePolish);
Vue.use(VeeValidate);

Vue.component('countries-editor', {
    props: {
        items: {
            type: Array,
            required: true
        },  
    },
    template: '#countries-editor-template',
    data() {
        return {
            showError: false,
            countryNames: ["Polska [PL]", "Niemcy [D]", "Dania [DK]", "Słowenia [SI]", "Chorwacja [HR]", "Austria [A]", "Belgia [B]", "Holandia [NL]"],
            nameToAdd: null,
            distanceToAdd: 0,

            columns: ["name", "distance", "actions"],
            options: {
                headings: {
                    name: 'Nazwa kraju',
                    distance: 'Dystans [km]',
                    actions: 'Akcje'
                },
                sortable: [],
                perPage: 20
            }
        }
    },
    computed: {
        availableCountryNames : function() {
            return this.countryNames.filter(name => this.items.map(item => item.name).indexOf(name) < 0);
        }
    },
    methods: {
        add: function () {
            if (!this.nameToAdd || this.distanceToAdd <= 0 || this.distanceToAdd % 1 != 0) {
                this.showError = true;
                return;
            }

            this.items.push({ name: this.nameToAdd, distance: this.distanceToAdd });
            this.nameToAdd = null;
            this.distanceToAdd = 0;
            this.showError = false;
        },
        remove: function (name) {
            var item = this.items.filter(i => i.name == name)[0];
            this.items.splice(this.items.indexOf(item), 1);
        }
    }
});

Vue.component('invoices-editor', {
    props: {
        items: {
            type: Array,
            required: true
        } 
    },
    template: '#invoices-editor-template',
    components: {
        Datepicker
    },
    data() {
        return {
            showError: false,
            dateToAdd: null,
            typeToAdd: null,
            amountToAdd: 0,

            columns: ["dateColumn", "type", "amount", "actions"],
            options: {
//TODO nie dziala:                dateColumns: ["date"],
//                toMomentFormat: true,
//                dateFormat: "ll",
                headings: {
                    dateColumn: 'Data wystawienia',
                    type: 'Waluta',
                    amount: 'Kwota',
                    actions: 'Akcje'
                },
                sortable: [],
                perPage: 20
            }
        }
    },
    methods: {
        add: function () {
            if (!this.dateToAdd || !this.typeToAdd || this.amountToAdd <= 0 || new Date(this.dateToAdd) > new Date()) {
                this.showError = true;
                return;
            }

            this.items.push({ date: this.dateToAdd, type: this.typeToAdd, amount: this.amountToAdd });
            this.dateToAdd = null;
            this.typeToAdd = null;
            this.amountToAdd = 0;
            this.showError = false;
        },
        remove: function (index) {
            this.items.splice(index - 1, 1);
        },
        moment: function (date) {
            return moment(date).format("ll");
        }
    }
});

var JourneyEditViewModel = function (model) {
    model.availableVehicles = [];
    model.errorElement = null;

    var vue = new Vue({
        el: "#JourneyEdit",
        data: model,
        methods: {
            validateBeforeSubmit() {
                this.$validator.validateAll().then((result) => {
                    if (result) {
                        this.$refs.form.submit();
                    } else {
                        this.errorElement = document.querySelectorAll('[data-vv-name="' +
                            this.$validator.errors.items[0].field + '"]')[0];
                        this.$forceUpdate();
                    }
                });
            },
            recalculateOtherCountriesTotalDistance: function (totalDistance, countriesTotalDistance) {
                this.otherCountriesTotalDistance = totalDistance - countriesTotalDistance;
            },
            momentYyyyMmDd: function (date) {
                return moment(date).format("YYYY-MM-DD");
            },
            scrollDownToErrorElement: function () {
                this.errorElement.scrollIntoView(true);
            }
        },
        updated: function () {
            if (!!this.errorElement) {
                this.scrollDownToErrorElement();
                this.errorElement = null;
            }
        },
        mounted: function () {
            this.$refs.form.style.display = "block";

            $(".vdp-datepicker").find("input").addClass("form-control");

            this.$http.get('/Vehicle/GetAllForAuthorizedUser').then(response => {
                this.availableVehicles = response.body;
            });
        },
        components: {
            Datepicker,
            Multiselect
        },
        computed: {
            startDateDisplayModel: function () {
                return !!this.startDate ? moment(this.startDate).format('YYYY-MM-DD') : null;
            },
            endDateDisplayModel: function () {
                return !!this.endDate ? moment(this.endDate).format('YYYY-MM-DD') : null;
            },
            countriesTotalDistance: function () {
                return this.countries.length > 1 ? this.countries.map(item => item.distance).reduce((prev, next) => Number(prev) + Number(next)) : this.countries.length > 0 ? this.countries[0].distance : 0;
            }
        },
        watch: {
            totalDistance: function (val) {
                this.recalculateOtherCountriesTotalDistance(val, this.countriesTotalDistance);
            },
            countriesTotalDistance: function (val) {
                this.recalculateOtherCountriesTotalDistance(this.totalDistance, val);
            }
        }
    });
}

JourneyEditViewModel(window.model);