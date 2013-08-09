define(function (require) {
	var system = require("durandal/system"),
		router = require('durandal/plugins/router'),
        app = require('durandal/app');

    return {
    	router: router,
    	immediate : function() {
    		alert("Executing immediate operation!");
    	},
    	viewAttached : function() {
    	},
    	operation: function () {
    		var result = system.defer(function (dfd) {
    			$ds.wait(1000).then(function () {
    				dfd.resolve();
    			});
    		}).promise();
			return result;
    	},
    	
    	throwException : function() {
    		throw new Error('Hello World');
    	},
        search: function() {
            //It's really easy to show a message box.
            //You can add custom options too. Also, it returns a promise for the user's response.
            app.showMessage('Search not yet implemented...');
        },
        activate: function () {
            return router.activate('welcome');
        }
    };
});