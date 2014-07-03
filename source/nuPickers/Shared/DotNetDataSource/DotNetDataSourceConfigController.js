
angular
    .module("umbraco")
    .controller("nuPickers.Shared.DotNetDataSource.DotNetDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.assemblyName = $scope.model.value.assemblyName || 'App_Code';
        $scope.model.value.apiController = 'DotNetDataSourceApi';

        $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetAssemblyNames').then(function (response) {

            $scope.assemblyNames = response.data;

            $scope.$watch('model.value.assemblyName', function () {

                $scope.classNames = null;

                $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetClassNames',
                    { params: { assemblyName: $scope.model.value.assemblyName } })
                    .then(function (response) {
                        $scope.classNames = response.data;
                    });

            });

            $scope.$watch('model.value.className', function () {

                $scope.properties = null;

                $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetProperties',
                    { params: { assemblyName: $scope.model.value.assemblyName, className: $scope.model.value.className } })
                    .then(function (response) {
                        $scope.properties = response.data;
                    });
            });

        });

    }]);

