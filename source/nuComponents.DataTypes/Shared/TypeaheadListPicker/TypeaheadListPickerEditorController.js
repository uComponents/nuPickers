
angular
    .module('umbraco')
    .controller("nuComponents.DataTypes.Shared.TypeaheadListPicker.TypeaheadListPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        function ($scope, dataSourceResource) {

            $scope.cursorUp = function () {
                // move highlight / active of selectable to next
            };

            $scope.cursorDown = function () {
                // move highlight / active of selectable to previous
            };

            $scope.clear = function () {
                $scope.typeahead = null;
                $scope.selectableOptions = null;
            };

            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {                

                if (newValue != null && newValue.length > 0) {

                    dataSourceResource.getEditorOptions($scope.model.config, newValue).then(function (response) {

                        if (response.data.length > 0) {
                            $scope.selectableOptions = response.data;
                        }
                        else {
                            $scope.typeahead = oldValue;
                        }                                                
                        
                    });

                }
                else {
                    $scope.selectableOptions = null;
                }

            });

        }]);

