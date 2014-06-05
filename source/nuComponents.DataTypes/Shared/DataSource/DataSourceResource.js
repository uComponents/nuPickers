
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

                    // if relations only, can we check for picker save event handler here ?



                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },

                getPickedKeys: function (config, savedValue) {

                    // if config.saveFormat = relationsOnly then .... get from relations

                    // else
                    return saveFormatResource.getSavedKeys(savedValue);
                }
            };
        }
    ]);