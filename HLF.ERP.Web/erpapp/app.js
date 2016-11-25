
    'use strict';

    var app = angular.module('HLFErpApp', [
        // Angular modules 
        'ngRoute',
        'ngSanitize',
        
        // Custom modules 
        'LocalStorageModule',
        'angular-loading-bar'
        

        // 3rd Party Modules
        
    ]);


    app.config(function ($routeProvider) {

        $routeProvider.when("/index", {
            controller: "indexController",
            templateUrl: "/erpapp/views/index.html"
        });

        $routeProvider.when("/form", {
            controller: "formController",
            templateUrl: "/erpapp/views/form.html"
        });
       

        $routeProvider.when("/form_controls", {
            controller: "formController",
            templateUrl: "/erpapp/views/form_controls.html"
        });

        $routeProvider.when("/form_validation", {
            controller: "formController",
            templateUrl: "/erpapp/views/form_validation.html"
        });


        $routeProvider.when("/form_wizards", {
            controller: "formController",
            templateUrl: "/erpapp/views/form_wizards.html"
        });

        $routeProvider.when("/tables", {
            controller: "staticdataTablesController",
            templateUrl: "/erpapp/views/tables.html"
        });


        $routeProvider.when("/tables_dynamic", {
            controller: "dynamicdataTablesController",
            templateUrl: "/erpapp/views/tables_dynamic.html"
        });

        $routeProvider.when("/register", {
            controller: "registerController",
            templateUrl: "/erpapp/views/register.html"
        });


        $routeProvider.when("/chartjs2", {
            controller: "chartsController",
            templateUrl: "/erpapp/views/chartjs2.html"
        });



        $routeProvider.when("/morisjs", {
            controller: "chartsController",
            templateUrl: "/erpapp/views/morisjs.html"
        });



        $routeProvider.when("/echarts", {
            controller: "chartsController",
            templateUrl: "/erpapp/views/echarts.html"
        });

        $routeProvider.when("/form_tabs", {
            controller: "formController",
            templateUrl: "/erpapp/views/form_tabs.html"
        });


        $routeProvider.when("/products", {
            controller: "productController",
            templateUrl: "/erpapp/views/products.html"
        });


        //$routeProvider.otherwise({ redirectTo: "/" });

    });

    //directive to show hide tabs
    app.directive('showTab', function () {
        return {
            link: function (scope, element, attrs) {
                element.click(function (e) {
                    e.preventDefault();
                    jQuery(element).tab('show');
                });
            }
        };
    });

    //directive to make data table
    app.directive('makeTable', ['$timeout', function ($timeout) {
        return {
            link: function ($scope, element, attrs) {
                $scope.$on('makedatatable', function () {
                    $timeout(function () {
                        jQuery(element).DataTable();
                    }, 0, false);
                })
            }
        };
    }]);

    //directive to make data table
    app.directive('autoComplete', ['$timeout', function ($timeout) {
        return {
            scope: {
                // creates a scope variable in your directive
                // called `autodatat` bound to whatever was passed
                // in via the `autodata` attribute in the DOM
                autodata: '=autodata'
            },
            link: function ($scope, element, attrs) {
                $scope.$watch('autodata', function (autodata) {

                    var productsArray = $.map(autodata, function (value, key) {
                        return {
                            label: value.ProductID,
                            value: value.ProductName
                        }
                    })
                  
                    jQuery(element).autocomplete({
                        lookup: productsArray
                    });
                });
            }
        };
    }]);



    //var serviceBase = 'http://localhost:50482/';
    var serviceBase = "http://localhost:52086/";
    //var serviceBase = 'http://hlfservice.azurewebsites.net/';
    app.constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase
    });



    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });

    app.run(['authService', function (authService) {
        authService.fillAuthData();
    }]);

