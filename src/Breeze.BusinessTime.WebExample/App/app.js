(function() {
    var app = angular.module('app', []);

    app.factory('dataService', function() {
        var manager = createEntityManager();

        var api = {        
            manager: manager,
            hasChanges: hasChanges,
            saveChanges: saveChanges,
        };

        return api;

        function createEntityManager() {
            // define the Breeze DataService for this app
            var service = new breeze.DataService({
                serviceName: '/api/data',
            });
            
            // create a new EntityManager that uses this metadataStore
            var entityManager = new breeze.EntityManager({
                dataService: service,
            });

            return entityManager;
        }

        function hasChanges() {
            return manager.hasChanges();
        }

        function saveChanges() {
            return manager.saveChanges()
                .catch(saveFailed);

            function saveFailed(error) {
                var reason = error.message;
                var detail = error.detail;

                if (error.entityErrors) {
                    reason = handleSaveValidationError(error);
                } else if (detail && detail.ExceptionType &&
                    detail.ExceptionType.indexOf('OptimisticConcurrencyException') !== -1) {
                    reason = "Another user, perhaps the server, may have deleted some data.";
                } else {
                    reason = "Failed to save changes: " + reason + " You may have to restart the app.";
                }

                console.error(reason);

                throw error; // so downstream promise users know it failed
            }

            function handleSaveValidationError(error) {
                var message = "Not saved due to validation error";
                try { // fish out the first error
                    var firstErr = error.entityErrors[0];
                    message += ": " + firstErr.errorMessage;
                } catch (e) { /* eat it for now */ }
                return message;
            }
        }
    });

    app.controller('homeController', function($scope, dataService) {

        dataService.manager.executeQuery(breeze.EntityQuery.from("Dealers"))
        .then(function(data) {
            $scope.dealers = data.results;
        });

    });
})();
