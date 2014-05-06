angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.DropDownList.DropDownListResource',
        function ($http, stylesheetResource) {

            return {

                getEditorOptions: function (config) {

                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/' + config.apiController + '/GetEditorOptions',
                        data: config
                    });

                }
            };
        }
);