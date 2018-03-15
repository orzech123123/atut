import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import Datepicker from 'vuejs-datepicker';
import moment from 'moment';

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
            var $startDate = $(this.$refs.startDate.$el.children[0]).find("input");
            var $endDate = $(this.$refs.endDate.$el.children[0]).find("input");
            $startDate.addClass("form-control");
            $endDate.addClass("form-control");
        },
        components: {
            Datepicker
        },
        computed: {
            startDateDisplayModel: function () {
                return !!this.startDate ? moment(this.startDate).format('YYYY-MM-DD') : null;
            },
            endDateDisplayModel: function () {
                return this.endDate ? moment(this.endDate).format('YYYY-MM-DD') : null;
            }
        }
    });
}

journeyEditViewModel(window.model);