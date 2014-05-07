
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.SqlDataSource.SqlDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.GetResultCount = function () {

            // execute the sql expression using the connection string to return a count

        };


        $http.get('backoffice/nuComponentsDataTypesShared/SqlDataSourceApi/GetConnectionStrings').then(function (response) {
            $scope.connectionStrings = response.data;
        });


    }]);

