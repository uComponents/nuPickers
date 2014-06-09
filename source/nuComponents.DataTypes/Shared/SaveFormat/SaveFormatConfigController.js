
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.SaveFormat.SaveFormatConfigController",
    ['$scope', function ($scope) {

        $scope.relationMappingBidirectional = false;
        
        $scope.$on('relationMappingChanged', function (event, relationType) {

            $scope.relationMappingBidirectional = (relationType != null && relationType.bidirectional);

            if ($scope.model.value == 'relationsOnly' && !$scope.relationMappingBidirectional) {
                // fallback to csv
                $scope.model.value = 'csv';
            }

        });

    }]);
