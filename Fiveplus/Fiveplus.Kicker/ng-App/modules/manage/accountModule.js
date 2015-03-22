/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

(function () {
    'use strict';

    var accountModule = angular.module(globalVars.moduleId);  //manageModule


accountModule.controller("externalLoginController", function ($rootScope, $scope, $route, $location, $window, $timeout, dataService)
{
    $scope.result1 = '';
    $scope.options1 = null;
    $scope.details1 = '';

});



accountModule.controller("accountController", function ($rootScope, $scope, $route, $location, $window, $timeout, dataService)
{
    $scope.result1 = '';
    $scope.options1 = null;
    $scope.details1 = '';
    $scope.registervm = {};

    $scope.createAccount = function (isValid)
    {
        if (!isValid) return;

        dataService.createAccount($scope.registervm). //global variable
          then(function (result) {
          //  $location.path('/home');
             $window.location.href = "/";     // this works
              console.log(result);
          }, function (result)
          {
              console.log(result);
          });

    };

    $scope.locationTest = function() {
        $location.path('home');
    };


});



})();


