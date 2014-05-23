
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.TypeaheadPicker.TypeaheadPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {


            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {                

                dataSourceResource.getEditorOptions($scope.model.config, $scope.typeahead).then(function (response) {

                    $scope.editorOptions = response.data;

                });


            });




        }]);