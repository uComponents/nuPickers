
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueScriptFileController",
    ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.ApiResource',
    function ($scope, apiResource) {

        apiResource.getScriptFiles().then(function (response) {
            $scope.scriptFiles = response.data;
        });

    }]);
