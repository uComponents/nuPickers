
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.Picker.PickerResource',
    ['$http', 'nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        function ($http, saveFormatResource) {

            return {
                 
                getEditorOptions: function (config) {

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/' + config.apiController + '/GetEditorOptions',
                        data: config
                    });

                },
                
                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },

                getSavedKeys: function (savedValue) {
                    return saveFormatResource.getSavedKeys(savedValue);
                }
            };
        }
]);