
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.Editor.EditorResource',
        ['$q',
        'nuComponents.DataTypes.Shared.DataSource.DataSourceResource',
        'nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        'nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
        function ($q, dataSourceResource, saveFormatResource, relationMappingResource) {

            return {

                getEditorDataItems: function (config, typeahead) {
                    return dataSourceResource.getEditorDataItems(config, typeahead);
                },

                getPickedKeys: function (config, savedValue) {

                    // create a new promise....
                    var deferred = $q.defer();

                    if (config.saveFormat == 'relationsOnly') {

                        relationMappingResource.getRelatedIds(config).then(function (response) {
                            deferred.resolve(response.data.map(function (id) { return id.toString(); })); // ensure returning an array of strings
                        });

                    } else {
                        deferred.resolve(saveFormatResource.getSavedKeys(savedValue));                        
                    }

                    return deferred.promise;
                },

                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },

                updateRelationMapping: function (config, pickedOptions) {
                    if (config.relationMapping != null) {
                        relationMappingResource.updateRelationMapping(config, pickedOptions);
                    }
                }

            };
        }
    ]);