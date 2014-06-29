
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.DataSource.DataSourceResource',
    ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getEditorDataItems: function (config, typeahead) {

                    // returns [{"key":"","label":""},{"key":"","label":""}...]
                    return $http({
                        method: 'POST',
                        url: 'backoffice/nuPickers/' + config.dataSource.apiController + '/GetEditorDataItems',
                        params: {
                            'contextId': editorState.current.id
                        },
                        data: {
                            'config': config,
                            'typeahead': typeahead
                        }
                    });

                }

            };
        }
    ]);