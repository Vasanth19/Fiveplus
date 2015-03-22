(function() {
    'use strict';
    var app = angular.module('myDirectives');
    
    // Usage:
    //     <cc-input label="Base Salary" model="vm.compensation.baseSalary"></cc-input>
    // Creates:
    // 
    app.directive('ccInput', ccInput);
    ccInput.$inject = ['$window'];
    function ccInput($window) {
       
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                model: '=',
                disabled: '='
            },
            templateUrl: 'ng-App/directives/templates/ccInput.html'
        };
        return directive;

        function link(scope, el, attrs) {
            var label = attrs['label'];
            el.find('.label').html(label);
        }
    }

    // Usage:
    //        <cc-label name="Base Salary" value="{{item.baseSalary}}"></cc-label>
    // Creates:
    // 
    app.directive('ccLabel', ccLabel);
    ccLabel.$inject = ['$window'];
    function ccLabel($window) {

        var directive = {
            restrict: 'E',
            scope: {
                name: '@',
                value: '@'
            },
            templateUrl: 'ng-App/directives/templates/ccLabel.html'
        };
        return directive;

    }


    app.directive('ccBreadcrumbs', ccBreadcrumbs);
    ccBreadcrumbs.$inject = ['$window'];
    function ccBreadcrumbs($window) {

        var directive = {
            restrict: 'E',
            scope: {
                activelink: '@'
            },
            templateUrl: 'ng-App/directives/templates/ccBreadcrumbs.html'
        };
        return directive;

    }

    app.directive('ccSpinnerWrapper', ccSpinnerWrapper);
    ccSpinnerWrapper.$inject = ['$window'];
    function ccSpinnerWrapper($window) {

        var directive = {
            restrict: 'E',
            templateUrl: 'ng-App/directives/templates/ccSpinnerWrapper.html'
        };
        return directive;

    }


})();