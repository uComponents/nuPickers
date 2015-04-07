
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.DataSource.DataSourceResource',
    ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getEditorDataItems: function (model, typeahead) {

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/' + model.config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'contextId': editorState.current.id,
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

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/' + model.config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'contextId': editorState.current.id,
                            'propertyAlias': model.alias,
                            'ids': ids,
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