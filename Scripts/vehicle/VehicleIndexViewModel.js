import Vue from "vue";
import { ClientTable, Event } from "vue-tables-2";

Vue.use(ClientTable);

var vehicleIndexViewModel = function (model) {
    let columns = ["name", "registrationNumber", "actions"];

    if (!!model.isAdmin) {
        columns.splice(0, 0, "companyColumn");
    }

    var vue = new Vue({
        el: "#VehicleIndex",
        mounted: function() {
            for (var company of model.map(m => m.company)) {
                if (this.companies.filter(m => m.key === company.key).length == 0) {
                    this.companies.push(company);
                }
            }  
        },
        watch: {
            filterCompany: function () {
                Event.$emit('vue-tables.filter::company', this.filterCompany);
            },
            filterName: function () {
                Event.$emit('vue-tables.filter::name', this.filterName);
            },
            filterRegistrationNumber: function () {
                Event.$emit('vue-tables.filter::registrationNumber', this.filterRegistrationNumber);
            }
        },
        data: {
            companies: [],
            columns: columns,
            options: {
                headings: {
                    companyColumn: "Firma",
                    name: 'Nazwa',
                    registrationNumber: 'Numer rejestracyjny',
                    actions: 'Akcje'
                },
                sortable: ["companyColumn", "name", "registrationNumber"],
                perPage: 20, 
                customFilters: [{
                        name: 'company',
                        callback: function (row, key) {
                            if (!key) {
                                return true;
                            }

                            return row.company.key == key;
                        }
                    },
                    {
                        name: 'name',
                        callback: function (row, key) {
                            if (!key) {
                                return true;
                            }

                            return row.name.toLowerCase().indexOf(key.toLowerCase()) !== -1;
                        }
                    },
                    {
                        name: 'registrationNumber',
                        callback: function (row, key) {
                            if (!key) {
                                return true;
                            }

                            return row.registrationNumber.toLowerCase().indexOf(key.toLowerCase()) !== -1;
                        }
                    }]
            },
            data: model,
            filterCompany: null,
            filterName: null,
            filterRegistrationNumber: null
        }
    });
}

vehicleIndexViewModel(window.model);