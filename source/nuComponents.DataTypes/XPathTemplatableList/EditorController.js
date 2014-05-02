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
            $scope.selectableOptions = []; // [{"key":"","markup":""}...]

            // array of option objects, for the selected list
            $scope.selectedOptions = []; // [{"key":"","markup":""}...]

            // http://api.jqueryui.com/sortable/
            $scope.sortableConfiguration = { axis: 'y' };


            // return ture, if the option could be a valid selection
            $scope.isSelectable = function (option) {

                return ($scope.model.config.allowDuplicates == '1' ||
                        $scope.selectedOptions.indexOf(option) == -1); // not in the selected list
            };

            
            $scope.canSelect = function (option) {
                return $scope.isSelectable(option) && ($scope.selectedOptions.length < $scope.model.config.maxItems || $scope.model.config.maxItems <= 0);
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {

                // if item can be selected and not exceeding the max
                if ($scope.canSelect(option)) {
                    $scope.selectedOptions.push(option);
                }
            };

            // count for dashed placeholders
            $scope.getPlaceholderCount = function () {
                var count = $scope.model.config.minItems - $scope.selectedOptions.length;
                if (count > 0) { return new Array(count); }
                return null;
            }

            // return true if there is more than 1 item in the selected list
            $scope.isSortable = function () {
                return $scope.selectedOptions.length > 1;
            };

            // remove option from 'selected'
            $scope.deselectOption = function ($index) {
                $scope.selectedOptions.splice($index, 1);
            };



            // call api, supplying all configuration details, and expect a collection of options (key / markup) to be populated
            //apiResource.getEditorOptions($scope.model.config.configuration).then(function (response) {
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

                // setup watch on selected options
                $scope.$watchCollection('selectedOptions', function () {

                    //recreate the csv in model.value for Umbraco - TODO: json, xml, or csv
                    $scope.model.value = $scope.selectedOptions.map(function (option) { return option.key; }).join();

                    // TOOD: validation checks on user data
                    //$scope.hasError = true;
                    //if ($scope.selectableOptions.length >= $scope.model.config.minItems
                    //    && ($scope.selectableOptions.length <= $scope.model.config.maxItems || $scope.model.config.maxItems < 1)) {
                    //    $scope.hasError = false;
                    //}

                    // toggle sorting ui
                    $scope.sortableConfiguration.disabled = !$scope.isSortable();
                });

                // build selectable options
                $scope.selectableOptions = editorOptions;
            });

    }]);