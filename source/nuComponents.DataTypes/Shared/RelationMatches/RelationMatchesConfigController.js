
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMatches.RelationMatchesConfigController",
    ['$scope', '$http', function ($scope, $http) {
        
        $http.get('backoffice/nuComponents/RelationMatchesApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;
        });

    }]);
