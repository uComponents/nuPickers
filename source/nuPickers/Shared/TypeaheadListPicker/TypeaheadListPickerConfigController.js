
angular
    .module("umbraco")
    .controller("nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerConfigController",
    ['$scope', 'nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerConfigState', function ($scope, typeaheadListPickerConfigState) {

        typeaheadListPickerConfigState.isTypeaheadListPicker = true;

    }]);
