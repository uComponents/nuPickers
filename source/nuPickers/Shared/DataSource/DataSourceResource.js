
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
                            'currentId': editorState.current.id,
                            'parentId': editorState.current.parentId,
                            'propertyAlias': model.alias
                        },
                        data: {
                            'config': model.config,
                            'typeahead': typeahead
                        }
                    });

                }

            };
        }
    ]);