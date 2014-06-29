
angular
    .module("umbraco")
    .controller("nuPickers.Shared.CustomLabel.CustomLabelConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $http.get('backoffice/nuPickers/CustomLabelApi/GetMacros').then(function (response) {
            $scope.macros = response.data;
        });

    }]);

