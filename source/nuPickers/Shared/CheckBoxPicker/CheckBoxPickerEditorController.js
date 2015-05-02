
angular
    .module("umbraco")
    .controller("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            $scope.checkAllState = false;
            $scope.checkBoxPickerOptions = [];

            $scope.checkAllClick = function () {

                $scope.checkAllState = !$scope.checkAllState;

                angular.forEach($scope.checkBoxPickerOptions, function (option) {
                    option.isChecked = $scope.checkAllState;
                });

                generateSaveValue();
            };

            $scope.checkBoxChange = function () {

                $scope.checkAllState = areAllChecked();

                generateSaveValue();
            };

            $scope.getPickedOptions = function () {
                return $scope.checkBoxPickerOptions.filter(function (option) { return option.isChecked == true; });
            };

            function areAllChecked() {
                return $scope.getPickedOptions().length == $scope.checkBoxPickerOptions.length;
            };

            function generateSaveValue () {
                $scope.model.value = editorResource.createSaveValue($scope.model.config, $scope.getPickedOptions());
            };

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

                    $scope.checkAllState = areAllChecked();
                });

            });

        }]);