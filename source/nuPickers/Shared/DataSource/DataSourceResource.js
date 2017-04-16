angular.module('umbraco.resources')
    .factory('nuPickers.Shared.DataSource.DataSourceResource',
    ['$http', 'editorState',
        function ($http, editorState) {

            return {

                /**
                 * Get 'editor data items', either the collection for picking, or specifc ones identified by key
                 * @param {Object} - the property editor model
                 * @param {string} - optional typeahead text
                 * @param {Array} - optional array of picked keys
                 * @returns {Object} - a promise to return an array of 'editor data items',  [{"key":"","label":""},{"key":"","label":""}...]
                 */
                getEditorDataItems: function (model, typeahead, keys) {
                    
                    var currentId = 0;
                    var parentId = 0;

                    if (editorState.current) {
                        currentId = editorState.current.id;
			            parentId = editorState.current.parentId;
                    }

                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/DataSourceApi/GetEditorDataItems',
                        params: {
                            'currentId': currentId,
                            'parentId': parentId,
                            'propertyAlias': model.alias
                        },
                        data: {
                            'config': model.config,
                            'typeahead': typeahead,
                            'keys': keys
                        }
                    });

                }

            };
        }
    ]);