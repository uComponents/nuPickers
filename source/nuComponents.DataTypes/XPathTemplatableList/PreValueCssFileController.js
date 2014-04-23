angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueCssFileController",
    ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.ApiResource',
    function ($scope, apiResource) {

        apiResource.getStylesheets().then(function (response) {
            $scope.cssFiles = response;
        })

    }]);