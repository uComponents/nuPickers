
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.Editor.EditorResource',
        ['nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        'nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        'nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
        function (dataSourceResource, saveFormatResource, relationMappingResource) {

            return {

                getEditorDataItems: function (config, typeahead) {
                    return dataSourceResource.getEditorDataItems(config, typeahead);
                },

                getPickedKeys: function (config, savedValue) {

                    if (config.saveFormat == 'relationsOnly') {

                        // TODO: convert colleciton of ints to strings ?

                        return relationMappingResource.getRelatedIds(config);

                    } else {
                        return saveFormatResource.getSavedKeys(savedValue);
                    }

                },

                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },

                updateRelationMapping: function (config, pickedOptions) {
                    relationMappingResource.updateRelationMapping(config, pickedOptions);
                }

            };
        }
    ]);