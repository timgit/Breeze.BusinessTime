(function() {
    var app = angular.module('app', []);

    app.factory('dataService', function() {

        // define the Breeze DataService for this app
        var service = new breeze.DataService({
            serviceName: '/breeze/data',
        });
            
        // create a new EntityManager that uses this metadataStore
        var entityManager = new breeze.EntityManager({
            dataService: service,
        });

        return entityManager;
        
    });

    app.controller('homeController', function($scope, dataService) {
        var vm = {
            save: save,
            errors: []
        };

        $scope.vm = vm;

        dataService.executeQuery(breeze.EntityQuery.from("Dealers").take(1))
        .then(function (data) {
            $scope.$apply(function() {
                $scope.dealer = data.results[0];
            });
        });

        function save() {
            dataService.saveChanges()
            .then(function () {
                $scope.$apply(function () {
                    vm.saveSucceeded = true;
                    vm.errors = [];
                });
            })
            .catch(function (error) {
                $scope.$apply(function () {
                    vm.saveSucceeded = false;

                    if (error.entityErrors)
                        vm.errors = error.entityErrors;
                    else
                        vm.errors = [error.message];
                });
            });

           
        }

    });
})();
