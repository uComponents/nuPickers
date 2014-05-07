
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.Core.PickerResource',
        function ($http) {

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