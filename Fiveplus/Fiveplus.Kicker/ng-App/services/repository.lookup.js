(function () {
    'use strict';

    var serviceId = 'repository.lookup';
    angular
        .module('fiveplus.data')
        .factory(serviceId, RepositoryLookup);

    RepositoryLookup.$inject = ['$http','repository.abstract'];

    function RepositoryLookup($http,AbstractRepository) {
        var entityName = 'lookup';
        var EntityQuery = breeze.EntityQuery;
        var cachedLookups = {};

        //called by datacontext
        function Ctor(mgr) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.manager = mgr;
            //Exposed data access functions
            this.getAll = getAll;
            this.data = data;
            this.cachedLookups = cachedLookups;

        }

        AbstractRepository.extend(Ctor);
        return Ctor;

        function getAll() {
            var self = this;
            return $http.get("/manage/profileconfig").then(function (result) {
                return cachedLookups.profileConfig = result.data;
            });
          
        }


        //Serving data as a promise
        function data() {
            return this.$q.when(cachedLookups);
        }

       
    }
})();