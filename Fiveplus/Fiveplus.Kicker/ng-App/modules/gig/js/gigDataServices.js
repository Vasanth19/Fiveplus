"use strict";
var dataServiceModule = angular.module("gigModule");        //Retrieve the existing module

dataServiceModule.factory("gigDataService", function ($http, $q) {
 
    var _topics = [];
    var _isInit = false;

    var _isReady = function() {
        return _isInit;
    };

   var _getCategories = function () {
        var _categories = [];

        var deferred = $q.defer();

        $http.get("http://localhost:12000/data/categories")
            .then(function (result) {
                //Success
                angular.copy(result.data, _categories);
                _isInit = true;
                deferred.resolve(_categories);
            },
                function () {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };

    var _getGigs = function () {
        var _gigs = [];

        var deferred = $q.defer();

        $http.get("/api/gig3?completeGraph=true")
            .then(function (result) {
                //Success
                angular.copy(result.data, _gigs);
                _isInit = true;
                deferred.resolve(_gigs);
            },
                function () {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };


    var _addGig = function (newGig) {

        var deferred = $q.defer();

        $http.post("/api/gig", newGig)
            .then(function (result) {
                var newlyCreatedGig = result.data;
                deferred.resolve(newlyCreatedGig);
            }, function (result) {
                deferred.reject(result);
            });

        return deferred.promise;
    };
    
    return {
        topics: _topics,
        isReady: _isReady,
        getCategories : _getCategories,
        addGig: _addGig,
        getGigs:_getGigs
    };

});
