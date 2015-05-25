angular
    .module("umbraco")
    .controller("nuPickers.Shared.TreePicker.TreePickerEditorController",
    ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            // call editorResource.getEditorDataItems($scope.model, { 'parentKey':'' }) 
            // if parentKey isn't set, or is null, then expect root items to be returned

        }
    ]);