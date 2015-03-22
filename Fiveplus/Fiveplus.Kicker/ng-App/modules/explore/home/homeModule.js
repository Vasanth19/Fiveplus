/// <reference path="../data/Gigs.js" />
//1. Make controllers a object instrad of function for minimizing to work. Refer Shawn wildermuth Course 8.11 minification.


(function () {
    'use strict';

    var dependencies =
  [
      // Angular modules 
      'ngAnimate', // animations
      'ngRoute', // routing
      'ngSanitize', // sanitizes html bindings (ex: sidebar.js)
      'ngMessages',
      'ui.router',

      // Custom modules 
      'common', // common functions, logger, spinner
      'common.bootstrap', // bootstrap dialog wrapper functions


      // 3rd Party Modules
      'breeze.angular', // configures breeze for an angular app
      'ui.bootstrap', // ui-bootstrap (ex: carousel, pagination, dialog)
      "myDataService",
      "myDirectives",
      "myFilters",
      'fiveplus.data'
  ];

    var homeModule = angular.module(globalVars.moduleId);




    var popularGigsCtrlId = 'popularGigsCtrl';
    homeModule.controller(popularGigsCtrlId, ['common', 'datacontext', popularGigsCtrl]);

    function popularGigsCtrl(common, datacontext) {

        var vm = this;
        //Breeze variables
        vm.userdetail = {};
        vm.mybio = 'my bio';
        vm.disableEdit = true;

        vm.showForm = false;

        activate();

        function activate() {

            common.activateController([], popularGigsCtrlId).then(function () {
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

 

})();


