var appRoot = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'angularStart.services', 'angularStart.directives']);

appRoot.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/home', { templateUrl: '/home', controller: 'HomeController'})
        .when('/home/about', { templateUrl: '/home/about', controller: 'HomeController'})
        .when('/home/contact', { templateUrl: '/home/contact', controller: 'HomeController'})
        .when('/localization', { templateUrl: '/localization', controller: 'LocalizationController' })
        .otherwise({ redirectTo: '/home' });
}])
.controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
    $scope.$on('$routeChangeSuccess', function (e, current, previous) {
        $scope.activeViewPath = $location.path();
    });
}]);
