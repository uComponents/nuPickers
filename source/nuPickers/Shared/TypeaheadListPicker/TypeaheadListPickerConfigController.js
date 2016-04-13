
angular
    .module("umbraco")
    .controller("nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerConfigController",
    ['$rootScope', '$scope', function ($rootScope, $scope) {

        // when save format is ready, tell it this picker is a typeahead
        // this is currently required as this is executed before the save format controller
        $scope.$on('saveFormatListening', function (event, arg) {
            $rootScope.$broadcast('isTypeaheadListPicker', true);
        });

    }]);
