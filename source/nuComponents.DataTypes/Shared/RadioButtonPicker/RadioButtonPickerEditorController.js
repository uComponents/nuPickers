
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RadioButtonPicker.RadioButtonPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Picker.PickerResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.radioButtonPickerOptions = response.data;
            });

        }]);