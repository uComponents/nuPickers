// api calls to get data

angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.ListPicker.ListPickerResource',
        function ($http, stylesheetResource) {

            return {

                getStylesheets: function () {
                    return stylesheetResource.getAll();
                },

                getScriptFiles: function () {
                    return $http.get('backoffice/nuComponents/XPathTemplatableListApi/GetScriptFiles');
                },

                getEditorOptions: function (config) {

                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/'+ config.listPickerApiController +'/GetEditorOptions',
                        data: config
                    });

                }
            };
        }
);