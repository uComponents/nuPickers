// controller used when configuring the datatype in developer section, this handles all custom PreValueFields

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueController",
    ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.PreValueData',
    function ($scope, preValueData) {
    
        preValueData.getMacros().then(function (response) {
            $scope.macros = response.data;
        });

        preValueData.getStylesheets().then(function (response) {
            $scope.cssFiles = response;
        })

        preValueData.getScriptFiles().then(function (response) {
            $scope.scriptFiles = response.data;
        });

        // set local scope (for Label Attribtue) from shared PreValueData
        $scope.macroSelected = preValueData.macroSelected;

        // watch the shared preValueData and update local scope if this changes
        $scope.$watch(function () { return preValueData.macroSelected; }, function () {
            // this scope value is used by the label attribute field
            $scope.macroSelected = preValueData.macroSelected;
        });
        
        // only called by the macro drop down
        $scope.selectMacro = function () {
            // alert other PreValueFields using this controller that a macro has been selected
            preValueData.macroSelected = Boolean($scope.model.value);
        };

    }]);