
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SaveFormat.SaveFormatConfigController",
    ['$scope', 'nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerConfigState', function ($scope, typeaheadListPickerConfigState) {
        
        $scope.isTypeaheadListPicker = false;

        if (typeaheadListPickerConfigState.isTypeaheadListPicker)
        {
            $scope.isTypeaheadListPicker = true; // (remove CSV and RelationsOnly options)
            typeaheadListPickerConfigState.isTypeaheadListPicker = false; // reset
        }
        
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
