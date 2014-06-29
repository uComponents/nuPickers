
angular
    .module("umbraco")
    .controller("nuPickers.Shared.LuceneDataSource.LuceneDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $http.get('backoffice/nuPickers/LuceneDataSourceApi/GetExamineSearchers').then(function (response) {
            $scope.examineSearchers = response.data;
        });

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'LuceneDataSourceApi';

    }]);

