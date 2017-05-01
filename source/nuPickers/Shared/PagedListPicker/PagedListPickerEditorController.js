angular
    .module("umbraco")
    .controller("nuPickers.Shared.PagedListPicker.PagedListPickerEditorController",
        ['$scope', 'nuPickers.Shared.Editor.EditorResource',
        function ($scope, editorResource) {
            
            //var itemsPerPage = $scope.model.config.pagedListPicker.itemsPerPage;
            $scope.currentPage = 1;
            $scope.pages = []; // an array of page numbers (suitable for ng-repeat)
            $scope.total = null;

            // watch count, as if it changes, the number of pages will need to be recalculated
            $scope.$watch('total', function (newValue, oldValue) {
                if (newValue != oldValue) {

                    var totalPages = Math.ceil(newValue / $scope.model.config.pagedListPicker.itemsPerPage);
                    var pages = [];

                    for (var page = 1; page <= totalPages; page++) {
                        pages.push(page);
                    }
                 
                    $scope.pages = pages;
                }
            });

            $scope.changePage = function (page) {
                $scope.currentPage = page;
                getEditorDataItems();
            };

            function getEditorDataItems() {
                editorResource.getEditorDataItems($scope.model, $scope.currentPage).then(function (response) {
                    $scope.$parent.selectableOptions = response.data.editorDataItems;
                    $scope.total = response.data.total;
                });
            }

            // get selectable options
            getEditorDataItems();

            // get selected options
            editorResource.getPickedEditorDataItems($scope.model).then(function (editorDataItems) {
                $scope.$parent.selectedOptions = editorDataItems;
            });

        }]);