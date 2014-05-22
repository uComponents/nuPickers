
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.SqlDataSource.SqlDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'SqlDataSourceApi';

        $http.get('backoffice/nuComponents/SqlDataSourceApi/GetConnectionStrings').then(function (response) {
            $scope.connectionStrings = response.data;
        });

    }]);

