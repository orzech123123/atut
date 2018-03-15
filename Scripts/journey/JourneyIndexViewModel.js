import Vue from "vue";
import { ClientTable } from "vue-tables-2";
import moment from 'moment';

moment.locale('pl');
window.moment = moment;

Vue.use(ClientTable);

var journeyIndexViewModel = function (model) {
    var vue = new Vue({
        el: "#JourneyIndex",
        data: {
            columns: ["startingPlace", "finalPlace", "startDate", "endDate", "actions"],
            options: {
                dateColumns: ["startDate", "endDate"], 
                toMomentFormat: true,
                dateFormat: "ll",
                headings: {
                    startingPlace: 'Miejsce wsiadania',
                    finalPlace: 'Miejsce końcowe',
                    startDate: 'Data wyjazdu',
                    endDate: 'Data powrotu',
                    actions: 'Akcje'
                },
                sortable: ["startingPlace", "finalPlace", "startDate", "endDate"],
                perPage: 20
            },
            data: model
        }
    });
}

journeyIndexViewModel(window.model);