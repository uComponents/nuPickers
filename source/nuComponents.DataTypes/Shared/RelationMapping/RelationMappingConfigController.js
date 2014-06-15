
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMapping.RelationMappingConfigController",
    ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {

        $http.get('backoffice/nuComponents/RelationMappingApi/GetRelationTypes').then(function (response) {
            $scope.relationTypes = response.data;

            // restore any saved value
            for (var i = 0; i < $scope.relationTypes.length; i++) {
                if ($scope.relationTypes[i].key == $scope.model.value.relationTypeAlias) {
                    $scope.selectedRelationType = $scope.relationTypes[i];
                    break;
                }
            }

            $scope.relationTypeChanged();
        });

        $scope.relationTypeChanged = function () {

            // trigger event for SaveFormat controller to recieve
            $rootScope.$broadcast('relationMappingChanged', $scope.selectedRelationType);
        }

        $scope.$on("formSubmitting", function () {

            if ($scope.selectedRelationType != null) {

                // rebuild the model.value
                $scope.model.value.relationTypeAlias = $scope.selectedRelationType.key; // only need to save the key (it's alias)

            } else {
                // clear all values, as no relation type set
                $scope.model.value = null;
            }

        });

    }]);
