angular
    .module("umbraco")
    .controller("nuPickers.Shared.TreePicker.TreePickerEditorController",
    ['$scope', 'nuPickers.Shared.Editor.EditorResource', 'dialogService',
        function ($scope, editorResource, dialogService) {

            $scope.selectedItems = [];
            if ($scope.model.config.treePicker == null) { $scope.model.config.treePicker = {} };

            // populate the rendermodel from saved data
            if ($scope.model.value) {
                _.each($scope.model.value, function(item) {
                    $scope.selectedItems.push(item);
                });
            }

            $scope.openTreePicker = function() {
                var options = {
                    // TODO: Determine path properly
                    template: "/App_Plugins/nuPickers/Shared/TreePicker/TreePickerEditorDialog.html",
                    dialogData: $scope.model,
                    callback: function(data) {

                        if (angular.isArray(data)) {
                            _.each(data, function(item, i) {
                                $scope.add(item);
                            });
                        } else {
                            $scope.clear();
                            $scope.add(data);
                        }
                    }
                };

                dialogService.open(options);
            };

            $scope.remove = function(index) {
                $scope.selectedItems.splice(index, 1);
            };

            $scope.clear = function() {
                $scope.selectedItems = [];
            };

            $scope.add = function(item) {
                var currKeys = _.map($scope.selectedItems, function(i) { return i.key; });
                if (currKeys.indexOf(item.key) < 0) {
                    $scope.selectedItems.push(item);
                }
            };

            $scope.$watchCollection('selectedItems', function(newVal, oldVal) {
                if (newVal !== oldVal) {
                    $scope.model.value = editorResource.createSaveValue($scope.model.config, newVal);
                }
            });

        }
    ]);