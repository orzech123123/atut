import Vue from "vue";
import VeeValidate, { Validator }  from 'vee-validate';
import VeeValidatePolish from 'vee-validate/dist/locale/pl';
import Datepicker from 'vuejs-datepicker';
import moment from 'moment';
import Multiselect from 'vue-multiselect';
import { ClientTable } from "vue-tables-2";

Vue.use(ClientTable);

Validator.localize('pl', VeeValidatePolish);
Vue.use(VeeValidate);

Vue.component('countries-editor', {
    props: {
        items: {
            type: Array,
            required: true
        },  
    },
    template: '#countries-editor-template',
    data() {
        return {
            showError: false,
            countryNames: ["Francja", "Belgia", "Hiszpania"],
            nameToAdd: null,
            distanceToAdd: 0,

            columns: ["name", "distance", "actions"],
            options: {
                headings: {
                    name: 'Nazwa',
                    distance: 'Dystans [km]',
                    actions: 'Akcje'
                },
                sortable: ["name", "distance"],
                perPage: 20
            }
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
                this.showError = true;
                return;
            }

            this.items.push({ name: this.nameToAdd, distance: this.distanceToAdd });
            this.nameToAdd = null;
            this.distanceToAdd = 0;
            this.showError = false;
        },
        remove: function (name) {
            var item = this.items.filter(i => i.name == name)[0];
            this.items.splice(this.items.indexOf(item), 1);
        }
    }
});

var JourneyEditViewModel = function (model) {
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
            },
            recalculateOtherCountriesTotalDistance: function (totalDistance, countriesTotalDistance) {
                this.otherCountriesTotalDistance = totalDistance - countriesTotalDistance;
            }
        },
        mounted: function () {
            var $startDate = $(this.$refs.startDate.$el.children[0]).find("input");
            var $endDate = $(this.$refs.endDate.$el.children[0]).find("input");
            $startDate.addClass("form-control");
            $endDate.addClass("form-control");
        },
        components: {
            Datepicker,
            Multiselect
        },
        computed: {
            startDateDisplayModel: function () {
                return !!this.startDate ? moment(this.startDate).format('YYYY-MM-DD') : null;
            },
            endDateDisplayModel: function () {
                return !!this.endDate ? moment(this.endDate).format('YYYY-MM-DD') : null;
            },
            countriesTotalDistance: function () {
                return this.countries.length > 1 ? this.countries.map(item => item.distance).reduce((prev, next) => Number(prev) + Number(next)) : this.countries.length > 0 ? this.countries[0].distance : 0;
            }
        },
        watch: {
            totalDistance: function (val) {
                this.recalculateOtherCountriesTotalDistance(val, this.countriesTotalDistance);
            },
            countriesTotalDistance: function (val) {
                this.recalculateOtherCountriesTotalDistance(this.totalDistance, val);
            }
        }
    });
}

JourneyEditViewModel(window.model);