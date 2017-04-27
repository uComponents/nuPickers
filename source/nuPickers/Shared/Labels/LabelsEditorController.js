
angular
    .module("umbraco")
    .controller("nuPickers.Shared.Labels.LabelsEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model).then(function (response) {

                $scope.options = response.data.editorDataItems;

            });

        }]);