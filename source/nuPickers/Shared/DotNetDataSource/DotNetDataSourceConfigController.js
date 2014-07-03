
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
                $scope.properties = null;

                $scope.model.value.className = null;

                $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetClassNames',
                    { params: { assemblyName: $scope.model.value.assemblyName } })
                    .then(function (response) {
                        $scope.classNames = response.data;
                    });

            });

            $scope.$watch('model.value.className', function () {

                $scope.properties = null;

                if ($scope.model.value.className != null) {
                    $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetProperties',
                        { params: { assemblyName: $scope.model.value.assemblyName, className: $scope.model.value.className } })
                        .then(function (response) {
                            $scope.properties = response.data;

                            // TODO: set any existing property values

                        });
                }
            });            


            $scope.$on("formSubmitting", function () {

                $scope.model.value.properties = $scope.properties.map(function (property) {
                    return { 'name': property.name, 'value': property.value }
                });

            });
        });



    }]);

