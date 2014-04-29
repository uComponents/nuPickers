// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.XPathTemplatableList.EditorController",
        ['$scope', 'nuComponents.DataTypes.XPathTemplatableList.ApiResource',
        function ($scope, apiResource) {

            /*
                $scope.model = {
                    "label":"XPathTemplatableList",
                    "description":"",
                    "view":"App_Plugins/nuComponents/DataTypes/XPathTemplatableList/Editor.html",
                    "config":{
                        "xmlSchema":"content",
                        "optionsXPath":"//*[@isDoc]",
                        "keyAttribute":"id",
                        "labelAttribute":"nodeName",
                        "macro":null,
                        "cssFile":null,
                        "scriptFile":null,
                        "listHeight":"0",
                        "minItems":"0",
                        "maxItems":"0",
                        "allowDuplicates":"1",
                        "showUnselectable":"1"
                    },
                    "hideLabel":false,
                    "id":160,
                    "value":"1067,1068,1069",
                    "alias":"xPathTemplatableList"
                };                
            */

            // array of option objects, for the selectable list 
            $scope.selectableOptions = []; // [{"key":"","markup":""},{"key":"","markup":""}...]

            // array of option objects, for the selected list
            $scope.selectedOptions = []; // [{"key":"","markup":""},{"key":"","markup":""}...]

            // call api, supplying all configuration details, and expect a collection of options (key / markup) to be populated
            apiResource.getEditorOptions($scope.model.config).then(function (response) {

                var editorOptions = response.data; // [{"key":"","markup":""},{"key":"","markup":""}...]

                // build selected options (from saved csv)
                var savedKeys = $scope.model.value.split(',');
                for (var i = 0; i < savedKeys.length; i++) { // loop though each saved key
                    for (var j = 0; j < editorOptions.length; j++) { // loop though each editor option
                        if (savedKeys[i] == editorOptions[j].key) {
                            $scope.selectedOptions.push(editorOptions[j]);
                            break; // break out of the editor option loop
                        }
                    }
                }

                // setup watch on selected options to recreate the csv in model.value for Umbraco
                $scope.$watchCollection('selectedOptions', function () {
                    $scope.model.value = $scope.selectedOptions.map(function (option) { return option.key; }).join();
                });

                // build selectable options
                $scope.selectableOptions = editorOptions;
            });

            // return ture, if the option can be selected
            $scope.isSelectable = function (option) {

                return ($scope.model.config.allowDuplicates == '1' ||
                        $scope.selectedOptions.indexOf(option) == -1); // not in the selected list
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {

                // TODO: check not exceeding the max

                // if item can be selected, then add it to the selected list
                if ($scope.isSelectable(option)) {
                    $scope.selectedOptions.push(option);
                }
            };

            $scope.deselectOption = function ($index, option) {

                // TODO: check not less than min

                // remove item from collection
                $scope.selectedOptions.splice($index, 1);

                //// remove this option from the selected collection
                //for (var i = 0; i < $scope.selectedOptions.length; i++) {
                //    if ($scope.selectedOptions[i] == option) {
                //        $scope.selectedOptions.splice(i, 1);
                //        break;
                //    }
                //}
            };
    }]);