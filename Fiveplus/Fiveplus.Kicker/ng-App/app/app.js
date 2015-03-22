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


    // Dependency injection by Module
    if (globalVars.moduleId == 'manageModule') {
        ['flow', 'ngAutocomplete'].forEach(function(item) {
            dependencies.push(item);
        });
    }


    var app = angular.module(globalVars.moduleId, dependencies);
    
    // Handle routing errors and success events
    app.run(['$route', '$rootScope', '$q',
        function ($route, $rootScope, $q)
    {
       

    }]);        
})();

