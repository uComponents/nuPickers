
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.CheckBoxPicker.CheckBoxPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Picker.PickerResource',
        function ($scope, pickerResource) {

            pickerResource.getEditorOptions($scope.model.config).then(function (response) {

                var editorOptions = response.data; 
                
                // set isChecked state for each option based on any saved value
                var savedKeys = $scope.model.value.split(',');
                for (var i = 0; i < savedKeys.length; i++) { // loop though each saved key
                    for (var j = 0; j < editorOptions.length; j++) { // loop though each editor option
                        if (savedKeys[i] == editorOptions[j].key) {
                            editorOptions[j].isChecked = true;
                            break;
                        }
                    }
                }

                $scope.checkBoxPickerOptions = editorOptions;

                // setup watch on selected options
                $scope.$watch('checkBoxPickerOptions', function () {

                    //recreate the csv in model.value for Umbraco - TODO: json, xml, or csv
                    console.log('check boxes changed')

                    $scope.model.value = $scope.checkBoxPickerOptions
                                                    .filter(function(option) {
                                                        return option.isChecked == true;
                                                    })
                                                    .map(function(option) { 
                                                        return option.key 
                                                    })
                                                    .join();

                },
                true); // deep watch

            });

        }]);