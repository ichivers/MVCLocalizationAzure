var angularStartServices = angular.module('angularStart.services', ['ngResource']);

angularStartServices.factory('ResourceService', ['$resource',
  function ($resource) {
      return {
          resourceClasses: $resource('api/v1/localization/:resourceClass', { resourceClass: '@resourceClass' }, {
              query: { method: 'GET', isArray: true }
          }),
          resources: $resource('api/v1/localization/:resourceClass/:resource',
              {
                  resourceClass: '@resourceClass',
                  resource: '@resource'
              }, {
                  save: { method: 'POST', isArray: false }
              })
      };
  }]);