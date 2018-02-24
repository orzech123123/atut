import Vue from "vue";
import { ClientTable } from "vue-tables-2";

Vue.use(ClientTable);

var vehicleIndexViewModel = function (model) {
    var vue = new Vue({
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
                perPage: 20,
//                templates: {
//                    actions: "akcje"
//                }
            },
            data: model
        }
    });
}

vehicleIndexViewModel(window.model);