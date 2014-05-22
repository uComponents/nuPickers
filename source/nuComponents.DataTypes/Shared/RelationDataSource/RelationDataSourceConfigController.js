
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationDataSource.RelationDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {
                
        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'RelationDataSourceApi';

        $http.get('backoffice/nuComponents/RelationDataSourceApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;
        });

    }]);
