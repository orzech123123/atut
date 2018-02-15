import { Alerter } from "../utils/alerter";
//
Alerter("You've reached the homepage");

import Vue from 'vue'

export class XViewModel extends Vue {
    constructor() {
        super({
            el: "#test",
            data: {
                message: "assaa"
            }
        });
    }
}

var viewModel = new XViewModel();