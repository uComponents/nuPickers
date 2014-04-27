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
                        "allowDuplicates":"1"
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
                $scope.$watch(function () { return $scope.selectedOptions.length; }, function () {
                    $scope.model.value = $scope.selectedOptions.map(function (option) { return option.key; }).join();
                });

                // build selectable options
                for (var i = 0; i < editorOptions.length; i++) {
                    // always add if duplicates can be selected
                    if ($scope.model.config.allowDuplicates == '1') { 
                        $scope.selectableOptions.push(editorOptions[i]);
                    }
                    // add if this option isn't already in the selected options array
                    else if ($scope.selectedOptions.indexOf(editorOptions[i]) == -1) {
                        $scope.selectableOptions.push(editorOptions[i]);
                    }
                }
            });

            // return ture, if the option can be selected (this is only relevant when hideInactive = false)
            $scope.isActive = function (option) {
                if ($scope.model.config.allowDuplicates == '1' ||
                    $scope.selectedOptions.indexOf(option) == -1) {
                    return true;
                }
                return false;
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {

                //TODO: check not exceeding the max

                // add item if allowing duplicates, else check it's not already in the list
                if ($scope.model.config.allowDuplicates == '1') {
                    $scope.selectedOptions.push(option);                    
                }
                // TODO: does this key already exist in the selectedOptions ?
                else if ($scope.selectedOptions.indexOf(option) == -1) {
                    $scope.selectedOptions.push(option);

                    // TODO: config value for hideInactive
                    // currently assuming hideInactive = true
                    for (var i = 0; i < $scope.selectableOptions.length; i++) {
                        if ($scope.selectableOptions[i] == option) {
                            $scope.selectableOptions.splice(i, 1);
                            break;
                        }
                    }
                }
            };
    }]);