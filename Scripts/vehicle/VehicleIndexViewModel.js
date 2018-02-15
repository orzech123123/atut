import Vue from "vue";
import { ClientTable } from "vue-tables-2";

Vue.use(ClientTable);

var vehicleIndexViewModel = function (model) {
    var vue = new Vue({
        el: "#VehicleIndex",
        data: {
            message: "assaa",
            columns:
            ["name", "registrationNumber"
//                {
//                    name: "name",
//                    title: "Nazwa"
//                },
//                {
//                    name: "registrationNumber",
//                    title: "Numer rejestracyjny"
//                }
            ],
            options: {
                headings: {
                    name: 'Nazwa',
                    registrationNumber: 'Numer rejestracyjny'
                },
                perPage: 20
            },
            data: model
        }
    });
}

vehicleIndexViewModel(window.model);