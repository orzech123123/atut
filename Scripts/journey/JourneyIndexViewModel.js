import Vue from "vue";
import { ClientTable, ServerTable, Event } from "vue-tables-2";
import moment from 'moment';
import Datepicker from 'vuejs-datepicker';
import VueResource from 'vue-resource';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);
Vue.use(ServerTable);
Vue.use(VueResource);

var journeyIndexViewModel = function (isAdmin) {
    var columns = ["vehiclesColumn", "startingPlace", "finalPlace", "startDate", "endDate", "actions"];
    
    if (!!isAdmin) {
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
                    .filter(ch => ch.$el.className == "VueTables VueTables--server")[0]
                    .data
                    .map(row => row.id);
                
                if (!this.filterCompany || !this.filterCountry || !this.filterFromDate || !this.filterToDate || journeyIds.length == 0) {
                    alert("Wybierz Firmę, Datę raportu od, Datę raportu do, Kraj oraz upewnij się, że masz na liście co najmniej jedną Trasę");
                    return;
                }

                let url = "/Report/GenerateReport?" +
                    "companyId=" +
                    this.filterCompany +
                    "&country=" +
                    this.filterCountry +
                    "&dateFrom=" +
                    moment(this.filterFromDate).format("YYYY-MM-DD") +
                    "&dateTo=" +
                    moment(this.filterToDate).format("YYYY-MM-DD") +
                    "&journeyIds=" +
                    journeyIds.join("&journeyIds=");

                window.open(url);
            }); 

            $("#notifyAdmin").on("click", () => {
                let journeyIds = this.$children
                    .filter(ch => ch.$el.className == "VueTables VueTables--server")[0]
                    .data 
                    .map(row => row.id);

                if (!this.filterCountry || !this.filterFromDate || !this.filterToDate || journeyIds.length == 0) {
                    alert("Wybierz Datę raportu od, Datę raportu do oraz Kraj, aby poinformować administratora, którego kraju rozliczenie zakończyłeś.\n" +
                        "Upewnij się także, że masz na liście co najmniej jedną Trasę.");
                    return;
                }

                let url = '/Report/NotifyAdmin?country=' +
                    this.filterCountry +
                    "&dateFrom=" +
                    moment(this.filterFromDate).format("YYYY-MM-DD") +
                    "&dateTo=" +
                    moment(this.filterToDate).format("YYYY-MM-DD") +
                    "&journeyIds=" +
                    journeyIds.join("&journeyIds=");

                this.$http.post(url).then(() => {
                    alert("Administrator został poinformowany o zakończeniu rozliczenia kraju " + this.filterCountry + ".");
                });
            });

            //TODO
            this.$http.get('/Account/GetAllCompanies').then(response => {
                this.companies = response.body;
            });
            this.$http.get('/Account/GetAllCountries').then(response => {
                this.countries = response.body;
            });
            //for (var company of model.map(m => m.company)) {
            //    if (this.companies.filter(m => m.key === company.key).length == 0) {
            //        this.companies.push(company);
            //    }
            //}
            //for (var country of model.map(m => m.countries).reduce((a, b) => a.concat(b), [])) {
            //    if (this.countries.filter(m => m.name === country.name).length == 0) {
            //        this.countries.push(country);
            //    }
            //}

            this.companies = this.companies.sort(function (a, b) { return a.value.localeCompare(b.value); });
        },
        watch: {
            filterCompany: function () {
                this.$refs.table.refresh();
//                Event.$emit('vue-tables.filter::company', this.filterCompany);
            },  
            filterCountry: function () {
                this.$refs.table.refresh();
//                Event.$emit('vue-tables.filter::country', this.filterCountry);
            },
            filterFromDate: function () {
                this.$refs.table.refresh();
//                Event.$emit('vue-tables.filter::dateFrom', this.filterFromDate);
            }, 
            filterToDate: function () {
                this.$refs.table.refresh();
//                Event.$emit('vue-tables.filter::dateTo', this.filterToDate);
            }  
        },
        data: {
            countries: [],
            companies: [],
            filterCompany: null,
            filterFromDate: null,
            filterToDate: null,
            filterCountry: null,
            columns: columns,
            options: {
                requestFunction: function (params) {
                    var dateFrom = !!vue && !!vue.filterFromDate ? moment(vue.filterFromDate).format("YYYY-MM-DD") : "";
                    var dateTo = !!vue && !!vue.filterToDate ? moment(vue.filterToDate).format("YYYY-MM-DD") : "";
                    var company = !!vue && !!vue.filterCompany ? vue.filterCompany : "";
                    var country = !!vue && !!vue.filterCountry ? vue.filterCountry : "";

                    return this
                        .$http
                        .get('/Journey/FetchAll?ascending=' + params.ascending + "&orderBy=" + (!!params.orderBy ? params.orderBy : "") + "&page=" + params.page + "&limit=" + params.limit + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&company=" + company + "&country=" + country);
                },
                responseAdapter: function (response) {
                    return {
                        data: response.data.data,
                        count: response.data.count
                    };
                },
                columnsClasses: {
                    actions: "VueTables__atut_actions_width"
                },
                rowClassCallback: function(row) {
                    if (row.isNotified) {
                        return "journey-is-notified";
                    }

                    return null;
                },
                dateColumns: ["startDate", "endDate"], 
                toMomentFormat: true,
                dateFormat: "ll",
                headings: {
                    companyColumn: "Firma",
                    vehiclesColumn: 'Pojazdy',
                    startingPlace: 'Miejscowość początkowa',
                    finalPlace: 'Miejscowość końcowa + kraj',
                    startDate: 'Data wyjazdu',
                    endDate: 'Data powrotu',
                    actions: 'Akcje'
                },
                sortable: ["startingPlace", "finalPlace", "startDate", "endDate"],
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
                    name: 'country',
                    callback: function (row, key) {
                        if (!key) {
                            return true;
                        }

                        return row.countries.filter(m => m.name == key).length > 0;
                    }
                    },
                    {
                        name: 'dateFrom',
                        callback: function (row, key) {
                            if (!key) {
                                return true;
                            }

                            key.setHours(0, 0, 0, 0);
                            return row.endDate >= moment(key);
                        }
                    },
                    {
                        name: 'dateTo',
                        callback: function (row, key) {
                            if (!key) {
                                return true;
                            }

                            key.setHours(23, 59, 59, 0);
                            return row.endDate <= moment(key);
                        }
                    }]
            }
        }
    });
}

journeyIndexViewModel(window.isAdmin);