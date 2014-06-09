
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMatches.RelationMatchesEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {

                $scope.relationMatches = response.data;

            });

        }]);