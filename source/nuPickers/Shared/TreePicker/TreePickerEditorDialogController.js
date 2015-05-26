angular
    .module("umbraco")
    .controller("nuPickers.Shared.TreePicker.TreePickerEditorDialogController",
    ['$scope', 'nuPickers.Shared.Editor.EditorResource', 'eventsService',
        function ($scope, editorResource, eventsService) {

            $scope.dialogTreeEventHandler = $({});
            $scope.selectedItems = [];

            //wires up selection
            function nodeSelectHandler(ev, args) {
                args.event.preventDefault();
                args.event.stopPropagation();

                eventsService.emit("dialogs.treePickerController.select", args);

                select(args.node);

                args.node.selected = args.node.selected === true ? false : true;
            };

            function select(node) {
                // Grab the properties we want to store
                var nodeDataModel = { key: node.id, label: node.name, icon: node.icon };

                if ($scope.dialogData.config.treePicker.multipicker) {
                    var i = $scope.selectedItems.indexOf(nodeDataModel); // TODO: Map // IDs
                    if (i < 0) {
                        $scope.dialogData.selection.push(nodeDataModel);
                    } else {
                        $scope.dialogData.selection.splice(i, 1);
                    }
                } else {
                    $scope.submit(node);
                }
            };

            $scope.selectMultiple = function (result) {
                $scope.submit($scope.dialogData.selection);
            };

            $scope.dialogTreeEventHandler.bind("treeNodeSelect", nodeSelectHandler);

            $scope.$on('$destroy', function () {
                $scope.dialogTreeEventHandler.unbind("treeNodeSelect", nodeSelectHandler);
            });

        }
    ]);