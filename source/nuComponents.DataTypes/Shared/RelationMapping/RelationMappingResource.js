
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
     ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getRelatedIds: function (config) {
                    // TODO: calculate relationScopeIdentifier
                    var relationScopeIdentifier = '';

                    return $http({
                        method: 'GET',
                        url: 'backoffice/nuComponents/RelationMappingApi/GetRelatedIds',
                        params: {
                            'contextId': editorState.current.id,
                            'relationTypeAlias': config.relationMapping.relationTypeAlias,
                            'relationScopeIdentifier': relationScopeIdentifier
                        }
                    });

                },

                updateRelationMapping: function (config, pickedOptions) {    

                    //TODO: build identifier (for relationScope) and array of picked keys

                    var relationScopeIdentifier = '';

                    $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/RelationMappingApi/UpdateRelationMapping',
                        params: {
                            'contextId': editorState.current.id,
                            'relationTypeAlias': config.relationMapping.relationTypeAlias,
                            'relationScopeIdentifier': relationScopeIdentifier
                        },
                        data:  pickedOptions.map(function (option) { return parseInt(option.key); })
                    });

                }

            };
        }
]);