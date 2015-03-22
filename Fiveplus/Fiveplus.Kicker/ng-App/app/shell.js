(function () { 
    'use strict';
    
    var controllerId = 'shellCtrl';

    angular.module(globalVars.moduleId).controller(controllerId,
        ['$rootScope', '$timeout', '$scope', 'common', 'config', shell]);

    function shell($rootScope, $timeout,$scope, common, config) {
        var vm = this;
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var events = config.events;
        vm.busyMessage = 'Please wait..."';
        vm.isBusy = false;

        $scope.sharedData = {};

        vm.spinnerOptions = {
            lines: 11, // The number of lines to draw
            length: 10, // The length of each line
            width: 5, // The line thickness
            radius: 20, // The radius of the inner circle
            corners: 0.5, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            direction: 1, // 1: clockwise, -1: counterclockwise
            color: '#18ba9b', // #rgb or #rrggbb
            speed: 1.0, // Rounds per second
            trail: 20, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            left: '40%',
            top:'40%'
        };

        activate();

        function activate() {
            $timeout(function() {

            }, 5000);
            logSuccess('Application Started! Loading data', null, true);
            common.activateController([], controllerId);
        }

        function toggleSpinner(on) { vm.isBusy = on; }

        $rootScope.$on('$stateChangeStart',
           function (event, next, current) { toggleSpinner(true); }
       );

        $rootScope.$on('$routeChangeStart',
            function (event, next, current) { toggleSpinner(true); }
        );
        
        $rootScope.$on(events.controllerActivateSuccess,
            function (data) { toggleSpinner(false); }
        );

        $rootScope.$on(events.spinnerToggle,
            function (data) { toggleSpinner(data.show); }
        );
    };
})();