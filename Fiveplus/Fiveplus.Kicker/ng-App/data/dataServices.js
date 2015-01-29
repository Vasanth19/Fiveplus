var dataServiceModule = angular.module("myDataService", []);
dataServiceModule.run(['$http', function ($http)
{
    $http.defaults.headers.common['RequestVerificationToken'] = angular.element(".tokenHolder").attr('RequestVerificationToken');
}]);


dataServiceModule.factory("dataService", function ($http, $q) {
 
    var _topics = [];
    var _isInit = false;

    var _isReady = function() {
        return _isInit;
    };

    var _getTopics = function() {

        var deferred = $q.defer();

        $http.get("http://localhost:56855/api/topics?includeReplies=true")
            .then(function(result) {
                //Success
                angular.copy(result.data, _topics);
                _isInit = true;
                deferred.resolve();
            },
                function() {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };

    var _addTopic = function(newTopic) {

        var deferred = $q.defer();

        $http.post("http://localhost:56855/api/topics/", newTopic)
            .then(function(result) {
                var newlyCreatedTopic = result.data;
                _topics.splice(0, 0, newlyCreatedTopic);
                deferred.resolve(newlyCreatedTopic);

            }, function() {
                deferred.reject();
            });

        return deferred.promise;
    };

    var _addReply = function(topic, newReply) {

        var deferred = $q.defer();

        $http.post("http://localhost:56855/api/topics/" + topic.id + "/replies", newReply)
            .then(function(result) {

                if (topic.replies == null) topic.replies = [];
                var newlyCreatedReply = result.data;
                topic.replies.push(newlyCreatedReply);
                deferred.resolve(newlyCreatedReply);

            }, function() {
                deferred.reject();
            });

        return deferred.promise;
    };

    var _getTopicById = function(id) {

        var deferred = $q.defer();

        if (_isReady()) {

            var topic = _findTopic(id);

            if (topic) {
                deferred.resolve(topic);
            } else {
                deferred.reject();
            }
        } else {
            _getTopics().then(function() {
                //success
                var topic = _findTopic(id);

                if (topic) {
                    deferred.resolve(topic);
                } else {
                    deferred.reject();
                }

            }, function() {
                //error
                deferred.reject();
            });
        }

        return deferred.promise;
    };

    function _findTopic(id) {
        var found = null;

        $.each(_topics, function(i, item) {
            if (item.id == id) {
                found = item;
                return false; // break out as soon as you find the item.
            }
        });

        return found;
    }


    var _getCategories = function () {
        var _categories = [];

        var deferred = $q.defer();

        $http.get("/api/init/categories")
            .then(function (result) {
                //Success
                angular.copy(result.data, _categories);
                _isInit = true;
                deferred.resolve(_categories);
            },
                function () {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };

    var _getGigs = function () {
        var _gigs = [];

        var deferred = $q.defer();

        $http.get("/api/gig3?completeGraph=true")
            .then(function (result) {
                //Success
                angular.copy(result.data, _gigs);
                _isInit = true;
                deferred.resolve(_gigs);
            },
                function () {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };


    var _addGig = function (newGig) {

        var deferred = $q.defer();

        $http.post("api/gig2", newGig)
            .then(function (result) {
                var newlyCreatedGig = result.data;
                deferred.resolve(newlyCreatedGig);
            }, function () {
                deferred.reject();
            });

        return deferred.promise;
    };
    
    return {
        topics: _topics,
        getTopics: _getTopics,
        addTopic: _addTopic,
        addReply: _addReply,
        isReady: _isReady,
        getTopicById: _getTopicById,
        getCategories : _getCategories,
        addGig: _addGig,
        getGigs:_getGigs

    };

});
