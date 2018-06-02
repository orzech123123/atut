webpackJsonp([2],{

/***/ 161:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_vue__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__ = __webpack_require__(130);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__);
﻿


__WEBPACK_IMPORTED_MODULE_0_vue___default.a.use(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["ClientTable"]);

var vehicleIndexViewModel = function (model) {
    var vue = new __WEBPACK_IMPORTED_MODULE_0_vue___default.a({
        el: "#VehicleIndex",
        data: {
            message: "assaa",
            columns:
            ["name", "registrationNumber", "actions"],
            options: {
                headings: {
                    name: 'Nazwa',
                    registrationNumber: 'Numer rejestracyjny',
                    actions: 'Akcje'
                },
                sortable: ["name", "registrationNumber"],
                perPage: 20
            },
            data: model
        }
    });
}

vehicleIndexViewModel(window.model);

/***/ })

},[161]);