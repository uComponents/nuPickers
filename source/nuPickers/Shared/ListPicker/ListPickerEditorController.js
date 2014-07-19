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
                }
            };

            // count for number of dashed placeholders to render
            $scope.getRequiredPlaceholderCount = function () {
                var count = $scope.model.config.listPicker.minItems - $scope.selectedOptions.length;
                if (count > 0) { return new Array(count); }
                return null;
            }

            // returns true is the 'select' placeholder should be rendered
            $scope.showSelectPlaceholder = function () {

                // TODO: and selectable options, has valid options - only prefetch should be able to do this

                return ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems)
                        && (($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems) || $scope.model.config.listPicker.maxItems == 0);
            }

            // returns true if selected options can be sorted
            $scope.isSortable = function () {

                // if prefetch list doesn't allow sorting
                if ($scope.$parent.model.config.prefetchListPicker && !$scope.$parent.model.config.prefetchListPicker.allowSorting) { return false; }

                // if not allowing duplicates, then check selected items > 1
                if (!$scope.model.config.listPicker.allowDuplicates && $scope.selectedOptions.length > 1) { return true; }

                // duplicates allowed, so chheck there are at least 2 distinct items
                var keys = $scope.selectedOptions.map(function (option) { return option.key; }); // key all selected keys
                return keys.filter(function (value, index) { return keys.indexOf(value) == index; }).length >= 2;
            };

            // remove option from 'selected'
            $scope.deselectOption = function ($index) {
                $scope.selectedOptions.splice($index, 1);
            };

            function initSelectedOptionsWatch() {
                $scope.$watchCollection('selectedOptions', function () {

                    // set validation state
                    $scope.listPickerForm.validation.$setValidity('validationMessage',
                                        ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems &&
                                        ($scope.selectedOptions.length <= $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems < 1)));

                    // toggle sorting ui
                    $scope.sortableConfiguration.disabled = !$scope.isSortable();

                    // update model for persistance
                    $scope.model.value = editorResource.createSaveValue($scope.model.config, $scope.selectedOptions);
                });
            }

            // method here so that typeahead doesn't need a reference to dataSource
            $scope.getEditorOptions = function (typeahead) {
                return editorResource.getEditorDataItems($scope.model.config, typeahead);
            };

            // if typeahead, then build picked options directly from the save value
            if ($scope.model.config.hasOwnProperty('typeaheadListPicker')) {

                // build selected options from full stored values (as these might not be present in the selectable collection)
                $scope.selectedOptions = editorResource.getPickedItems($scope.model);
                $scope.selectedOptions = $scope.selectedOptions || [];

                initSelectedOptionsWatch(); // selected options restored, so setup watch

            } else {

                $scope.getEditorOptions().then(function (response) {

                    $scope.selectableOptions = response.data; 

                    // build selected options from picked keys (ensures label is up to date)
                    editorResource.getPickedKeys($scope.model).then(function (pickedKeys) {
                        for (var i = 0; i < pickedKeys.length; i++) {
                            for (var j = 0; j < $scope.selectableOptions.length; j++) {
                                if (pickedKeys[i] == $scope.selectableOptions[j].key) {
                                    $scope.selectedOptions.push($scope.selectableOptions[j]);
                                    break;
                                }
                            }
                        }
                    });

                    initSelectedOptionsWatch(); // selected options restored, so setup watch
                });
            }

            $scope.$on("formSubmitting", function () {
                editorResource.updateRelationMapping($scope.model, $scope.selectedOptions);
            });
}]);