
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownPicker.DropDownPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.dropDownPickerOptions = response.data;

                var savedKey = pickerResource.getSavedKeys($scope.model.value);
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
                        $scope.model.value = pickerResource.createSaveValue($scope.model.config, [$scope.pickedOption]);
                    }
                });
            });

        }]);