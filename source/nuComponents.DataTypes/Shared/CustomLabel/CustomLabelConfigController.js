
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.LabelMacro.LabelMacroConfigController",
    ['$scope', '$http', function ($scope, $http) {

        $http.get('backoffice/nuComponents/CustomLabelApi/GetMacros').then(function (response) {
            $scope.macros = response.data;
        });

    }]);

