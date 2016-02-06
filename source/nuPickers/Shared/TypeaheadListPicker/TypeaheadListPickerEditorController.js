
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

                $scope.noMatch = false;
                if (newValue != null && newValue.length >= $scope.model.config.typeaheadListPicker.minCharacters) {

                    // cancel the timeout as setting a new one
                    if (wait) {
                        $timeout.cancel(wait);
                    }

                    wait = $timeout(function () {

                        $scope.getEditorOptions(newValue).then(function (response) {

                            // no options returned
                            if (response.data.length == 0){
                                $scope.noMatch = true;
                            }

                            $scope.selectableOptions = response.data;
                        });

                    }, 250);

                } else {

                    // Cancel the timeout (since we're not actually doing a search)
                    if (wait) {
                        $timeout.cancel(wait);
                    }

                    $scope.selectableOptions = [];

                }

            });

            if ($scope.model.config.typeaheadListPicker.limitTo > 0) {
                $scope.$watchCollection('selectableOptions', function () {
                    $scope.selectableOptions = $scope.selectableOptions.slice(0, $scope.model.config.typeaheadListPicker.limitTo);
                });
            }

}]);

