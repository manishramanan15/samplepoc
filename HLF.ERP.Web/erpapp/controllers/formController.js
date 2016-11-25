(function () {
    'use strict';


    app.controller('formController', ['$scope', '$location', '$timeout', 'odataService', function ($scope, $location, $timeout, odataService) {

        odataService.getProducts().then(function (results) {
            //console.log(results.data.value);
            $scope.autodata = results.data.value;
            //$scope.$broadcast('makeautofill');


        }, function (error) {
            //alert(error.data.message);
        });


    }]);


})();
