// persist data between multiple pre value controllers

angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.XPathTemplatableList.PreValueStateResource',
        function ($http) {
            
            return {

                macroSelected: null

            };
        }
);