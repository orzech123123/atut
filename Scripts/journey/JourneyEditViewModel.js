import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import Datepicker from 'vuejs-datepicker';
import moment from 'moment';

Validator.localize('pl', VeeValidatePolish);
Vue.use(VeeValidate);

Vue.component('items-editor', {
    props: {
        items: {
            type: Array,
            required: true
        },  
    },
    template: '#items-editor-template',
    data() {
        return {
            countryNames: ["Francja", "Belgia", "Hiszpania"],
            nameToAdd: null,
            distanceToAdd: 0
        }
    },
    computed: {
        availableCountryNames : function() {
            return this.countryNames.filter(name => this.items.map(item => item.name).indexOf(name) < 0);
        }
    },
    methods: {
        add: function () {
            if (!this.nameToAdd || this.distanceToAdd <= 0) {
                alert("Uzupełnij dane");
                return;
            }

            this.items.push({ name: this.nameToAdd, distance: this.distanceToAdd });
            this.nameToAdd = null;
            this.distanceToAdd = 0;
        },
        remove: function(index) {
            this.items.splice(index, 1);
        }
    }
});


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
                return !!this.endDate ? moment(this.endDate).format('YYYY-MM-DD') : null;
            }
        }
    });
}

journeyEditViewModel(window.model);