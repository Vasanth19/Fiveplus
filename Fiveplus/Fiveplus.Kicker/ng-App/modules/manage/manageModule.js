/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

var manageModule = angular.module("manageModule", ["ui.router", "ngRoute", "ngSanitize",
    "myDataService", "myDirectives", "myFilters", "flow", "field-directive", 'ngAutocomplete']);


manageModule.config(function ($routeProvider,$stateProvider) {
   
    $routeProvider.when("/profile", { controller: "dummyController", templateUrl: "/ng-App/modules/manage/subPages/profile.html" });
    $routeProvider.when("/security", { controller: "dummyController", templateUrl: "/ng-App/modules/manage/subPages/security.html" });
    $routeProvider.when("/payment", { controller: "dummyController", templateUrl: "/ng-App/modules/manage/subPages/payment.html" });
    $routeProvider.when("/preferences", { controller: "dummyController", templateUrl: "/ng-App/modules/manage/subPages/preferences.html" });

  
    //$stateProvider
    //    .state("profile", { url: "/profile", templateUrl: "/ng-App/modules/manage/subPages/profile.html" })
    //    .state("security", { url: "/security", templateUrl: "/ng-App/modules/manage/subPages/security.html" })
    //    .state("payment", { url: "/payment", templateUrl: "/ng-App/modules/manage/subPages/payment.html" })
    //    .state("preferences", { url: "/preferences", templateUrl: "/ng-App/modules/manage/subPages/preferences.html" });

    $routeProvider.otherwise("/profile");
    $routeProvider.otherwise({ redirectTo: "/profile" });

});



manageModule.controller("indexController", function ($rootScope, $scope, $state, $route, $location, $window, $timeout, dataService, antiForgeryService)
{

    $scope.requestVerificationToken = angular.element(".tokenHolder").attr('RequestVerificationToken');

    $scope.result1 = '';
    $scope.options1 = null;
    $scope.details1 = '';
    console.log(antiForgeryService.token);

    $scope.profileConfig = {};
    dataService.getProfileConfig()
        .then(function(profileConfig) {
            //Success
            $scope.profileConfig = profileConfig;
        }, function() { //Error
            console.log("Error Occured while fetching profileConfig");
        });


    $scope.uploader = {}; //Holds the flow object

    $scope.flowTarget = function() {
        return {
            "target": '/api/upload/user',
            "singleFile": "true"
        };
    };

    $scope.flowSuccess = function(message) {
        //Add it to media url
        console.log(message);
    };

    $scope.tabs = [
        { title: "Profile", route: "#profile", active: false },
        { title: "Security", route: "#security", active: false },
        { title: "Payment", route: "#payment", active: false },
        { title: "Preferences", route: "#preferences", active: false }
    ];

    $scope.tabs.forEach(function(entry)
    {
        if ($location.path().substring(1) == entry.route.substring(1)) {
            entry.active = true;
        } else {
            entry.active = false;
        }
    });


    $scope.basicProfile = {};
    dataService.getBasicProfile()
       .then(function (basicProfile)
       {
           //Success
           $scope.basicProfile = basicProfile;
       }, function ()
       { //Error
           console.log("Error Occured while fetching basicProfile");
       });

    $scope.submitForm = function (isValid)
    {
        $scope.showForm = false;
        console.log(isValid);
    };

    $scope.submitSetPassword = function (isValid) {
        console.log($scope.requestVerificationToken);
        if (isValid) {

            dataService.setPassword($scope.basicSecurity,$scope.requestVerificationToken). //global variable
            then(function (result)
            {
                            console.log(result);
             }, function(result) {
                console.log(result);
            });
        }
    };


    $scope.resetForm = function ()
    {
        $timeout(function ()
        {
            $scope.$broadcast('hide-errors-event');
        });
     
    };
    $scope.cancelForm = function ()
    {
        $scope.showForm = false;
    
       // $window.history.back();
    };

    $scope.basicSecurity = {};

    $scope.$watch('profileConfig.twoFactor', function () {
        if (angular.isDefined($scope.profileConfig.twoFactor)) {
            dataService.setTFA($scope.profileConfig.twoFactor);
        }
    });

    $scope.$watch('profileConfig.browserRemembered', function ()
    {
        if (angular.isDefined($scope.profileConfig.browserRemembered)) {
            dataService.setBrowser($scope.profileConfig.browserRemembered);
        }
    });



});


manageModule.controller("dummyController", function ($rootScope, $scope, $state, $route, $location, dataService) {
    $scope.$parent.showForm = false;
    //Needed to trigger it when the route changes
    $scope.tabs.forEach(function (entry)
    {
        if ($location.path().substring(1) == entry.route.substring(1)) {
            entry.active = true;
        } else {
            entry.active = false;
        }
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