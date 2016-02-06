
angular
    .module('umbraco')
    .controller("nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerEditorController",
        ['$scope', '$timeout',
        function ($scope, $timeout) {

            //$scope.clear = function () {
            //    $scope.typeahead = null;
            //    $scope.selectableOptions = null;
            //};

            var wait; // typeahead call to get options based on text input

            // setup a watch on the input
            $scope.$watch('typeahead', function (newValue, oldValue) {

                // cancel any existing timeout
                if (wait) { $timeout.cancel(wait); }

                if (newValue != null && newValue.length >= $scope.model.config.typeaheadListPicker.minCharacters) {

                    wait = $timeout(function () {
                        $scope.getEditorOptions(newValue).then(function (response) {
                            $scope.noMatch = response.data.length == 0;
                            $scope.selectableOptions = response.data;
                        });
                    }, 250);

                } else {
                    $scope.noMatch = false;
                    $scope.selectableOptions = [];
                }

            });

            if ($scope.model.config.typeaheadListPicker.limitTo > 0) {
                $scope.$watchCollection('selectableOptions', function () {
                    $scope.selectableOptions = $scope.selectableOptions.slice(0, $scope.model.config.typeaheadListPicker.limitTo);
                });
            }

}]);

