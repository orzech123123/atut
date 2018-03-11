webpackJsonp([1],{

/***/ 193:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue__ = __webpack_require__(2);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_vue___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_vue__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_vue_tables_2___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__);
﻿


__WEBPACK_IMPORTED_MODULE_0_vue___default.a.use(__WEBPACK_IMPORTED_MODULE_1_vue_tables_2__["ClientTable"]);

var journeyIndexViewModel = function (model) {
    var vue = new __WEBPACK_IMPORTED_MODULE_0_vue___default.a({
        el: "#JourneyIndex",
        data: {
            columns: ["startingPlace", "actions"],
            options: {
                headings: {
                    startingPlace: 'Miejsce wsiadania',
                    actions: 'Akcje'
                },
                sortable: ["startingPlace"],
                perPage: 20
            },
            data: model
        }
    });
}

journeyIndexViewModel(window.model);

/***/ })

},[193]);