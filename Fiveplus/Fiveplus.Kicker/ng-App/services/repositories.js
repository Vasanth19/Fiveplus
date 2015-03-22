(function () {
    'use strict';

    var serviceId = 'repositories';
    angular
        .module('fiveplus.data')
        .factory(serviceId, repositories);

    repositories.$inject = ['$injector'];

    function repositories($injector) {
        var manager;

        var service = {
            getRepo: getRepo,
            init:init
        };

        return service;

        //called by datacontext
        function init(mgr) { manager = mgr; }

        //get named repository Ctor
        function getRepo(repoName) {
            var fullRepoName = 'repository.' + repoName.toLowerCase();
            var Repo = $injector.get(fullRepoName);
            return new Repo(manager);
        }

    }
})();