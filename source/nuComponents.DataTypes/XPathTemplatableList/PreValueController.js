// controller used when configuring the datatype in developer section

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

        // POC - sharing the selectedMacro data between different instances of this controller, when resources value changes, set a local scope var
        $scope.$watch(function () { return preValueData.selectedMacro; }, function () {
            $scope.selectedMacro = preValueData.selectedMacro;
        });
        
        // watch for any changes in selecting a macro - and then set on the shared resource obj (TODO: probably a better way of setting this)
        $scope.$watch(function () { return $scope.selectedMacro; }, function () {
            preValueData.selectedMacro = $scope.selectedMacro;
        });

    }]);