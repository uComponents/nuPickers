
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMapping.RelationMappingConfigController",
    ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {

        $http.get('backoffice/nuComponents/RelationMappingApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;

            // restore any saved value
            for (var i = 0; i < $scope.relationTypes.length; i++) {
                if ($scope.relationTypes[i].key == $scope.model.value) {
                    $scope.selectedRelationType = $scope.relationTypes[i];
                    break;
                }
            }

            $scope.relationTypeChanged();
        });

        $scope.relationTypeChanged = function () {

            // update the value to be saved (only need to save key)
            if ($scope.selectedRelationType != null) {                
                $scope.model.value = $scope.selectedRelationType.key;
            } else {
                $scope.model.value = null;
            }            

            // trigger event for SaveFormat controller to recieve
            $rootScope.$broadcast('relationMappingChanged', $scope.selectedRelationType);
        }

    }]);
