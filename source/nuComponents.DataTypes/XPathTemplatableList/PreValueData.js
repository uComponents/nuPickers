// api calls to get data, and to also persist data between multiple controllers (eg. between pre value fields)

angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.XPathTemplatableList.PreValueData',
        function ($http, stylesheetResource) {
            
            return {

                getMacros: function () {
                    return $http.get('backoffice/nuComponents/XPathTemplatableListApi/GetMacros');
                },

                getStylesheets: function () {
                    return stylesheetResource.getAll();
                },

                getScriptFiles: function () {
                    return $http.get('backoffice/nuComponents/XPathTemplatableListApi/GetScriptFiles');
                },

                macroSelected: null

            };
        }
);