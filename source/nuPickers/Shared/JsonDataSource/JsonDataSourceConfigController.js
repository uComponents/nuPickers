
angular
    .module("umbraco")
    .controller("nuPickers.Shared.JsonDataSource.JsonDataSourceConfigController",
    ['$scope', function ($scope) {

        // hide the key option if this is being used by labels
        $scope.isLabels = false;
        $scope.$on('isLabels', function () { $scope.isLabels = true; });
        // NOTE: broadcast to tell sender that we're ready to listen not required (happens to work as in correct execution order)

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.apiController = 'JsonDataSourceApi';

    }]);

