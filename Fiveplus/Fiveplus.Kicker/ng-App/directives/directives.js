/// <reference path="../data/Gigs.js" />
/// <reference path="directives.js" />
/// <reference path="../_partials/hmGigItem.html" />
var myDirectives = angular.module("myDirectives", []);

myDirectives.directive('hmGigItem', function() {
    return {
        restrict: "E",
        replace: true,
        scope: {
            gig: '='
        },
        templateUrl: "/ng-App/directives/_html/hmGigItem.html",
        controller: function($scope) {
            //Variables
            $scope.displayProps = [];
            $scope.displayProps.favorite = false;

            //http://jsfiddle.net/EQmSN/ for more zooming
            $(".zoomfancybox").fancybox({
                arrows: false,
                openEffect: 'elastic',
                openSpeed: 150,
                closeEffect: 'elastic',
                closeSpeed: 150,
                closeClick: true
            });

            //Methods
           
        }
    };
});


myDirectives.directive('hmCategorySearch', function () {
    return {
        restrict: "E",
        replace: true,
        scope: {
            categories: '=',
            selected: '&'
        },
        templateUrl: "/ng-App/directives/_html/hmCategorySearch.html",
        controller: function ($scope) {
            //Variables

            //Methods

        }
    };
});