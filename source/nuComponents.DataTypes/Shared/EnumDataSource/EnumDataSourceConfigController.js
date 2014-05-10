
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.EnumDataSource.EnumDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        //// is value not supplied,  then default to empty string
        //$scope.model.value = $scope.model.value || new Object();
        //$scope.model.value.assemblyName = $scope.model.value.assemblyName || '';

        $http.get('backoffice/nuComponentsDataTypesShared/EnumDataSourceApi/GetAssemblyNames').then(function (response) {

            $scope.assemblyNames = response.data;

            $scope.$watch('model.value.assemblyName', function () {

                $scope.enumNames = null;
                //$scope.model.value.enumName = null;
                
                $http.get('backoffice/nuComponentsDataTypesShared/EnumDataSourceApi/GetEnumNames',
                    { params: { assemblyName: $scope.model.value.assemblyName } })
                    .then(function (response) {

                        $scope.enumNames = response.data;

                    });

            });

        });

    }]);

