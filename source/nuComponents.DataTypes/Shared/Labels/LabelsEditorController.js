
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.Labels.LabelsEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {

                $scope.options = response.data;

            });

        }]);