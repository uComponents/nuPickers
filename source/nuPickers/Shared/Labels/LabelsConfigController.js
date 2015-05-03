
angular
    .module("umbraco")
    .controller("nuPickers.Shared.Labels.LabelsConfigController",
    ['$rootScope', '$scope', function ($rootScope, $scope) {

        // if labels, then data sources don't need to ask for key data
        $rootScope.$broadcast('isLabels', true);

    }]);
