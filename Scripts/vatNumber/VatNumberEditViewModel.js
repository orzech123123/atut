import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import { ClientTable } from "vue-tables-2";
import moment from 'moment';
import VueResource from 'vue-resource';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);
Vue.use(VueResource);

Validator.extend('requireNotNull', {
    getMessage: field => '',
    validate: value => value != "null"
});
Validator.localize('pl', VeeValidatePolish);
Vue.use(VeeValidate, {
    fieldsBagName: 'vFields',
    dictionary: {
        pl: {
            custom: {
                company: {
                    requireNotNull: (field) => 'Pole Firma jest wymagane.'
                }
            }
        }
    }
});

var VatNumberEditViewModel = function (model) {
    model.availableCompanies = [];
    model.errorElement = null;

    var vue = new Vue({
        el: "#VatNumberEdit",
        data: model,
        methods: {
            onCountryChange(companyId) {
                window.location = window.vatNumberEditUrl + "/" + companyId;
            }
        },
        mounted: function () {
            this.$refs.form.style.display = "block";

            this.$http.get('/Account/GetAllCompanies').then(response => {
                this.availableCompanies = response.body;
            });
        },
        computed: {
            selectedCompany: {
                get: function() {
                    if (!this.company) {
                        return "null";
                    }

                    let company = this.availableCompanies.find(c => c.key == this.company.key);
                    return !!company ? company.key : null;
                },
                set: function(newValue) {
                    if (newValue == "null") {
                        this.company = null;
                    } else {
                        let company = this.availableCompanies.find(c => c.key == newValue);
                        this.company = { key: company.key, value: company.value };
                    }
                }
            }
        }
    });
}

VatNumberEditViewModel(window.model);