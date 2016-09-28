
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
                        deferred.resolve(saveFormatResource.getSavedKeys(model.value, model.config.saveFormat));
                    }

                    return deferred.promise;
                },

                // get keys & labels of picked items (required by typeahead, as picked keys might not be in the source data)
                getPickedItems: function(model) {                    
                    return saveFormatResource.getSavedItems(model.value);
                },

                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                }

            };
        }
    ]);