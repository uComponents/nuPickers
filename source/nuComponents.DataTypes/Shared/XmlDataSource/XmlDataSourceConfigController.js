
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.XmlDataSource.XmlDataSourceConfigController",
    ['$scope',
    function ($scope) {



        // hard coded value for testing
        $scope.model.value = {
            "xmlSchema": "content",
            "optionsXPath": "//*[@isDoc]",
            "keyAttribute": "id",
            "labelAttribute": "nodeName",
            "labelMacro": null
        };

    }]);

