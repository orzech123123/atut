import Vue from "vue";
import { ClientTable, Event } from "vue-tables-2";
import moment from 'moment';
import Datepicker from 'vuejs-datepicker';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);

var journeyIndexViewModel = function (model) {
    var columns = ["vehiclesColumn", "startingPlace", "finalPlace", "startDate", "endDate", "actions"];
    
    if (!!model.isAdmin) {
        columns.splice(0, 0, "companyColumn");
    }

    var vue = new Vue({
        el: "#JourneyIndex",
        components: {
            Datepicker
        },
        mounted: function () {
            $(".vdp-datepicker").find("input").addClass("form-control");
            
            $("#generateReport").on("click", () => {
                let journeyIds = this.$children
                    .filter(ch => ch.$el.className == "VueTables VueTables--client")[0]
                    .filteredData
                    .map(row => row.id);

                let journeyIdsString = "journeyIds=" + journeyIds.join("&journeyIds=");

                window.open("/Report/GenerateReport?" + journeyIdsString);
            });

            for (var company of model.map(m => m.company)) {
                if (this.companies.filter(m => m.key === company.key).length == 0) {
                    this.companies.push(company);
                }
            }
            for (var country of model.map(m => m.countries).reduce((a, b) => a.concat(b))) {
                if (this.countries.filter(m => m.name === country.name).length == 0) {
                    this.countries.push(country);
                }
            }
        },
        watch: {
            filterCompany: function() {
                Event.$emit('vue-tables.filter::company', this.filterCompany);
            },  
            filterCountry: function() {
                Event.$emit('vue-tables.filter::country', this.filterCountry);
            },
            filterFromDate: function() {
                Event.$emit('vue-tables.filter::dateFrom', this.filterFromDate);
            }, 
            filterToDate: function() {
                Event.$emit('vue-tables.filter::dateTo', this.filterToDate);
            }  
        },
        data: {
            countries: [],
            companies: [],
            columns: columns,
            options: {
                dateColumns: ["startDate", "endDate"], 
                toMomentFormat: true,
                dateFormat: "ll",
                headings: {
                    companyColumn: "Firma",
                    vehiclesColumn: 'Pojazdy',
                    startingPlace: 'Miejscowość wsiadania',
                    finalPlace: 'Miejscowość docelowa + kraj',
                    startDate: 'Data wyjazdu',
                    endDate: 'Data powrotu',
                    actions: 'Akcje'
                },
                sortable: ["companyColumn", "startingPlace", "finalPlace", "startDate", "endDate"],
                perPage: 20,
                customFilters: [{
                    name: 'company',
                    callback: function (row, key) {
                        if (key == "null") {
                            return true;
                        }
                        
                        return row.company.key == key;
                    }
                },
                {
                    name: 'country',
                    callback: function (row, key) {
                        if (key == "null") {
                            return true;
                        }

                        return row.countries.filter(m => m.name == key).length > 0;
                    }
                    },
                    {
                        name: 'dateFrom',
                        callback: function (row, key) {
                            if (key == "null") {
                                return true;
                            }

                            key.setHours(0, 0, 0, 0);
                            return row.startDate >= moment(key);
                        }
                    },
                    {
                        name: 'dateTo',
                        callback: function (row, key) {
                            if (key == "null") {
                                return true;
                            }

                            key.setHours(23, 59, 59, 0);
                            return row.startDate <= moment(key);
                        }
                    }]
            },
            data: model,
            filterCompany: null,
            filterFromDate: null,
            filterToDate: null,
            filterCountry: null
        }
    });
}

journeyIndexViewModel(window.model);