import { Alerter } from "../utils/alerter";
//
//Alerter("You've reached the homepage");

import Vue from "vue";
import { ClientTable } from "vue-tables-2";
Vue.use(ClientTable); 

var viewModel = new Vue({
    el: "#test",
    data: {
        message: "assaa",
        columns: ['id', 'name', 'age'],
        tableData: [
            { id: 1, name: "John", age: "20" },
            { id: 2, name: "Jane", age: "24" },
            { id: 3, name: "Susan", age: "16" },
            { id: 4, name: "Chris", age: "55" },
            { id: 5, name: "Dan", age: "40" }
        ],
        options: {
            // see the options API
        
        }
    }
});