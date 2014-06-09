
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownPicker.DropDownPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {
                $scope.dropDownPickerOptions = response.data;

                var savedKey = editorResource.getPickedKeys($scope.model.config, $scope.model.value);
                if (savedKey[0])
                {
                    var i = 0;
                    var found = false;
                    do
                    {
                        if ($scope.dropDownPickerOptions[i].key == savedKey)
                        {
                            $scope.pickedOption = $scope.dropDownPickerOptions[i];
                            found = true;
                        }
                        i++;

                    } while (!found && i < $scope.dropDownPickerOptions.length)                  
                }

                $scope.$watch('pickedOption', function () {
                    if ($scope.pickedOption == null) {
                        $scope.model.value = null;
                    } else {
                        $scope.model.value = editorResource.createSaveValue($scope.model.config, [$scope.pickedOption]);
                    }
                });
            });

        }]);