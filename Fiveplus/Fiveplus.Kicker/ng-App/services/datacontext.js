(function () {
    'use strict';

    var app = angular.module('fiveplus.data', [
        // Angular modules 
        'ngAnimate',
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules

    ]);

    app.run(['$http', function ($http) {
        $http.defaults.headers.common['RequestVerificationToken'] = angular.element(".tokenHolder").attr('RequestVerificationToken');
    }]);


    var serviceId = 'datacontext';
    app.factory(serviceId, datacontext);

    datacontext.$inject = ['config', 'common', 'entityManagerFactory', 'repositories'];

    function datacontext(config, common, emFactory, repositories) {
        var $q = common.$q;
        var log = common.logger.getLogFn(serviceId);
        var logSuccess = common.logger.getLogFn(serviceId, 'success');
        var logError = common.logger.getLogFn(serviceId, 'error');
        var primePromise;
        var repoNames = ['lookup', 'userdetail'];
        var manager = emFactory.newManager();

        var service = {
            prime: prime,
            saveChanges: saveChanges,
            rejectChanges: rejectChanges
            // Repositories to be added on demand
        };

        init();

        return service;



        function init() {
            repositories.init(manager);
            defineLazyLoadedRepos();
        };

        //add ES5 property to datacontext.
        function defineLazyLoadedRepos() {
            repoNames.forEach(function (name) {
                Object.defineProperty(service, name, {
                    configurable: true,
                    get: function () {
                        var repo = repositories.getRepo(name);
                        Object.defineProperty(service, name, {
                            value: repo,
                            configurable: false,
                            enumerable: true
                        });
                        return repo;
                    }
                });
            });

        }

        function prime() {
            if (primePromise) return primePromise;

            primePromise = $q.all([service.lookup.getAll()]).then(success);
            return primePromise;

            function success() {
                log("getAll suceeded");

            }
        };


        function saveChanges() //global save
        {
            manager.saveChanges();
        }

        function rejectChanges() //global reject
        {
            manager.rejectChanges();
        }

    }


})();