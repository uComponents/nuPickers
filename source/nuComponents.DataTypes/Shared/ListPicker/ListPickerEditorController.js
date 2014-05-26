// this is used as an abstract base controller for both the PrefetchList and TypeaheadList controllers

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.ListPicker.ListPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {

            // array of option objects, for the selectable list 
            $scope.selectableOptions = []; // [{"key":"","label":""}...]

            // array of option objects, for the selected list
            $scope.selectedOptions = []; // [{"key":"","label":""}...]

            // http://api.jqueryui.com/sortable/
            $scope.sortableConfiguration = { axis: 'y' };

            // returns true if option hasn't yet been picked, or duplicates are allowed
            $scope.isSelectable = function (option) {
                return ($scope.model.config.listPicker.allowDuplicates || !$scope.isUsed(option));
            };

            // returns true is this option has been picked
            $scope.isUsed = function (option) {
                return $scope.selectedOptions.map(function (option) { return option.key; }).indexOf(option.key) > 0;
            };

            // return true if option can be picked
            $scope.isValidSelection = function (option) {
                return $scope.isSelectable(option) && ($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems <= 0);
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {
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

            // returns true if there are at least two different items in 'selected'
            $scope.isSortable = function () {

                if (!$scope.model.config.allowDuplicates && $scope.selectedOptions > 1) {
                    return true;
                }

                var keys = $scope.selectedOptions.map(function (option) { return option.key; }); // key all selected keys
                return keys.filter(function (value, index) { return keys.indexOf(value) == index; }).length >= 2;
            };

            // remove option from 'selected'
            $scope.deselectOption = function ($index) {
                $scope.selectedOptions.splice($index, 1);
            };

            // method here so that typeahead doesn't need a reference to dataSource
            $scope.getEditorOptions = function (typeahead) {
                return dataSourceResource.getEditorOptions($scope.model.config, typeahead);
            };

            var savedKeys = dataSourceResource.getSavedKeys($scope.model.value); // if set within promise callback function below, this is empty (as value is a primitive type)
            $scope.getEditorOptions().then(function (response) {

                var editorOptions = response.data; // [{"key":"","label":""},{"key":"","label":""}...]

                // build selected options                
                for (var i = 0; i < savedKeys.length; i++) { 
                    for (var j = 0; j < editorOptions.length; j++) { 
                        if (savedKeys[i] == editorOptions[j].key) {
                            $scope.selectedOptions.push(editorOptions[j]);
                            break;
                        }
                    }
                }

                // look at the config to see if it's a prefetch (so know to set initial list or not)
                if ($scope.model.config.prefetchListPicker != null) {
                    $scope.selectableOptions = editorOptions;
                }
            });
                 
            // setup watch on selected options
            $scope.$watchCollection('selectedOptions', function () {

                // use the picker resourse to save in the correct format
                $scope.model.value = dataSourceResource.createSaveValue($scope.model.config, $scope.selectedOptions);

                // set validation state
                $scope.listPickerForm.validation.$setValidity('validationMessage',
                                    ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems &&
                                    ($scope.selectedOptions.length <= $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems < 1)));

                // toggle sorting ui
                $scope.sortableConfiguration.disabled = !$scope.isSortable();
            });

}]);