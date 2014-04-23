
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueLabelAttributeController",
    ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.PreValueStateResource',
    function ($scope, preValueStateResource) {

        // set local scope from the shared 
        $scope.macroSelected = preValueStateResource.macroSelected;

        // watch the shared apiResource and update local scope if this changes
        $scope.$watch(function () { return preValueStateResource.macroSelected; }, function () {
            // this scope value is used by the label attribute field
            $scope.macroSelected = preValueStateResource.macroSelected;
        });

    }]);