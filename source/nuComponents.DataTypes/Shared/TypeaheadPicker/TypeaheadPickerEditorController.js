
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.TypeaheadPicker.TypeaheadPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {


            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {                

                if (newValue.length > 0) {

                    dataSourceResource.getEditorOptions($scope.model.config, $scope.typeahead).then(function (response) {

                        if (response.data.length > 0) {
                            $scope.editorOptions = response.data;
                        }
                        else {
                            $scope.typeahead = oldValue;
                        }                                                
                        
                    });

                }
                else {
                    $scope.editorOptions = null;
                }

            });




        }]);