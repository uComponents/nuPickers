
angular
    .module("umbraco.directives")
    .directive('nuCursorUp', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                element.bind('keydown keypress', function (event) {
                    if (event.which === 38) {
                        scope.$apply(attrs.nuCursorUp);
                        event.preventDefault();
                    }
                });
            }
        }
    });


angular
    .module("umbraco.directives")
    .directive('nuCursorDown', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                element.bind('keydown keypress', function (event) {
                    if (event.which === 40) {
                        scope.$apply(attrs.nuCursorDown);
                        event.preventDefault();
                    }
                });
            }
        }
    });

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

