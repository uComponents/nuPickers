
angular
    .module("umbraco")
    .controller("nuPickers.Shared.JsonDataSource.JsonDataSourceConfigController",
    ['$scope', function ($scope) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'JsonDataSourceApi';

    }]);

