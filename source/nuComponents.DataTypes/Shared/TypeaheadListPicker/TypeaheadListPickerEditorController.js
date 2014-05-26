
angular
    .module('umbraco')
    .controller("nuComponents.DataTypes.Shared.TypeaheadListPicker.TypeaheadListPickerEditorController",
        ['$scope', 
        function ($scope) {

            $scope.cursorUp = function () {
                // move highlight / active of selectable to next
            };

            $scope.cursorDown = function () {
                // move highlight / active of selectable to previous
            };

            //$scope.clear = function () {
            //    $scope.typeahead = null;
            //    $scope.selectableOptions = null;
            //};

            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {                

                if (newValue != null && newValue.length > 0) { // TODO: check length meets min typeahead length specified in config
                    
                    $scope.getEditorOptions(newValue).then(function (response) {

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

            if ($scope.model.config.typeaheadListPicker.limitTo > 0) {
                $scope.$watchCollection('selectableOptions', function () {                    
                    $scope.selectableOptions = $scope.selectableOptions.slice(0, $scope.model.config.typeaheadListPicker.limitTo);
                });
            }

}]);

