// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuPickers.Shared.PrefetchListPicker.PrefetchListPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            // get slectable options, and then build selected options
            editorResource.getEditorDataItems($scope.model).then(function (response) {

                $scope.$parent.selectableOptions = response.data.editorDataItems;

                // build selected options from picked keys (avoids a potential ajax call if the save value doesn't contain label data)
                editorResource.getPickedKeys($scope.model).then(function (pickedKeys) {

                    var selectedOptions = [];

                    for (var i = 0; i < pickedKeys.length; i++) {
                        for (var j = 0; j < $scope.selectableOptions.length; j++) {
                            if (pickedKeys[i] == $scope.selectableOptions[j].key) {
                                selectedOptions.push($scope.selectableOptions[j]);
                                break;
                            }
                        }
                    }

                    $scope.$parent.selectedOptions = selectedOptions;
                });

                // setup filtering
                if ($scope.model.config.prefetchListPicker.enableFiltering) {

                    var allSelectableOptions = $scope.selectableOptions;

                    $scope.$watch('filter', function (newValue, oldValue) {

                        // if the filter is empty then just return all items
                        if (newValue == null || newValue.length == 0) {
                            return $scope.$parent.selectableOptions = allSelectableOptions;
                        }

                        newValue = newValue.toLowerCase();
                        var filteredSelectableOptions = allSelectableOptions.filter(function (item) {
                            // strip html before searching
                            return String(item.label).replace(/(<([^>]+)>)/gm, '').toLowerCase().indexOf(newValue) != -1;
                        });

                        if (filteredSelectableOptions.length > 0) {
                            return $scope.$parent.selectableOptions = filteredSelectableOptions;
                        }
                        else {
                            $scope.filter = oldValue;
                        }

                    });

                }

            });

}]);