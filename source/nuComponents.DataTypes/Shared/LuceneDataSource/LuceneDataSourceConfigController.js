
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.LuceneDataSource.LuceneDataSourceConfigController",
    ['$scope', function ($scope) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'LuceneDataSourceApi';

    }]);

