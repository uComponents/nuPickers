
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.RelationMatches.RelationMatchesEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Picker.PickerResource',
        function ($scope, pickerResource) {

            
            // repurposing the pickerResource pattern TODO: refactor this method out to a dataSourceResource
            pickerResource.getEditorOptions($scope.model.config).then(function (response) {

                $scope.relationMatches = response.data;

            });

        }]);