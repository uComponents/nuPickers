angular.module('umbraco.resources')
    .factory('nuPickers.Shared.Editor.EditorResource',
        ['$q',
        'nuPickers.Shared.DataSource.DataSourceResource',
        'nuPickers.Shared.SaveFormat.SaveFormatResource',
        'nuPickers.Shared.RelationMapping.RelationMappingResource',
        function ($q, dataSourceResource, saveFormatResource, relationMappingResource) {

            return {

                /** 
                 * Get the 'data editor items' for a property editor
                 * Proxy to dataSourceResource.getEditorDataItems
                 * @param {Object} model - the property editor model
                 * @param {string} or {Array} typeahead - optional typeahead text
                 * @returns {Object} - a promise of an http response with data of an array of 'data editor item' objects: [{ key: '', label: '' }]
                 */
                getEditorDataItems: function (model, typeahead) {
                    return dataSourceResource.getEditorDataItems(model, typeahead);
                },

                /**
                 * Get an array of all the picked keys
                 * @param {Object} model - the property editor model
                 * @returns {Object} - a promise to return an array of string keys
                 */
                getPickedKeys: function (model) {

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

                /**
                 * Get keys & labels of picked items (required by typeahead, as picked keys might not be in the source data)
                 * @param {Object} model - the property editor model
                 * @returns {Object} - a promise of an array of 'data editor item' objects: [{ key: '', label: '' }]
                 */
                getPickedEditorDataItems: function (model) {

                    var deferred = $q.defer();

                    // attempt to construct data from saved value
                    var pickedEditorDataItems = saveFormatResource.tryGetDataEditorItems(model.value);

                    if (pickedEditorDataItems != null) 
                    {
                        deferred.resolve(pickedEditorDataItems)
                    }
                    else // save format couldn't restore both key and label
                    {
                        var keys = saveFormatResource.getSavedKeys(model.value);

                        // re-query data source supplying keys
                        dataSourceResource.getEditorDataItems(model, null, keys).then(function (response) {
                            deferred.resolve(response.data);
                        });
                    }

                    return deferred.promise;
                },

                /**
                 * Proxy to saveFormat.createSaveValue
                 * @param {Object} config - 
                 * @param {Object} pickedOptions -
                 */
                createSaveValue: function (config, pickedOptions) {
                    return saveFormatResource.createSaveValue(config, pickedOptions);
                }

            };
        }
    ]);