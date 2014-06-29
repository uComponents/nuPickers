
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SqlDataSource.SqlDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.connectionString = $scope.model.value.connectionString || 'umbracoDbDSN';
        $scope.model.value.apiController = 'SqlDataSourceApi';

        $http.get('backoffice/nuPickers/SqlDataSourceApi/GetConnectionStrings').then(function (response) {
            $scope.connectionStrings = response.data;
        });

    }]);

