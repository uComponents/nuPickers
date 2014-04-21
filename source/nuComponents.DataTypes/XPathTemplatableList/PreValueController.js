// controller used when configuring the datatype in developer section

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueController",
    ['$scope', '$http', 'stylesheetResource', 'nuComponents.DataTypes.XPathTemplatableList.Resources',
    function ($scope, $http, stylesheetResource, resources) {
    
        // TODO: move all 'data getters' into the init of the shared resources obj
        $http.get('backoffice/nuComponents/XPathTemplatableListApi/GetMacros')
            .then(function (response) {
                $scope.macros = response.data;
            });
        
        stylesheetResource.getAll()
            .then(function (stylesheets) {
                $scope.cssFiles = stylesheets;
            })

        $http.get('backoffice/nuComponents/XPathTemplatableListApi/GetScriptFiles')
            .then(function (response) {
                $scope.scriptFiles = response.data;
            });

        // POC - sharing the selectedMacro data between different instances of this controller, when resources value changes, set a local scope var
        $scope.$watch(function () { return resources.selectedMacro; }, function () {
            $scope.selectedMacro = resources.selectedMacro;
        });
        
        // watch for any changes in selecting a macro - and then set on the shared resource obj (TODO: probably a better way of setting this)
        $scope.$watch(function () { return $scope.selectedMacro; }, function () {
            resources.selectedMacro = $scope.selectedMacro;
        });

    }]);