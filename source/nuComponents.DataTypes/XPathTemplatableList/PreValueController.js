// controller used when configuring the datatype in developer section

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.PreValueController",
    function ($scope, $http, stylesheetResource) {

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


    });