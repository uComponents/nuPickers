
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownPicker.DropDownPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {
                $scope.dropDownPickerOptions = response.data;

                var pickedKey = editorResource.getPickedKeys($scope.model.config, $scope.model.value)[0];
                if (pickedKey)
                {
                    var i = 0;
                    var found = false;
                    do
                    {
                        if ($scope.dropDownPickerOptions[i].key == pickedKey)
                        {
                            $scope.pickedOption = $scope.dropDownPickerOptions[i];
                            found = true;
                        }
                        i++;

                    } while (!found && i < $scope.dropDownPickerOptions.length)                  
                }

                $scope.$on("formSubmitting", function () {

                    $scope.model.value = editorResource.createSaveValue($scope.model.config, [$scope.pickedOption]);

                    editorResource.updateRelationMapping($scope.model.config, [$scope.pickedOption]);

                });

            });

        }]);