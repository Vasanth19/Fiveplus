var dataServiceModule = angular.module("myDataService");

dataServiceModule.factory("dataService", function ($http, $q) {
 
    var _isInit = false;

    var _isReady = function() {
        return _isInit;
    };

   
    var _getProfileConfig = function () {
        var _profileConfig = {};

        var deferred = $q.defer();

        $http.get("/manage/profileconfig")
            .then(function (result) {
                //Success
                angular.copy(result.data, _profileConfig);
                _isInit = true;
                deferred.resolve(_profileConfig);
            },
                function () {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };

    var _getBasicProfile = function ()
    {
        var _basicProfile = {};

        var deferred = $q.defer();

        $http.get("/manage/basicProfile")
            .then(function (result)
            {
                //Success
                angular.copy(result.data, _basicProfile);
                _isInit = true;
                deferred.resolve(_basicProfile);
            },
                function ()
                {
                    deferred.reject();
                }
            );
        return deferred.promise;
    };

    var _setTFA = function (status)
    {
        return $http.post('/manage/TFA/' + status);
     
    };

    var _setBrowser = function (status)
    {
        return $http.post('/manage/rememberBrowser/' + status);

    };


    var _createAccount = function (registervm)
    {
        return $http.post('/account/register', registervm);

    };

    return {
        getProfileConfig: _getProfileConfig,
        getBasicProfile: _getBasicProfile,
        setTFA: _setTFA,
        setBrowser: _setBrowser,
        createAccount: _createAccount
    };

});
