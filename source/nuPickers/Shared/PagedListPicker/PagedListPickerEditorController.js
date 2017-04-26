angular
    .module("umbraco")
    .controller("nuPickers.Shared.PagedListPicker.PagedListPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {
            
            // get slectable options
            editorResource.getEditorDataItems($scope.model).then(function (response) {
                $scope.setSelectableOptions(response.data);
            });

            // get selected options
            editorResource.getPickedEditorDataItems($scope.model).then(function (editorDataItems) {
                $scope.setSelectedOptions(editorDataItems);
            });

        }]);