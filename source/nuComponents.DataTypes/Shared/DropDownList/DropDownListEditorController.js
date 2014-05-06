
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownList.DropDownListEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Core.PickerResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.dropDownListOptions = response.data; // [{"key":"","markup":""},{"key":"","markup":""}...]
            });

        }]);