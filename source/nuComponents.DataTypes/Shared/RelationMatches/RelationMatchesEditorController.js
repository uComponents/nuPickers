
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMatches.RelationMatchesEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {

            dataSourceResource.getEditorDataItems($scope.model.config).then(function (response) {

                $scope.relationMatches = response.data;

            });

        }]);