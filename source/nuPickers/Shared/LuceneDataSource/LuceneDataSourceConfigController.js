
angular
    .module("umbraco")
    .controller("nuPickers.Shared.LuceneDataSource.LuceneDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.isLabels = false;
        $scope.$on('isLabels', function () { $scope.isLabels = true; });
        // NOTE: broadcast to tell sender that we're ready to listen not required (happens to work as in correct execution order)

        $http.get('backoffice/nuPickers/LuceneDataSourceApi/GetExamineSearchers').then(function (response) {
            $scope.examineSearchers = response.data;
        });

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'LuceneDataSourceApi';

    }]);

