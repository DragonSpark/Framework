define(["durandal/system", "durandal/binder", "./core", "./monitor", "./operation"], function (system, binder, core, monitor, operation) {
	var ctor = function ()
		{
			this.core = core;
		
			this.context = ko.observable( this );
			this.operations = ko.observableArray([]);
			this._remaining = [];
			this.status = ko.observable(core.ExecutionStatus.None);
			this.allowClose = ko.observable(true);
			this.percentageComplete = ko.observable(0);

			this.options = ko.observable({
				allowCancel: ko.observable(true),
				closeOnCompletion: ko.observable(true),
				anyExceptionIsFatal: ko.observable(false),
				items: []
			});
		};
	
	ctor.prototype.attached = function(element, parent, settings)
	{
		var that = this;
		that.settings = $.extend({}, {
			title: "Loading... please wait."
		}, settings);
		
		that.title = ko.observable(that.settings.title);
		
		var model = settings.bindingContext.$data;
		var item = monitor.getMonitor( model );
		var current = monitor.initialize(that, model);
		if ( current )
		{
			that.context( item );
			current.then( function() { that.context( that ); } );
		}
	};

	ctor.prototype.monitor = function (profile) {
		var self = this,
		    extend = $.extend( {
				title: this.settings.title || this.title(),
				allowCancel: true,
				closeOnCompletion: true,
				anyExceptionIsFatal: false,
			}, profile, { dataContext: null } );
		
		var options = ko.mapping.fromJS(extend);
		self.options(options);
		
		self.operations.removeAll();

		var items = ko.utils.unwrapObservable(profile.items) || ( profile.item ? [profile.item] : [] );
		items.map(function(i) {
			var item = new operation(this, i);
			self.operations.push(item);
			self._remaining.push(item);
		});

		if ( items.length && !self._active) {
			self._active = system.defer();
			self.status(core.ExecutionStatus.Executing);
			self._checkCompletion();
		}

		var deferred = self._active ? self._active : system.defer( function (d) { d.resolve(); } );
		var result = deferred.promise();
		return result;
	};

	ctor.prototype._checkCompletion = function (a) {
		var self = this,
			args = $.extend({}, { error: null, wasCancelled: false }, a);

		this.options().closeOnCompletion( this.options().closeOnCompletion() & args.error == null );

		var next = !args.wasCancelled && ( args.error == null || ( !this.options().anyExceptionIsFatal() && ( this._current == null || this._current.exceptionHandlingAction() == core.ExceptionHandlingAction.Continue ) ) );
		if (next) {
			var length = this.operations().length;
			var complete = length == 1 ? .5 : (this.operations().indexOf(this._current) + 1) / length;
			this.percentageComplete(complete );
			this._current = this._remaining.length > 0 ? this._remaining.shift() : null;
			if (this._current) {
				try {
					var promise = self._current.execute(self);
					Q(promise).then(function () { self._checkCompletion(); }, function (error) { self._complete(error); });
				} catch (e) {
					self._complete(e);
				}
			} else {
				this._complete();
			}
		} else {
			this._complete(args.error, args.wasCancelled);
		}
	},
	
	ctor.prototype.close = function () {
		this.operations().map(function(i) {
			i.reset();
		});

		this._active.resolve();
		this._active = null;

		this.operations.removeAll();
		this._remaining.splice();

		this.status(core.ExecutionStatus.None);
		this.percentageComplete(0);
		this._currentOperation = null;
	},

	ctor.prototype._complete = function (e, cancelled) {
		var self = this;
		var items = this._remaining.slice(this._remaining.indexOf(this._current) + 1);
		items.map(function(i) {
			if (i.abort) {
				i.abort();
			}
		});

		this.percentageComplete(1);
		var status = cancelled ? core.ExecutionStatus.Canceled : (function () {
			var from = Enumerable.From(self.operations());
			var item = from.FirstOrDefault(null, "$.exception() != null");
			var result = item == null ? core.ExecutionStatus.Completed : self.options().anyExceptionIsFatal() || item.exception() instanceof FatalError ? core.ExecutionStatus.CompletedWithFatalException : core.ExecutionStatus.CompletedWithException;
			return result;
		})();
		this.status(status);

		this.allowClose(status != core.ExecutionStatus.CompletedWithFatalException);
		this.options().allowCancel( false );
		
		var close = this.allowClose() && this.options().closeOnCompletion() && e == null;
		if (close) {
			this.close();
		}
		var propagate = this._current != null && this._current.exception() != null && this._current.exceptionHandlingAction() == core.ExceptionHandlingAction.Throw ? this._current.exception() : null;
		this._current = null;
		if ( propagate != null )
		{
			throw propagate;
		}
	};

	return ctor;
});