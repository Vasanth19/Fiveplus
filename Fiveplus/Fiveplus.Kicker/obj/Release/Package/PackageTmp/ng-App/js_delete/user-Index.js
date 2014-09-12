/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

var userIndexModule = angular.module("userIndex", ["ngRoute", , "myDataService", "myDirectives"]);


userIndexModule.config(function($routeProvider) {
    //  $routeProvider.when("/", { controller: "topicsController", templateUrl: "/templates/topicsView.html" });
    $routeProvider.when("/addcomment", { controller: "addCommentController", templateUrl: "/templates/user/addComment.html" });
    $routeProvider.when("/message/:id", { controller: "singleTopicController", templateUrl: "/templates/singleTopicView.html" });

    $routeProvider.otherwise({ redirectTo: "/" });
});


function indexController($scope, $http, dataService) {
    $scope.categories = [];

    dataService.getCategories()
        .then(function (categories) {
            //Success
            $scope.categories = categories;
        }, function () { //Error
        console.log("Error Occured while fetching categories");
        });

    $scope.GetResultsByCategory = function (subcategory) {
        console.log("Selected " + subcategory);
        //Get new results based on subcategory
    };
 
}