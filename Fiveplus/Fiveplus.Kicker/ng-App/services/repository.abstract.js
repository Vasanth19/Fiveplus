(function () {
    'use strict';

    var serviceId = 'repository.abstract';
    angular
        .module('fiveplus.data')
        .factory(serviceId, RepositoryAbstract);

    RepositoryAbstract.$inject = ['common'];

    function RepositoryAbstract(common) {
        var EntityQuery = breeze.EntityQuery;
        var logError = common.logger.getLogFn(this.serviceId, 'error');


        function Ctor() {
            this.isLoaded = false;
        }

        Ctor.extend = function(repoCtor) {
            repoCtor.prototype = new Ctor();
            repoCtor.prototype.constructor = repoCtor;
         };

        // SHARED by repositoy classes
        Ctor.prototype._areItemsLoaded = _areItemsLoaded;
        Ctor.prototype._queryFailed = _queryFailed;
        // Breeze Helper functions
        Ctor.prototype._getAllLocal = _getAllLocal;
        Ctor.prototype._getFilteredLocal = _getFilteredLocal;
        Ctor.prototype._getLocalEntityCount = _getLocalEntityCount;
        Ctor.prototype._getInlineCount = _getInlineCount;
        //convienence  functions
        Ctor.prototype.log = common.logger.getLogFn(this.serviceId);
        Ctor.prototype.$q = common.$q;

        return Ctor;

        function _areItemsLoaded(value) {
            if (value === undefined) {
                return this.isLoaded;
            }
            return this.isLoaded = value;
        }

        function _getAllLocal(resource, ordering) {
            return EntityQuery.from(resource)
            .orderBy(ordering)
            .using(this.manager)
            .executeLocally();
        }

        function _getFilteredLocal(resource, predicate, ordering) {
            return EntityQuery.from(resource)
            .where(predicate)
            .orderBy(ordering)
            .using(this.manager)
            .executeLocally();
        }

        function _getLocalEntityCount(resource) {
            return EntityQuery.from(resource)
            .using(this.manager)
            .executeLocally()
            .length;
        }

        function _getInlineCount(data) {
            return data.inlineCount;
        }

        function _queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error Retriving data ' + error.message;
            logError(msg);
            throw error;
        }    
    }
})();