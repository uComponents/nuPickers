
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
    .directive('nuEnterKey', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                element.bind('keydown keypress', function (event) {
                    if (event.which === 13) {
                        scope.$apply(attrs.nuCursorDown);
                        event.preventDefault();
                    }
                });
            }
        }
    });
