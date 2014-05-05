// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.ListPicker.ListPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.ListPicker.ListPickerResource',
        function ($scope, listPickerResource) {

            /*
            <div class="ng-binding">model.config = {

                $scope.model = {
                    "label":"Xml List Picker - Test 1",
                    "description":"",
                    "view":"App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerEditor.html",
                    "config":{     
                    
                        // THE DATA SOURCE COULD DIFFER

                        "xmlDataSource":{
                            "xmlSchema":"member",
                            "optionsXPath":"//*[@isDoc]",
                            "keyAttribute":"id",
                            "labelAttribute":
                            "nodeName",
                            "labelMacro":null
                         },

                         "listPicker": {
                            "cssFile":null,
                            "scriptFile":null,
                            "listHeight":null,
                            "minItems":"0",
                            "maxItems":"0",
                            "allowDuplicates":"0",
                            "hideUsed":"1",
                            "enableFiltering": "0"} // TODO: swap to bool
                        }

                        // "listPickerApiController" : "XPathTemplatableListApi"
                    },
                    "hideLabel":false,
                    "id":160,
                    "value":"1067,1068,1069",
                    "alias":"xmlListPickerTest1"
                };                
            */




            // array of option objects, for the selectable list 
            $scope.selectableOptions = []; // [{"key":"","markup":""}...]

            // array of option objects, for the selected list
            $scope.selectedOptions = []; // [{"key":"","markup":""}...]

            // http://api.jqueryui.com/sortable/
            $scope.sortableConfiguration = { axis: 'y' };

            // returns true if option hans't yet been picked, or duplicates are allowed
            $scope.isSelectable = function (option) {
                return ($scope.model.config.listPicker.allowDuplicates == '1' || !$scope.isUsed(option));
            };

            // returns true is this option has been picked
            $scope.isUsed = function (option) {
                return $scope.selectedOptions.indexOf(option) >= 0;
            };

            // return true is option can be picked
            $scope.isValidSelection = function (option) {
                return $scope.isSelectable(option) && ($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems || $scope.model.config.listPicker.maxItems <= 0);
            };

            // picking an item from 'selectable' for 'selected'
            $scope.selectOption = function (option) {

                // if item can be selected and not exceeding the max
                if ($scope.isValidSelection(option)) {
                    $scope.selectedOptions.push(option);
                }
            };

            // count for dashed placeholders
            $scope.getRequiredPlaceholderCount = function () {
                var count = $scope.model.config.listPicker.minItems - $scope.selectedOptions.length;
                if (count > 0) { return new Array(count); }
                return null;
            }

            $scope.showSelectPlaceholder = function () {

                return ($scope.selectedOptions.length >= $scope.model.config.listPicker.minItems)
                        && (($scope.selectedOptions.length < $scope.model.config.listPicker.maxItems) || $scope.model.config.listPicker.maxItems == 0);
                // TODO: and selectable options, has valid options
            }

            // return true if there is more than 1 item in the selected list
            $scope.isSortable = function () {
                return $scope.selectedOptions.length > 1; // TODO: check selectedOptions are not all the same
            };

            // remove option from 'selected'
            $scope.deselectOption = function ($index) {
                $scope.selectedOptions.splice($index, 1);
            };


            // call api, supplying all configuration details, and expect a collection of options (key / markup) to be populated
            //pickerResource.getEditorOptions($scope.model.config.configuration).then(function (response) {
            listPickerResource.getEditorOptions($scope.model.config).then(function (response) {

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

                    // TODO: how to return error to Umbraco ?
                    //var isValid = ($scope.selectableOptions.length >= $scope.model.config.minItems
                    //               && ($scope.selectableOptions.length <= $scope.model.config.maxItems || $scope.model.config.maxItems < 1));

                    // toggle sorting ui
                    $scope.sortableConfiguration.disabled = !$scope.isSortable();
                });

                // build selectable options
                $scope.selectableOptions = editorOptions;

                // setup filtering
                if ($scope.model.config.listPicker.enableFiltering) {
                    $scope.allSelectableOptions = $scope.selectableOptions;

                    $scope.$watch('model.filterQuery', function (newValue, oldValue) {
                        if (newValue == null || newValue.length == 0)
                            return $scope.selectableOptions = $scope.allSelectableOptions;

                        $scope.selectableOptions = $scope.allSelectableOptions.filter(function (item) {

                            //TODO: prevent typing that would yeild no matches

                            return item.markup.toLowerCase().indexOf(newValue) != -1;
                        });
                    });
                }
            });

        }]);