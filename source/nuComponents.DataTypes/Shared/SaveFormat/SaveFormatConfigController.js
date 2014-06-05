
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.SaveFormat.SaveFormatConfigController",
    ['$scope', function ($scope) {

        $scope.relationTypeMappingBidirectional = false;
        
        $scope.$on('relationTypeMappingChanged', function (event, relationType) {

            $scope.relationTypeMappingBidirectional = (relationType != null && relationType.bidirectional);

            if ($scope.model.value == 'relationsOnly' && !$scope.relationTypeMappingBidirectional) {
                // fallback to csv
                $scope.model.value = 'csv';
            }

        });

    }]);
