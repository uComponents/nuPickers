// this is used as an abstract base controller for both the PrefetchList and TypeaheadList controllers

angular
    .module("umbraco")
    .controller("nuPickers.Shared.ListPicker.ListPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

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
                return $scope.selectedOptions.map(function (option) { return option.key; }).indexOf(option.key) >= 0;
            };

            // return true if option can be picked
            $scope.isValidSelection = function (option) {
                return $scope.isSelectable(option) && ($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems <= 0);
            };

            $scope.cursorUp = function () {
                // TODO: move highlight / active of next selectable
            };

            $scope.cursorDown = function () {
                // TODO: move highlight / active of previous selectable
            };

            $scope.enterKey = function () {
                // TODO: select highlighted
            }

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {
                if ($scope.isValidSelection(option)) {
                    $scope.selectedOptions.push(option);

                    $scope.model.value = editorResource.createSaveValue($scope.model.config, $scope.selectedOptions);

                    editorResource.updateRelationMapping($scope.model, $scope.selectedOptions);
                }
            };

            // count for dashed placeholders
            $scope.getRequiredPlaceholderCount = function () {
                var count = $scope.model.config.listPicker.minItems - $scope.selectedOptions.length;
                if (count > 0) { return new Array(count); }
                return null;
            }

            $scope.showSelectPlaceholder = function () {

                

                // TODO: and selectable options, has valid options - noly prefetch should be able to do this


                return ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems)
                        && (($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems) || $scope.model.config.listPicker.maxItems == 0);
            }

            // returns true if there are at least two different items in 'selected'
            $scope.isSortable = function () {

                if ($scope.$parent.model.config.prefetchListPicker && !$scope.$parent.model.config.prefetchListPicker.allowSorting) {
                    return false;
                }

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
                return editorResource.getEditorDataItems($scope.model.config, typeahead);
            };

            $scope.getEditorOptions().then(function (response) {

                var editorOptions = response.data; // [{"key":"","label":""},{"key":"","label":""}...]

                editorResource.getPickedKeys($scope.model).then(function (pickedKeys) {

                    // build selected options                
                    for (var i = 0; i < pickedKeys.length; i++) {
                        for (var j = 0; j < editorOptions.length; j++) {
                            if (pickedKeys[i] == editorOptions[j].key) {
                                $scope.selectedOptions.push(editorOptions[j]);
                                break;
                            }
                        }
                    }

                });
              
                // look at the config to see if it's a prefetch (so know to set initial list or not)
                if ($scope.model.config.prefetchListPicker != null) {
                    $scope.selectableOptions = editorOptions;
                }
            });
                 
            // setup watch on selected options
            $scope.$watchCollection('selectedOptions', function () {

                // set validation state
                $scope.listPickerForm.validation.$setValidity('validationMessage',
                                    ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems &&
                                    ($scope.selectedOptions.length <= $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems < 1)));

                // toggle sorting ui
                $scope.sortableConfiguration.disabled = !$scope.isSortable();
            });

            /*
            $scope.$on("formSubmitting", function () {

                $scope.model.value = editorResource.createSaveValue($scope.model.config, $scope.selectedOptions);

                editorResource.updateRelationMapping($scope.model, $scope.selectedOptions);
            });
            */

}]);