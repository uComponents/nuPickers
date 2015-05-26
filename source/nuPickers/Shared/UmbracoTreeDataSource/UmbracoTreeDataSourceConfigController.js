
angular
    .module("umbraco")
    .controller("nuPickers.Shared.UmbracoTreeDataSource.UmbracoTreeDataSourceConfigController",
    ['$scope', function ($scope) {

        $scope.model.value = $scope.model.value || new Object();

    }]);