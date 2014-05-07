
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownPicker.DropDownPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Picker.PickerResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.dropDownPickerOptions = response.data; // [{"key":"","markup":""},{"key":"","markup":""}...]
            });

        }]);