
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.CustomLabel.CustomLabelConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $http.get('backoffice/nuComponents/CustomLabelApi/GetMacros').then(function (response) {
            $scope.macros = response.data;
        });

    }]);

