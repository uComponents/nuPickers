
angular
    .module("umbraco")
    .controller("nuPickers.Shared.SaveFormat.SaveFormatConfigController",
    ['$scope', function ($scope) {

        $scope.typeaheadListPicker = false; // identify if this is a typeahead list picker (remove CSV and RelationsOnly options)
        $scope.relationMappingBidirectional = false;
        
        $scope.$on('relationMappingChanged', function (event, relationType) {
            if (!$scope.typeaheadListPicker) {
                $scope.relationMappingBidirectional = (relationType != null && relationType.bidirectional);

                if ($scope.model.value == 'relationsOnly' && !$scope.relationMappingBidirectional) {
                    $scope.model.value = 'csv'; // fallback to csv
                }
            }
        });

        //// listen for a typeahead list picker identifying itself
        //$scope.$on('typeaheadListPicker', function (event, args) {

        //    $scope.typeaheadListPicker = args;
        //    $scope.model.value = 'json';
        //});

    }]);
