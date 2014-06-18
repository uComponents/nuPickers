
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
     ['$http', 'editorState',
        function ($http, editorState) {

            function getRelationIdentifier(config) {

                var relationIdentifier = '';

                if (config.relationMapping.relationIdentifier == '1') {
                    // if relation type is bidirectional then use propertyType ID
                    // else use datatype instance ID
                }

                return relationIdentifier;
            }

            return {

                getRelatedIds: function (config) {

                    return $http({
                        method: 'GET',
                        url: 'backoffice/nuComponents/RelationMappingApi/GetRelatedIds',
                        params: {
                            'contextId': editorState.current.id,
                            'relationTypeAlias': config.relationMapping.relationTypeAlias,
                            'relationIdentifier': getRelationIdentifier(config)

                        }
                    });

                },

                updateRelationMapping: function (config, pickedOptions) {    

                    $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/RelationMappingApi/UpdateRelationMapping',
                        params: {
                            'contextId': editorState.current.id,
                            'relationTypeAlias': config.relationMapping.relationTypeAlias,
                            'relationIdentifier': getRelationIdentifier(config)
                        },
                        data:  pickedOptions.map(function (option) { return parseInt(option.key); })
                    });

                }

            };
        }
]);