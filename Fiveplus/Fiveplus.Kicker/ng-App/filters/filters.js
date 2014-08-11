/// <reference path="../data/Gigs.js" />
/// <reference path="directives.js" />
/// <reference path="../_partials/hmGigItem.html" />
var myFilters = angular.module("myFilters", []);


myFilters.filter('to_trusted', [
    '$sce', function ($sce) {
        return function (text) {
            return $sce.trustAsHtml(text);
        };
    }
]);

myFilters.filter('shortRating', function () {
    return function(input) {

        if (String(input).length > 3) {
            return String(input).substring(0, 1) + "K+";
        } else {
            return input;
        }
    };
});


