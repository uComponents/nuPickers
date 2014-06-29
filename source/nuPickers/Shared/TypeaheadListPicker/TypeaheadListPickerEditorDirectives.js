
angular
    .module("umbraco.directives")
    .directive('nuBlur', function () { // ng-blur isn't yet avaiable with the build of AngularJs used by Umbraco
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                element.bind('blur', function () {
                    scope.$apply(attrs.nuBlur);
                });
            }
        }
    });

