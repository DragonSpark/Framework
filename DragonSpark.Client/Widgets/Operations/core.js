define(["dragonspark/enum", "./monitor", "dragonspark/application"], function ( enumerable, monitor, application ) {
	var instance = {
		ExecutionStatus: new enumerable({
			None: {},
			Executing: {},
			Canceling: {},
			Canceled: { description: "Canceled by User" },
			Aborted: {},
			Completed: {},
			CompletedWithException: { description: "Completed with Exception" },
			CompletedWithFatalException: { description: "Completed with Fatal Exception (oops!)" }
		}),
		ExceptionHandlingAction: new enumerable({
			None: {},
			Continue: {},
			Throw: {}
		}),
		_initialized : false,
		initialize: function()
		{
			if ( !instance._initialized && ( instance._initialized = true ) )
			{
				application.on("application:refresh").then(function () {
					monitor._broadcast("Refreshing Application... Please Wait.", "application:operations:refreshing", "application:operations:refreshed");
				}, instance);
			}
			return instance;
		}
	};
	
	return instance;
});