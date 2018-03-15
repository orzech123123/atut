import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import Datepicker from 'vuejs-datepicker';

Validator.localize('pl', VeeValidatePolish);
Vue.use(VeeValidate);

var journeyEditViewModel = function (model) {
    var vue = new Vue({
        el: "#JourneyEdit",
        data: model,
        methods: {
            validateBeforeSubmit() {
                this.$validator.validateAll().then((result) => {
                    if (result)
                    {
                        this.$refs.form.submit();
                    }
                });
            }
        },
        mounted: function () {
            var $endDate = $(this.$refs.endDate.$el.children[0]).find("input");
            $endDate.addClass("form-control");
            $endDate.removeAttr("readonly");
        },
        components: {
            Datepicker
        }
    });
}

journeyEditViewModel(window.model);