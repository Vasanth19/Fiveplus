(function () {
    'use strict';

    var serviceId = 'repository.userdetail';
    angular
        .module('fiveplus.data')
        .factory(serviceId, RepositoryUserdetail);

    RepositoryUserdetail.$inject = ['$http', 'repository.abstract'];

    function RepositoryUserdetail($http, AbstractRepository) {
        
        var entity = {
            name: 'UserDetails', //Same as in web api method
            type: 'userdetail'  //Same as in metadata
        };
        var EntityQuery = breeze.EntityQuery;


        //called by datacontext
        function Ctor(mgr) {
            this.serviceId = serviceId;
            this.entityName = entity.name;
            this.manager = mgr;
            //Exposed data access functions
            this.getAll = getAll;
            this.setPassword = setPassword;
            this.changePassword = changePassword;


        }

        AbstractRepository.extend(Ctor);
        return Ctor;

        function getAll( forceRemote) {
            var userdetail = {};
            var self = this;

            if (!forceRemote && self._areItemsLoaded()) {
                userdetail = self._getAllLocal(entity.name,'userId');
                return self.$q.when(userdetail);
            }

            return EntityQuery.from(entity.name)
               .expand('user')
              .toType(entity.type)
              .using(self.manager).execute()
              .then(success).catch(self._queryFailed);

            function success(data) {
                userdetail = data.results;
                self._areItemsLoaded(true);
              return userdetail;
            }

    }

        function setPassword(model) {
            return $http.post('/manage/setPassword', model);
        }

        function changePassword(model) {

            return $http.post('/manage/changePassword', model);

            var deferred = this.$q.defer();

            $http.post('/manage/changePassword', model)
                .then(
                function (result) {
                    deferred.resolve(result);
                }, function (result) {
                    deferred.reject(result);
                });

            return deferred.promise;
        }
       

       
    }
})();