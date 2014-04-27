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

            // local data
            $scope.selectedKeys = [];

            apiResource.getEditorOptions($scope.model.config).then(function (response) {

                // get data to build ui
                $scope.editorOptions = response.data; // [{"key":"","markup":""},{"key":"","markup":""}...]

                // rebuild selectedKeys from csv
                if ($scope.model.value) {
                    var savedKeys = $scope.model.value.split(',');
                    for (var i = 0; i < savedKeys.length; i++) {
                        // TODO: check to see if this key is in the available editor options

                        $scope.selectedKeys.push(savedKeys[i]);

                    }
                }

                // setup watch on local data
                $scope.$watch(function () { return $scope.selectedKeys.length; }, function () {
                    // set the model.value (for Umbraco to persist)
                    $scope.model.value = $scope.selectedKeys.join();
                });

            });

            $scope.isVisibileOption = function (key) {
                // TODO: add 'Hide Invalid Options'
                return $scope.isActiveOption(key);
            };

            $scope.isActiveOption = function (key) {
                if ($scope.model.config.allowDuplicates == '1' || $scope.selectedKeys.indexOf(key) == -1) {
                    return true;
                }
                return false;
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (key) {

                //TODO: check not exceeding the max

                // add item if allowing duplicates, else check it's not already in the list
                if ($scope.model.config.allowDuplicates == '1') {
                    $scope.selectedKeys.push(key);
                }
                else if ($scope.selectedKeys.indexOf(key) == -1) {
                    $scope.selectedKeys.push(key);
                }                
            };
    }]);