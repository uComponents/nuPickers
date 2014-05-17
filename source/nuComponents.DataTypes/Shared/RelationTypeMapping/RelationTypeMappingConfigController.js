
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationTypeMapping.RelationTypeMappingConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $http.get('backoffice/nuComponents/RelationTypeMappingApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;
        });

    }]);
