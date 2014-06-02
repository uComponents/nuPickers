
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
    ['$http', 'nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource', 'editorState',
        function ($http, saveFormatResource, editorState) {

            return {

                getEditorDataItems: function (config, typeahead) {

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/' + config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'contextId': editorState.current.id
                        },
                        data: {
                            'config': config,
                            'typeahead': typeahead
                        }
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