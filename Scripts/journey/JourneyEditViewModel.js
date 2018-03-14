import Vue from "vue";
import VeeValidate from 'vee-validate';

Vue.use(VeeValidate);

var journeyEditViewModel = function (model) {
    var vue = new Vue({
        el: "#JourneyEdit",
        data: model,
        methods: {
            validateBeforeSubmit() {
                this.$validator.validateAll().then((result) => {
                    console.log(this.errors);
                    console.log(this.errors.items.length);

                    if (result) {
                    // eslint-disable-next-line
                    alert('Form Submitted!');
                    return;
                    }

                    alert('Correct them errors!');

                });
            }
        }
    });
}

journeyEditViewModel(window.model);