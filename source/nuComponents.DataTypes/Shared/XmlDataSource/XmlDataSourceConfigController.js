
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.XmlDataSource.XmlDataSourceConfigController",
    ['$scope', function ($scope) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'XmlDataSourceApi';

    }]);

