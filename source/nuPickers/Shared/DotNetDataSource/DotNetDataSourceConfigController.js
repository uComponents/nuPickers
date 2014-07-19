
angular
    .module("umbraco")
    .controller("nuPickers.Shared.DotNetDataSource.DotNetDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $scope.model.value = $scope.model.value || new Object();
        $scope.model.value.assemblyName = $scope.model.value.assemblyName || null;
        $scope.model.value.apiController = 'DotNetDataSourceApi';

        $scope.buildProperties = function () {

            $scope.properties = null;

            if ($scope.model.value.className != null) {
                $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetProperties',
                    {
                        params: {
                            assemblyName: $scope.model.value.assemblyName,
                            className: $scope.model.value.className
                        }
                    })
                    .then(function (response) {
                        if (response.data != 'null') {
                            $scope.properties = response.data;

                            if ($scope.model.value.properties) {
                                for (var i = 0; i < $scope.properties.length; i++) {
                                    for (var j = 0; j < $scope.model.value.properties.length; j++) {
                                        if ($scope.properties[i].name == $scope.model.value.properties[j].name) {
                                            $scope.properties[i].value = $scope.model.value.properties[j].value;
                                        }
                                    }
                                }
                            }
                        }
                    });
            }
        };

        $scope.$watch('model.value.assemblyName', function () {

            $scope.classNames = null; // clear, incase new ones can't be set
            $scope.properties = null;

            if ($scope.model.value.assemblyName != null) {
                $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetClassNames',
                    {
                        params: { assemblyName: $scope.model.value.assemblyName }
                    })
                    .then(function (response) {
                        $scope.classNames = response.data;
                        $scope.buildProperties(); // (re)build properties collection
                    });
            }
        });

        $scope.$watch('model.value.className', function () {
            $scope.buildProperties();
        });

        $scope.$on("formSubmitting", function () {

            $scope.model.value.properties = null;
            $scope.model.value.properties = $scope.properties.map(function (property) {
                return { 'name': property.name, 'value': property.value }
            });

        });

        // set collection of assemblies
        $http.get('backoffice/nuPickers/DotNetDataSourceApi/GetAssemblyNames').then(function (response) {
            $scope.assemblyNames = response.data;
        });

    }]);

