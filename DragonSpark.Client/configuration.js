define(["durandal/events"], function ( events ) {
	var instance = $.extend( ko.observable(), {
		apply: function (data) {
			instance(data);
			instance.trigger("application:configuration:refreshed", instance, data);
		}
	} );

	events.includeIn(instance);

	return instance;
});