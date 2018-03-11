import Vue from "vue";
import { ClientTable } from "vue-tables-2";

Vue.use(ClientTable);

var journeyIndexViewModel = function (model) {
    var vue = new Vue({
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