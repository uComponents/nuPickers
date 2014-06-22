
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.RelationMapping.RelationMappingResource',
     ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getRelatedIds: function (model) {

                    return $http({
                        method: 'GET',
                        url: 'backoffice/nuComponents/RelationMappingApi/GetRelatedIds',
                        params: {
                            'contextId': editorState.current.id,
                            'propertyAlias' : model.alias,
                            'relationTypeAlias': model.config.relationMapping.relationTypeAlias,
                            'relationsOnly' : model.config.saveFormat == 'relationsOnly'
                        }
                    });

                },

                updateRelationMapping: function (model, pickedOptions) {    

                    $http({
                        method: 'POST',
                        url: 'backoffice/nuComponents/RelationMappingApi/UpdateRelationMapping',
                        params: {
                            'contextId': editorState.current.id,
                            'propertyAlias': model.alias,
                            'relationTypeAlias': model.config.relationMapping.relationTypeAlias,
                            'relationsOnly': model.config.saveFormat == 'relationsOnly'
                        },
                        data:  pickedOptions.map(function (option) { return parseInt(option.key); })
                    });

                }

            };
        }
]);