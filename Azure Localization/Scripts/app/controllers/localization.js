angular.module('main')
    .controller('LocalizationController', ['$scope', 'ResourceService',
    function ($scope, ResourceService) {
        $scope.resources = [];
        $scope.gridOptions = {
            data: 'resources',
            enableCellSelection: true,
            enableCellEditOnFocus: true,
            enableCellEdit: true,
            enableRowSelection: false,
            columnDefs: [
                {
                    field: 'Culture',
                    displayName: 'Culture'
                },
                {
                    field: 'Key',
                    displayName: 'Key'
                },
                {
                    field: 'Value',
                    displayName: 'Value',
                    enableCellEdit: true
                }
            ]
        };
        $scope.title = "Localization";
        ResourceService.resourceClasses.query(function (data) {
            $scope.resourceClasses = data;
        });
        $scope.resourceClass_Selected = function (selectedResourceClass) {
            ResourceService.resourceClasses.query({ resourceClass: selectedResourceClass.Name }, function (data) {
                $scope.resources = data;
            })
        };

        $scope.$on('ngGridEventStartCellEdit', function (evt) {
            $scope.OriginalRow = angular.copy(evt.targetScope.row.entity);
        });

        $scope.$on('ngGridEventEndCellEdit', function (evt) {
            $scope.UpdatedRow = evt.targetScope.row.entity;
            if ($scope.OriginalRow.Value != $scope.UpdatedRow.Value)
                ResourceService.resources.save({
                    resourceClass: evt.targetScope.row.entity.Area,
                    resource: evt.targetScope.row.entity.Key
                }, evt.targetScope.row.entity);
        })
    }]);