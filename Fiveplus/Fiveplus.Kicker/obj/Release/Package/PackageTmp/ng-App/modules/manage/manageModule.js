/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

var manageModule = angular.module("manageModule", ["ui.router", "ngRoute", "ngSanitize", "myDataService", "myDirectives", "myFilters"]);


manageModule.config(function ($routeProvider,$stateProvider) {
   
  
    $stateProvider
        .state("tab1", { url: "/profile", templateUrl: "/ng-App/modules/manage/subPages/profile.html" })
        .state("tab2", { url: "/profile", templateUrl: "/ng-App/modules/manage/subPages/profile.html" })
        .state("tab3", { url: "/profile", templateUrl: "/ng-App/modules/manage/subPages/profile.html" });

    $routeProvider.otherwise({ redirectTo: "/" });

});



manageModule.controller("indexController", function ($rootScope, $scope, $state) {

    $scope.go = function (route) {
        console.log($state);
        console.log(route);
        $state.go(route);
    };

    $scope.active = function(route){
        return $state.is(route);
    };
    
    $scope.tabs = [
        { heading: "Tab 1", route:"tab1", active:false },
        { heading: "Tab 2", route:"tab2", active:false },
        { heading: "Tab 3", route:"tab3", active:false }
    ];

    $scope.$on("$stateChangeSuccess", function() {
        $scope.tabs.forEach(function(tab) {
            tab.active = $scope.active(tab.route);
        });
    });
});


function index1Controller($scope, $http, dataService) {
    //http://jsoneditoronline.org/

    
    $scope.filterSubcategory = "";
    $scope.isBusy = true;
    $scope.i = 0;
    $scope.data = "I am Awesome";
    $scope.someHtml = '<img src="http://angularjs.org/img/AngularJS-large.png" />';
    $scope.gigs = [];

    $scope.images = [1, 2, 3, 4, 5, 6, 7, 8];

    $scope.loadMore = function() {
        var last1 = $scope.images[$scope.images.length - 1];
        for (var i = 1; i <= 8; i++) {
            $scope.images.push(last1 + i);
        }
    };

  
    $scope.categories = [];

    dataService.getCategories()
        .then(function (categories) {
            //Success
            $scope.categories = categories;
        }, function () { //Error
        console.log("Error Occured while fetching categories");
        });

    dataService.getGigs()
    .then(function (_gigs) {
        //Success
        $scope.gigs = _gigs;
        console.log(_gigs);
    }, function () { //Error
        console.log("Error Occured while fetching Gigs");

    });

 
    $scope.GetResultsByCategory = function (subcategory) {
        console.log("Selected " + subcategory);
        //Get new results based on subcategory
    };

}