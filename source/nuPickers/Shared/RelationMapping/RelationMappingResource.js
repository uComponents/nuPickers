
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.RelationMapping.RelationMappingResource',
     ['$http', 'editorState',
        function ($http, editorState) {

            return {

                getRelatedIds: function (model) {

                    return $http({
                        method: 'GET',
                        url: 'backoffice/nuPickers/RelationMappingApi/GetRelatedIds',
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
                        url: 'backoffice/nuPickers/RelationMappingApi/UpdateRelationMapping',
                        params: {
                            'contextId': editorState.current.id,
                            'propertyAlias': model.alias,
                            'relationTypeAlias': model.config.relationMapping.relationTypeAlias,
                            'relationsOnly': model.config.saveFormat == 'relationsOnly'
                        },
                        data: (pickedOptions == null || pickedOptions.length == 0 || pickedOptions[0] == null) ? []
                            : pickedOptions.map(function (option) { return parseInt(option.key); })
                    });

                }

            };
        }
]);