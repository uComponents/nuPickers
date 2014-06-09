
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
     ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getRelatedIds: function (config) {
                    console.log('getting related ids');

                    /// supply current id, and relation type

                   

                },

                updateRelationMapping: function (config, pickedOptions) {
                    console.log('updating relation mapping...');


                    // supply current id, relation type, (boundary = build identifier here ?)


                    console.log(editorState);
                    console.log(config);

                }

            };
        }
]);