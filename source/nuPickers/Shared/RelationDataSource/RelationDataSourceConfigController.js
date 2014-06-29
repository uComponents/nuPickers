
angular
    .module("umbraco")
    .controller("nuPickers.Shared.RelationDataSource.RelationDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {
                
        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'RelationDataSourceApi';

        $http.get('backoffice/nuPickers/RelationDataSourceApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;
        });

    }]);
