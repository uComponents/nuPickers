// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.PrefetchListPicker.PrefetchListPickerEditorController",
        ['$scope',
        function ($scope) {

            var allSelectableOptions = [];
            
            $scope.$watchCollection('selectableOptions', function () {
                allSelectableOptions = $scope.selectableOptions;
            });


            // setup filtering
            if ($scope.model.config.prefetchListPicker.enableFiltering) {

                $scope.$watch('filter', function (newValue, oldValue) {

                    // if the filter is empty then just return all items
                    if (newValue == null || newValue.length == 0) {
                        return $scope.selectableOptions = $scope.allSelectableOptions;
                    }

                    newValue = newValue.toLowerCase();
                    var filteredSelectableOptions = $scope.allSelectableOptions.filter(function (item) {
                        // strip html before searching
                        return String(item.label).replace(/(<([^>]+)>)/gm, '').toLowerCase().indexOf(newValue) != -1;
                    });

                    if (filteredSelectableOptions.length > 0) {
                        return $scope.selectableOptions = filteredSelectableOptions;
                    }
                    else {
                        $scope.filter = oldValue;
                    }

                });
            }

}]);