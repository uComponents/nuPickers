// controller used by datatype editor

angular
    .module("umbraco")
    .controller("nuComponents.DataTypes.Shared.DropDownList.DropDownListEditorController",
        ['$scope', 'nuComponents.DataTypes.Shared.DropDownList.DropDownListResource',
        function ($scope, dropDownListResource) {

            /*
                expects to find:
                    $scope.model.condig.apiController = "XmlDropDownListApi" 
            */

            dropDownListResource.getEditorOptions($scope.model.config).then(function (response) {
                $scope.dropDownListOptions = response.data; // [{"key":"","markup":""},{"key":"","markup":""}...]
            });

        }]);