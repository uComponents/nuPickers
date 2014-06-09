
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RadioButtonPicker.RadioButtonPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {
                $scope.radioButtonPickerOptions = response.data;                               

                $scope.pickedKey = editorResource.getPickedKeys($scope.model.config, $scope.model.value)[0];

                $scope.$on("formSubmitting", function () {

                    var i = 0;
                    var found = false;
                    var pickedOption = null;
                    do {
                        if ($scope.radioButtonPickerOptions[i].key == $scope.pickedKey) {

                            pickedOption = $scope.radioButtonPickerOptions[i];
                            found = true;
                        }
                        i++;

                    } while (!found && i < $scope.radioButtonPickerOptions.length)


                    $scope.model.value = editorResource.createSaveValue($scope.model.config, [pickedOption]);

                    editorResource.updateRelationMapping($scope.model.config, [pickedOption]);

                });

            });

        }]);