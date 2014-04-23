// api calls to get data

angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.XPathTemplatableList.ApiResource',
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
                }
            };
        }
);