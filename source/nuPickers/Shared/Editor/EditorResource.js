
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.Editor.EditorResource',
        ['$q',
        'nuPickers.Shared.DataSource.DataSourceResource',
        'nuPickers.Shared.SaveFormat.SaveFormatResource',
        'nuPickers.Shared.RelationMapping.RelationMappingResource',
        function ($q, dataSourceResource, saveFormatResource, relationMappingResource) {

            return {

                getEditorDataItems: function (model, typeahead) {
                    return dataSourceResource.getEditorDataItems(model, typeahead);
                },

                getPickedKeys: function (model) {

                    // create a new promise....
                    var deferred = $q.defer();

                    if (model.config.saveFormat == 'relationsOnly') {

                        relationMappingResource.getRelatedIds(model).then(function (response) {
                            deferred.resolve(response.data.map(function (id) { return id.toString(); })); // ensure returning an array of strings
                        });

                    } else {
                        deferred.resolve(saveFormatResource.getSavedKeys(model.value));                        
                    }

                    return deferred.promise;
                },

                // get keys & labels of picked items (required by typeahead, as picked keys might not be in the source data)
                getPickedItems: function(model) {                    

                    // create a new promise....
                    var deferred = $q.defer();

                    if (model.config.saveFormat == 'relationsOnly') {
                        relationMappingResource.getRelatedIds(model).then(function (response) {
                            var ids = response.data.map(function (id) { return id.toString(); }).join(",");
                            dataSourceResource.getEditorDataItemsByIds(model, ids).then(function (response) {
                                deferred.resolve(saveFormatResource.getSavedItems(response.data));
                            });
                        });
                    } else {
                        var ids = saveFormatResource.getSavedKeys(model.value).join(",");
                        dataSourceResource.getEditorDataItemsByIds(model, ids).then(function (response) {
                            deferred.resolve(saveFormatResource.getSavedItems(response.data));
                        });
                    }
                    return deferred.promise;
                },

                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                },

                updateRelationMapping: function (model, pickedOptions) {
                    if (model.config.relationMapping != null) {
                        relationMappingResource.updateRelationMapping(model, pickedOptions);
                    }
                }

            };
        }
    ]);