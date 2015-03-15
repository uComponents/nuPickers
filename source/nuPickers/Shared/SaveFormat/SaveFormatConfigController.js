
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SaveFormat.SaveFormatConfigController",
    ['$scope', 'nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerConfigState', function ($scope, typeaheadListPickerConfigState) {
        
        $scope.relationMappingBidirectional = false;
        
            $scope.model.value = $scope.model.value || 'csv';

            $scope.$on('relationMappingChanged', function (event, relationType) {
                $scope.relationMappingBidirectional = (relationType != null && relationType.bidirectional);

                if ($scope.model.value == 'relationsOnly' && !$scope.relationMappingBidirectional) {
                    $scope.model.value = 'csv'; // fallback to csv
                }
            });

    }]);
