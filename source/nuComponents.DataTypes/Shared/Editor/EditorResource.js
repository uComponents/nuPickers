
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.Editor.EditorResource',
        ['$http',
        'editorState',
        'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        'nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        'nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
        function ($http, editorState, dataSourceResource, saveFormatResource, relationMappingResource) {

            return {

                getEditorDataItems: function (config, typeahead) {
                    return dataSourceResource.getEditorDataItems(config, typeahead);
                },

                getPickedKeys: function (config, savedValue) {

                    // if config.saveFormat = relationsOnly then .... get from relations

                    // else
                    return saveFormatResource.getSavedKeys(savedValue);
                },

                createSaveValue: function (config, pickedOptions) {

                    // if relations only, can we check for picker save event handler here ?

                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },


                updateRelationMapping: function (config, pickedOptions) {

                    //TODO:

                }

            };
        }
    ]);