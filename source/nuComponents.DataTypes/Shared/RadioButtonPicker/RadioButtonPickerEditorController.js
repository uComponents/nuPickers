
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RadioButtonPicker.RadioButtonPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Picker.PickerResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.radioButtonPickerOptions = response.data;                               

                $scope.pickedKey = pickerResource.getSavedKeys($scope.model.value)[0];

                $scope.$watch('pickedKey', function () {

                    var i = 0;
                    var found = false;
                    do {
                        if ($scope.radioButtonPickerOptions[i].key == $scope.pickedKey) {

                            $scope.model.value = pickerResource.createSaveValue($scope.model.config, [$scope.radioButtonPickerOptions[i]]);
                            found = true;
                        }
                        i++;

                    } while (!found && i < $scope.radioButtonPickerOptions.length)

                    
                });

            });

        }]);