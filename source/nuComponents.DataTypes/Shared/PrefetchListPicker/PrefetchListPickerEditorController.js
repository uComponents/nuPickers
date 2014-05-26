// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.PrefetchListPicker.PrefetchListPickerEditorController",
        ['$scope',
        function ($scope) {

            // setup filtering
            if ($scope.model.config.prefetchListPicker.enableFiltering) {

                // re-get all options so that we have a reference to restore them
                $scope.getEditorOptions().then(function (response) {

                    var allSelectableOptions = response.data;

                    $scope.$watch('filter', function (newValue, oldValue) {

                        // if the filter is empty then just return all items
                        if (newValue == null || newValue.length == 0) {
                            return $scope.selectableOptions = allSelectableOptions;
                        }

                        newValue = newValue.toLowerCase();
                        var filteredSelectableOptions = allSelectableOptions.filter(function (item) {
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

                });
            }

}]);