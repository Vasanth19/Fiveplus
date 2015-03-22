(function ()
{
    'use strict';

    var app = angular.module(globalVars.moduleId);

    // Collect the routes
    app.constant('routes', getRoutes());

    app.constant('routeStates', getRouteStates());

 
    app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', 'routeStates', stateConfigurator]);
    function stateConfigurator($stateProvider, $urlRouterProvider, $locationProvider, routeStates) {

        routeStates.forEach(function (r) {
            r.resolve = angular.extend(r.resolve || {}, { prime: prime }); // add the resolve function to all routes
            $stateProvider.state(r);
        });


        $urlRouterProvider.otherwise('/profile');

        $locationProvider.html5Mode({
            enabled: false,
            requireBase: false
        });

    }


    prime.$inject = ['datacontext'];
    function prime(datacontext) {
        datacontext.prime();
    }


    // Define the routes 
    function getRouteStates() {
        return [
          
             {
                 name: 'profile',
                 url: '/profile',
                 templateUrl: '/ng-App/modules/manage/subPages/profile.html',
                 controller: 'profileController as vm'
                 

             },
             {
                 name: 'security',
                 url: '/security',
                 templateUrl: '/ng-App/modules/manage/subPages/security.html',
                 controller: 'securityController as vm'
                 
             },
              {
                  name: 'payment',
                  url: '/payment',
                  templateUrl: '/ng-App/modules/manage/subPages/payment.html'

              },
               {
                   name: 'preferences',
                   url: '/preferences',
                   templateUrl: '/ng-App/modules/manage/subPages/preferences.html'

               }
        ];
    }

    // Define the routes 
    function getRoutes()
    {
        return [
            {
                url: '/profile',
                config: {
                    controller: "tabController as vmTab", templateUrl: "/ng-App/modules/manage/subPages/profile.html"
                }
            }, {
                url: '/security',
                config: {
                    controller: "tabController as vmTab", templateUrl: "/ng-App/modules/manage/subPages/security.html"
                }
            }, {
                url: '/payment',
                config: {
                    controller: "tabController as vmTab", templateUrl: "/ng-App/modules/manage/subPages/payment.html"
                }
            }, {
                url: '/preferences',
                config: {
                    controller: "tabController as vmTab", templateUrl: "/ng-App/modules/manage/subPages/preferences.html"
                    
                }
            }
        ];
    }
})();