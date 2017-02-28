
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.DataSource.DataSourceResource',
    ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getEditorDataItems: function (model, typeahead) {
                    
                    var currentId = 0;
                    var parentId = 0;

                    if (editorState.current) {
                        currentId = editorState.current.id;
			            parentId = editorState.current.parentId;
                    }

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/' + model.config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'currentId': currentId,
                            'parentId': parentId,
                            'propertyAlias': model.alias
                        },
                        data: {
                            'config': model.config,
                            'typeahead': typeahead
                        }
                    });

                },

                getEditorDataItemsByIds: function (model, ids) {

                    // If no ids are provided make sure no content is returned
                    if (!ids || ids === '')
                        ids = '0';

                    var currentId = 0;
                    var parentId = 0;

                    if (editorState.current) {
                        currentId = editorState.current.id;
                        parentId = editorState.current.parentId;
                    }

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/' + model.config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'currentId': currentId,
                            'parentId': parentId,
                            'propertyAlias': model.alias,
                            'ids': ids
                        },
                        data: {
                            'config': model.config,
                            'typeahead': null
                        }
                    });

                }

            };
        }
    ]);
