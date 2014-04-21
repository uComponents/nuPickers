// TODO: is there a better name for this ? - this obj is used to supply data to controllers, and to persist state between controller instances

//adds the resource to umbraco.resources module:
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.XPathTemplatableList.Resources',
        function ($http) {

            //the factory object returned
            return {

                selectedMacro: null,

            };
        }
);