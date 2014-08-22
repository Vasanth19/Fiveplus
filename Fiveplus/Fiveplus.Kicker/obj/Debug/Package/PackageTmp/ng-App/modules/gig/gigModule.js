/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

var gigModule = angular.module("gigModule", ["ngRoute", "myDataService", "myDirectives", "myFilters", "mgo-angular-wizard",
                                             "field-directive", "localytics.directives", "textAngular", "flow"]);


gigModule.config(function($routeProvider) {
    $routeProvider.when("/", { controller: "gigIndexController", templateUrl: "/ng-App/modules/gig/subPages/indexView.html" });
    $routeProvider.when("/create", { controller: "gigCreateController", templateUrl: "/ng-App/modules/gig/subPages/createGig.html" });
    $routeProvider.when("/message/:id", { controller: "singleTopicController", templateUrl: "/templates/singleTopicView.html" });

    $routeProvider.otherwise({ redirectTo: "/" });
});


function gigIndexController($scope, $http, dataService) {
    //http://jsoneditoronline.org/

    
    $scope.filterSubcategory = "";
    $scope.isBusy = true;
    $scope.i = 0;
    $scope.data = "I am GIG Awesome";
    $scope.someHtml = '<img src="http://angularjs.org/img/AngularJS-large.png" />';
    $scope.gigs = [];

    $scope.images = [1, 2, 3, 4, 5, 6, 7, 8];

    $scope.loadMore = function() {
        var last1 = $scope.images[$scope.images.length - 1];
        for (var i = 1; i <= 8; i++) {
            $scope.images.push(last1 + i);
        }
    };

    $scope.addMoreGigs = function () {
        $scope.i = $scope.i + 1;
        var last = {
            "title": $scope.i + "$ake a professional quality photo of a phrase or name spelt in...",
            "title_full": "take a professional quality photo of a phrase or name spelt in Scrabble tiles",
            "gig_id": 12354,
            "gig_url": "/ceppii/take-a-professional-quality-photo-of-a-phrase-or-name-spelt-in-scrabble-tiles",
            "img_medium": "<img src=\"http://cdn1.fiverrcdn.com/photos/327941/v2_162/small3.jpg?1368531638\"  alt=\"take a professional quality photo of a phrase or name spelt in Scrabble tiles\"   >",
            "video_thumb": false,
            "seller_name": "ceppii",
            "seller_img": "<img src=\"http://cdn1.fiverrcdn.com/photos/104001/thumb/springbreak2.jpg?1339889643\"    width=\"32\" height=\"32\">",
            "seller_created_at": "over 3 years",
            "seller_country_name": "United States",
            "seller_country": "us",
            "seller_url": "/ceppii",
            "seller_level": "level_two_seller",
            "gig_locale": "en",
            "seller_id": 2324
        };
        $scope.gigs.push({
            "title": $scope.i + "$ake a professional quality photo of a phrase or name spelt in...",
            "title_full": "take a professional quality photo of a phrase or name spelt in Scrabble tiles",
            "gig_id": 12354,
            "gig_url": "/ceppii/take-a-professional-quality-photo-of-a-phrase-or-name-spelt-in-scrabble-tiles",
            "img_medium": "<img src=\"http://cdn1.fiverrcdn.com/photos/327941/v2_162/small3.jpg?1368531638\"  alt=\"take a professional quality photo of a phrase or name spelt in Scrabble tiles\"   >",
            "video_thumb": false,
            "seller_name": "ceppii",
            "seller_img": "<img src=\"http://cdn1.fiverrcdn.com/photos/104001/thumb/springbreak2.jpg?1339889643\"    width=\"32\" height=\"32\">",
            "seller_created_at": "over 3 years",
            "seller_country_name": "United States",
            "seller_country": "us",
            "seller_url": "/ceppii",
            "seller_level": "level_two_seller",
            "gig_locale": "en"
        });

       // $scope.gigs.splice(0,0,last);

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

 

    $scope.saveGig = function () {
        var newGig = {
            "title": "take a professional quality photo of a phrase or name spelt in...",
            "title_full": "take a professional quality photo of a phrase or name spelt in Scrabble tiles",
            "duration": 5,
            "price": "$5",
            "rating": 9,
            "rating_count": 255,
            "is_featured": true,
            "gig_id": 327941,
            "gig_url": "/ceppii/take-a-professional-quality-photo-of-a-phrase-or-name-spelt-in-scrabble-tiles",
            "img_medium": "<img src=\"http://cdn1.fiverrcdn.com/photos/327941/v2_162/small3.jpg?1368531638\"  alt=\"take a professional quality photo of a phrase or name spelt in Scrabble tiles\"   >",
            "video_thumb": false,
            "seller_name": "ceppii",
            "seller_img": "<img src=\"http://cdn1.fiverrcdn.com/photos/104001/thumb/springbreak2.jpg?1339889643\"    width=\"32\" height=\"32\">",
            "seller_created_at": "over 3 years",
            "seller_country_name": "United States",
            "seller_country": "us",
            "seller_url": "/ceppii",
            "seller_level": "level_two_seller",
            "gig_locale": "en",
            "seller_id": 103826
        };

        dataService.addGig(newGig).then(function (newlyCreatedGig) {
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

function gigCreateController($scope, $http, dataService, WizardHandler) {
    $scope.finished = function() {
        alert("Wizard finished :)");
    };

    $scope.logStep = function() {
        console.log("Step continued");
    };

    $scope.goBack = function() {
        WizardHandler.wizard().goTo(0);
    };

    $scope.applications = ["Hi", "Hello"];
}