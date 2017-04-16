
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SaveFormat.SaveFormatConfigController",
    ['$rootScope', '$scope', function ($rootScope, $scope) {

        $scope.relationMappingBidirectional = false;
        
        $scope.model.value = $scope.model.value || 'csv';

        $scope.$on('relationMappingChanged', function (event, relationType) {
            $scope.relationMappingBidirectional = (relationType != null && relationType.bidirectional);

            if ($scope.model.value == 'relationsOnly' && !$scope.relationMappingBidirectional) {
                $scope.model.value = 'csv'; // fallback to csv
            }
        });



    }]);
