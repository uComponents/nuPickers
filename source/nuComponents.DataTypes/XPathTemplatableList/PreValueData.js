// api calls to get data, and to also persist data between multiple controllers (eg. between pre value fields)

angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.XPathTemplatableList.PreValueData',
        function ($http) {

            //the factory object returned
            return {

                selectedMacro: null

            };
        }
);