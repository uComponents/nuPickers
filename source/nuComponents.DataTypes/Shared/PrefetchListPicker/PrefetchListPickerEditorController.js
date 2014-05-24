// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.PrefetchListPicker.PrefetchListPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {

            // array of option objects, for the selectable list 
            $scope.selectableOptions = []; // [{"key":"","label":""}...]

            // array of option objects, for the selected list
            $scope.selectedOptions = []; // [{"key":"","label":""}...]

            // http://api.jqueryui.com/sortable/
            $scope.sortableConfiguration = { axis: 'y' };

            // returns true if option hans't yet been picked, or duplicates are allowed
            $scope.isSelectable = function (option) {
                return ($scope.model.config.listPicker.allowDuplicates || !$scope.isUsed(option));
            };

            // returns true is this option has been picked
            $scope.isUsed = function (option) {
                return $scope.selectedOptions.indexOf(option) >= 0;
            };

            // return true is option can be picked
            $scope.isValidSelection = function (option) {
                return $scope.isSelectable(option) && ($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems <= 0);
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {

                // if item can be selected and not exceeding the max
                if ($scope.isValidSelection(option)) {
                    $scope.selectedOptions.push(option);
                }
            };

            // count for dashed placeholders
            $scope.getRequiredPlaceholderCount = function () {
                var count = $scope.model.config.listPicker.minItems - $scope.selectedOptions.length;
                if (count > 0) { return new Array(count); }
                return null;
            }

            $scope.showSelectPlaceholder = function () {

                return ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems)
                        && (($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems) || $scope.model.config.listPicker.maxItems == 0);
                // TODO: and selectable options, has valid options
            }
            
            $scope.isSortable = function () {

                // return true if there is are at least two different items selected
                if (!$scope.model.config.allowDuplicates && $scope.selectedOptions > 1) {
                    return true;
                }

                // else check collection contains at least two different items
                var keys = $scope.selectedOptions.map(function (option) { return option.key; }); // key all selected keys

                return keys.filter(function (value, index) { return keys.indexOf(value) == index; }).length >= 2;
            };

            // remove option from 'selected'
            $scope.deselectOption = function ($index) {
                $scope.selectedOptions.splice($index, 1);
            };

            // call api, supplying all configuration details, and expect a collection of options (key / label) to be populated
            dataSourceResource.getEditorOptions($scope.model.config).then(function (response) {

                var editorOptions = response.data; // [{"key":"","label":""},{"key":"","label":""}...]

                // build selected options
                var savedKeys = dataSourceResource.getSavedKeys($scope.model.value);
                for (var i = 0; i < savedKeys.length; i++) { // loop though each saved key
                    for (var j = 0; j < editorOptions.length; j++) { // loop though each editor option
                        if (savedKeys[i] == editorOptions[j].key) {
                            $scope.selectedOptions.push(editorOptions[j]);
                            break; // break out of the editor option loop
                        }
                    }
                }

                // setup watch on selected options
                $scope.$watchCollection('selectedOptions', function () {

                    // use the picker resourse to save in the correct format
                    $scope.model.value = dataSourceResource.createSaveValue($scope.model.config, $scope.selectedOptions);

                    // TODO: how to return error to Umbraco ?
                    //var isValid = ($scope.selectableOptions.length >= $scope.model.config.minItems
                    //               && ($scope.selectableOptions.length <= $scope.model.config.maxItems || $scope.model.config.maxItems < 1));

                    // toggle sorting ui
                    $scope.sortableConfiguration.disabled = !$scope.isSortable();
                });

                // build selectable options
                $scope.selectableOptions = editorOptions;

                // setup filtering
                if ($scope.model.config.prefetchListPicker.enableFiltering) {
                    $scope.allSelectableOptions = $scope.selectableOptions;

                    $scope.$watch('model.filterQuery', function (newValue, oldValue) {

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
                            // reset value - TODO: flash UI ?
                            $scope.model.filterQuery = oldValue;
                        }

                    });
                }
            });

        }]);