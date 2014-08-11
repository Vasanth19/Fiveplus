/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

var gigIndexModule = angular.module("gigIndex", ["ngRoute", , "myDataService", "myDirectives","myFilters"]);


gigIndexModule.config(function($routeProvider) {
     $routeProvider.when("/", { controller: "indexController", templateUrl: "ng-App/templates/indexView.html" });
     $routeProvider.when("/newgig", { controller: "newGigController", templateUrl: "ng-App/templates/newGigView.html" });
     $routeProvider.when("/message/:id", { controller: "singleTopicController", templateUrl: "ng-App/templates/singleTopicView.html" });

    $routeProvider.otherwise({ redirectTo: "/" });
});


function indexController($scope, $http, dataService) {
    //http://jsoneditoronline.org/

    
    $scope.isBusy = true;
    $scope.gigs = [];

    dataService.getGigs()
   .then(function (_gigs) {
       //Success
       $scope.gigs = _gigs;
       console.log(_gigs);
   }, function () { //Error
       console.log("Error Occured while fetching Gigs");

   });
    


}


function newGigController($scope, $http, dataService) {
    //http://jsoneditoronline.org/


    $scope.isBusy = true;

    $scope.newGig = {};


    $scope.saveGig = function () {
        dataService.addGig($scope.newGig).then(function (newlyCreatedGig) {
            //Success
            console.log("gig Saved");
            console.log(newlyCreatedGig);
        }, function () {
            //Error
            console.log("Error Occured while saving gig");
        });


    };

    $scope.GetResultsByCategory = function (subcategory) {
        console.log("Selected " + subcategory);
        //Get new results based on subcategory
    };

}