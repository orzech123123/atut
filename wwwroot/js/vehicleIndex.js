webpackJsonp([3],{

/***/ 163:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue__ = __webpack_require__(124);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_vue__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__ = __webpack_require__(131);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__);
﻿


__WEBPACK_IMPORTED_MODULE_0_vue___default.a.use(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["ClientTable"]);

var vehicleIndexViewModel = function (model) {
    let columns = ["name", "registrationNumber", "actions"];

    if (!!model.isAdmin) {
        columns.splice(0, 0, "companyColumn");
    }

    var vue = new __WEBPACK_IMPORTED_MODULE_0_vue___default.a({
        el: "#VehicleIndex",
        mounted: function() {
            for (var company of model.map(m => m.company)) {
                if (this.companies.filter(m => m.key === company.key).length == 0) {
                    this.companies.push(company);
                }
            }  

            this.companies = this.companies.sort(function (a, b) { return a.value.localeCompare(b.value); });
        },
        watch: {
            filterCompany: function () {
                __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["Event"].$emit('vue-tables.filter::company', this.filterCompany);
            },
            filterName: function () {
                __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["Event"].$emit('vue-tables.filter::name', this.filterName);
            },
            filterRegistrationNumber: function () {
                __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["Event"].$emit('vue-tables.filter::registrationNumber', this.filterRegistrationNumber);
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

/***/ })

},[163]);