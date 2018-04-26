import Vue from "vue";
import { ClientTable } from "vue-tables-2";
import moment from 'moment';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);

var journeyIndexViewModel = function (model) {
    var columns = ["vehiclesColumn", "startingPlace", "finalPlace", "startDate", "endDate", "actions"];

    if (!!model.isAdmin) {
        columns.splice(0, 0, "companyNameShort");
    }

    var vue = new Vue({
        el: "#JourneyIndex",
        data: {
            columns: columns,
            options: {
                dateColumns: ["startDate", "endDate"], 
                toMomentFormat: true,
                dateFormat: "ll",
                headings: {
                    companyNameShort: "Firma",
                    vehiclesColumn: 'Pojazdy',
                    startingPlace: 'Miejscowość wsiadania',
                    finalPlace: 'Miejscowość docelowa + kraj',
                    startDate: 'Data wyjazdu',
                    endDate: 'Data powrotu',
                    actions: 'Akcje'
                },
                sortable: ["companyNameShort", "startingPlace", "finalPlace", "startDate", "endDate"],
                perPage: 20
            },
            data: model
        }
    });
}

journeyIndexViewModel(window.model);