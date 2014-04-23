
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueMacroController",
    ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.ApiResource', 'nuComponents.DataTypes.XPathTemplatableList.PreValueStateResource',
    function ($scope, apiResource, preValueStateResource) {

        apiResource.getMacros().then(function (response) {
            $scope.macros = response.data;
        });

        $scope.selectMacro = function () {
            // alert other PreValues that a macro has been selected
            preValueStateResource.macroSelected = Boolean($scope.model.value); // an empty string returns false
        };

    }]);
