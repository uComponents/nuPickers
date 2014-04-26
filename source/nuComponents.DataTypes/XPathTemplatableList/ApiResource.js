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
                },

                getEditorOptions: function (config) {
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/XPathTemplatableListApi/GetEditorOptions',                        
                        data: config // posts the full config obj which can be reinflated as XPathTemplatableListPreValueEditor
                    });                        
                }


            };
        }
);