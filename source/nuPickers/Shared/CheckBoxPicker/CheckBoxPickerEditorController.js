
angular
    .module("umbraco")
    .controller("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model).then(function (response) {

                $scope.checkBoxPickerOptions = response.data; 
                
                // restore any saved values
                editorResource.getPickedKeys($scope.model).then(function (pickedKeys) {
                    for (var i = 0; i < pickedKeys.length; i++) { 
                        for (var j = 0; j < $scope.checkBoxPickerOptions.length; j++) { 
                            if (pickedKeys[i] == $scope.checkBoxPickerOptions[j].key) {
                                $scope.checkBoxPickerOptions[j].isChecked = true;
                                break;
                            }
                        }
                    }
                });
               
                $scope.checkBoxChange = function () {
                    $scope.model.value = editorResource.createSaveValue($scope.model.config, $scope.getPickedOptions());
                }

                $scope.$on("formSubmitting", function () {
                    editorResource.updateRelationMapping($scope.model, $scope.getPickedOptions());
                });

                $scope.getPickedOptions = function () {
                    return $scope.checkBoxPickerOptions.filter(function (option) { return option.isChecked == true; });
                };

            });

        }]);