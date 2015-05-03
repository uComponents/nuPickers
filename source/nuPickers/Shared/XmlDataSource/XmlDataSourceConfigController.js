
angular
    .module("umbraco")
    .controller("nuPickers.Shared.XmlDataSource.XmlDataSourceConfigController",
    ['$scope', function ($scope) {

        // hide the key option if this is being used by labels
        $scope.isLabels = false;
        $scope.$on('isLabels', function (event, arg) { $scope.isLabels = arg; });

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'XmlDataSourceApi';


    }]);

