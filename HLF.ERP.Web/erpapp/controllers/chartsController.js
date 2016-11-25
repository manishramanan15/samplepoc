(function () {
    'use strict';

    
    app.controller('chartsController', ['$scope', '$location', 'authService',function ($scope, $location, authService) {

        $scope.logOut = function () {
            authService.logOut();
            $location.path('/index');
        }

        $scope.authentication = authService.authentication();

    }]);


})();
