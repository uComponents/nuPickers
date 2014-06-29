
angular
    .module("umbraco")
    .controller("nuPickers.Shared.XmlDataSource.XmlDataSourceConfigController",
    ['$scope', function ($scope) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'XmlDataSourceApi';

    }]);

