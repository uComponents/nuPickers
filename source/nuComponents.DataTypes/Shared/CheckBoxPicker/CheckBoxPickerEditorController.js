
angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.CheckBoxPicker.CheckBoxPickerEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.Editor.EditorResource',
        function ($scope, editorResource) {

            editorResource.getEditorDataItems($scope.model.config).then(function (response) {

                var editorOptions = response.data; 
                
                // set isChecked state for each option based on any saved value
                var savedKeys = editorResource.getPickedKeys($scope.model.config, $scope.model.value);
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

                    $scope.model.value = editorResource.createSaveValue($scope.model.config,
                                                                        $scope.checkBoxPickerOptions.filter(function (option) {
                                                                            return option.isChecked == true;
                                                                        }));
                },
                true); // deep watch


                // TODO: set up an event to do something on save - this will set the relations, instead of the event handler 
                // (as save value that the event handler relies on maybe empty)

            });

        }]);