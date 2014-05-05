
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.XmlDataSource.XmlDataSourceConfigController",
    ['$scope', '$http', function ($scope, $http) {

        // TODO: where better to move this more genereic api call ?
        $http.get('backoffice/nuComponentsDataTypesShared/XmlDataSourceApi/GetMacros').then(function (response) {
            $scope.macros = response.data;
        });

        // TODO: how best to set a defualt for an unconfigured type ?

        //$scope.model.value = {
        //    "xmlSchema": "content",
        //    "optionsXPath": "//*[@isDoc]",
        //    "keyAttribute": "id",
        //    "labelAttribute": "nodeName",
        //    "labelMacro": null                // could be a better place for this to move to ?
        //};

    }]);

