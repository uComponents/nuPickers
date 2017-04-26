
angular
    .module('umbraco')
    .controller("nuPickers.Shared.TypeaheadListPicker.TypeaheadListPickerEditorController",
        ['$scope', '$timeout', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, $timeout, editorResource) {

            // build selected options (typeahead doesn't have any default selectable options without first entering some typeahead text)
            editorResource.getPickedEditorDataItems($scope.model).then(function (editorDataItems) {
                $scope.setSelectedOptions(editorDataItems);
            });

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
                        editorResource.getEditorDataItems($scope.model, newValue).then(function (response) {
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

