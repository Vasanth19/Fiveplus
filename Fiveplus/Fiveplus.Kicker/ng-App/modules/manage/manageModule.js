/// <reference path="../data/Gigs.js" />
/// <reference path="~/assets/plugins/angular/angular.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.

(function () {
    'use strict';

    var manageModule = angular.module(globalVars.moduleId);



    var sidebarControllerId = 'sidebarController';
    manageModule.controller(sidebarControllerId, sidebarController);

    sidebarController.$inject = ['$scope'];
    function sidebarController($scope) {

            var vm = this;

             //#region ngFLow

            vm.uploader = {}; //Holds the flow object

            vm.flowTarget = function () {
                return {
                    "target": '/api/upload/user',
                    "singleFile": "true"
                };
            };

            vm.flowSuccess = function (message) {
                //Add it to media url
                //console.log(message);
            };

        //#endregion


            vm.title = 'sideBar';

            activate();

            function activate() { }

}
 


var indexControllerId = 'indexController';
manageModule.controller(indexControllerId,
 function ($rootScope, $scope, $modal, $log, $state, $route, $location, $window, $timeout, dataService, common, moduleConfigService, datacontext) {
    
    var log = common.logger.getLogFn(indexControllerId);
    
    var vm = this;
    var shell = $scope.$parent;

    vm.profileConfig = {};


    vm.status = {
        success:false,
        error:false
    };
   

    vm.tabs = [
      { title: "Profile", route: "#profile", active: false },
      { title: "Security", route: "#security", active: false },
      //{ title: "Payment", route: "#payment", active: false },
      { title: "Preferences", route: "#preferences", active: false }
    ];

    // Set the Active property on initialization
    vm.tabs.forEach(function (entry) {
        if ($location.path().substring(1) == entry.route.substring(1)) {
            entry.active = true;
        } else {
            entry.active = false;
        }
    });
    
    //Show how to get the value of element in controller for ASP.net
    vm.requestVerificationToken = angular.element(".tokenHolder").attr('RequestVerificationToken');


    activate();

    function activate() {

        console.log("moduleId = " + moduleConfigService.moduleId);
        var promises = [getBasicProfile()];
        common.activateController(promises, "indexController")
            .then(function ()
            {
                if (datacontext.lookup.cachedLookups.profileConfig == undefined) {
                    $timeout(function () {
                        vm.profileConfig = datacontext.lookup.cachedLookups.profileConfig;
                    });
                } else {
                    vm.profileConfig = datacontext.lookup.cachedLookups.profileConfig;
                }
                log('Activated index controller');

            });

    }

    function getBasicProfile() {
       return  dataService.getBasicProfile()
       .then(function (basicProfile)
       {
           //Success
           return vm.basicProfile = $scope.sharedData.basicProfile =  basicProfile;
       }, function ()
       { //Error
           console.log("Error Occured while fetching basicProfile");
       });
    }
    
    vm.cancelForm = function ()
    {
        datacontext.rejectChanges();
        $timeout(function () {
            common.$broadcast('hide-errors-event');
        });
    };

    vm.saveChanges = function () {
        datacontext.saveChanges();
        vm.status.success = true;
    };
 

    $scope.$watch('vm.profileConfig.twoFactor', function () {
        if (angular.isDefined(vm.profileConfig.twoFactor)) {
            dataService.setTFA(vm.profileConfig.twoFactor);
        }
    });

    $scope.$watch('vm.profileConfig.browserRemembered', function ()
    {
        if (angular.isDefined(vm.profileConfig.browserRemembered)) {
            dataService.setBrowser(vm.profileConfig.browserRemembered);
        }
    });


});



 var profileControllerId = 'profileController';
 manageModule.controller(profileControllerId, ['common', 'datacontext',profileController]);
 
 function profileController(common, datacontext) {

     var vm = this;
     //Breeze variables
     vm.userdetail = {};
     vm.mybio = 'my bio';
     vm.disableEdit = true;

     vm.showForm = false;
    
     activate();

     function activate() {

         common.activateController([getUserDetail()], profileControllerId).then(function () {
             vm.newUserName = vm.userdetail.user.userName;
             vm.mybio = vm.userdetail.biography;
         });
     }

     function getUserDetail() {
         return datacontext.userdetail.getAll()
           .then(function (data) {
               //Success
               return vm.userdetail = data[0];
           });
     }

 }



 var securityControllerId = 'securityController';
 manageModule.controller(securityControllerId, ['common', 'datacontext', securityController]);

 function securityController(common, datacontext) {

     var log = common.logger.getLogFn(securityControllerId);

     var vm = this;
     vm.basicSecurity = {};
     //Breeze variables
     vm.userdetail = {};
     vm.user = {};
     vm.errors = [];


     activate();

     function activate() {
         common.activateController([getUserDetail()], securityControllerId).then(function () {
             vm.user = vm.userdetail.user;
         });
     }

     vm.submitPassword = function (isValid) {
         if (isValid) {
             if (vm.user.passwordHash == '') {
                 setPassword();
             } else {
                 changePassword();
             }

         }

         function setPassword() {
             datacontext.userdetail.setPassword(vm.basicSecurity).then(
                             _success,
                             _failed);
         }

         function changePassword() {
             datacontext.userdetail.changePassword(vm.basicSecurity).then(
                             _success,
                             _failed);
         }

       
     };

     function _success(result) {
         log('Data Saved Successfully');
     }

     function _failed(result) {
         vm.errors = result.data.errors;
     }

     function getUserDetail() {
         return datacontext.userdetail.getAll()
           .then(function (data) {
               //Success
               return vm.userdetail = data[0];

           });
     }

 }




})();


