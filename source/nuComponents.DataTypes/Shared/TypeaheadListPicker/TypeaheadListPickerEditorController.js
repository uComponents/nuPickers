
angular
    .module('umbraco')
    .controller("nuComponents.DataTypes.Shared.TypeaheadListPicker.TypeaheadListPickerEditorController",
        ['$scope', 
        function ($scope) {
     
            //$scope.clear = function () {
            //    $scope.typeahead = null;
            //    $scope.selectableOptions = null;
            //};

            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {                

                if (newValue != null && newValue.length >= $scope.model.config.typeaheadListPicker.minCharacters) {
                    
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
                    $scope.selectableOptions = [];
                }

            });

            if ($scope.model.config.typeaheadListPicker.limitTo > 0) {
                $scope.$watchCollection('selectableOptions', function () {                    
                    $scope.selectableOptions = $scope.selectableOptions.slice(0, $scope.model.config.typeaheadListPicker.limitTo);
                });
            }

}]);

