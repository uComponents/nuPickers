
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SaveFormat.SaveFormatConfigController",
    ['$rootScope', '$scope', function ($rootScope, $scope) {
        
        $scope.isTypeaheadListPicker = false;
        //$scope.$on('isTypeaheadListPicker', function (event, arg) { $scope.isTypeaheadListPicker = arg; });
        $rootScope.$broadcast('saveFormatListening');

        $scope.relationMappingBidirectional = false;
        
        if (!$scope.isTypeaheadListPicker) {

            $scope.model.value = $scope.model.value || 'csv';

            $scope.$on('relationMappingChanged', function (event, relationType) {
                $scope.relationMappingBidirectional = (relationType != null && relationType.bidirectional);

                if ($scope.model.value == 'relationsOnly' && !$scope.relationMappingBidirectional) {
                    $scope.model.value = 'csv'; // fallback to csv
                }
            });

        }
        else { // it's a typeahead so only allow formats that save both key and label
            $scope.model.value = $scope.model.value || 'json';
        }


    }]);
