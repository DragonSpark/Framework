define(function (require) {
	var system = require("durandal/system"),
		monitor = require("./monitor"),
		operation = require("./operation");
	var ctor = function (element, settings) {
		this.module = module;
		this.settings = settings;
		this.operations = ko.observableArray([]);

		this._remaining = [];

		$.extend(this.settings, {
			title: "Loading... please wait."
		});

		this.title = ko.observable(this.settings.title);
		this.status = ko.observable(module.ExecutionStatus.None);
		this.allowClose = ko.observable(true);
		this.percentageComplete = ko.observable(0);

		this.options = ko.observable({
			allowCancel: ko.observable(true),
			closeOnCompletion: ko.observable(true),
			anyExceptionIsFatal: ko.observable(false),
			items: []
		});

		monitor.initialize(this, settings.bindingContext);
	};

	ctor.prototype.monitor = function (profile) {
		var self = this;
		$.extend({
			title: this.settings.title || this.title(),
			allowCancel: true,
			closeOnCompletion: true,
			anyExceptionIsFatal: false,
			items: profile.item != null ? [profile.item] : []
		}, profile);
		
		var options = ko.mapping.fromJS(profile);
		this.options(options);
		this.operations.removeAll();

		profile.items.map(function(i) {
			var item = new operation(this, i);
			self.operations.push(item);
			self._remaining.push(item);
		});

		if (!this._active) {
			this._active = system.defer();
			this.status(module.ExecutionStatus.Executing);
			this._checkCompletion();
		}

		var result = this._active.promise();
		return result;
	};

	ctor.prototype._checkCompletion = function (args) {
		var self = this;
		args = $.extend({ error: null, wasCancelled: false }, args);

		this.options().closeOnCompletion( this.options().closeOnCompletion() & args.error == null );

		var next = !args.wasCancelled && ( args.error == null || ( !this.options().anyExceptionIsFatal() && ( this._current == null || this._current.exceptionHandlingAction() == module.ExceptionHandlingAction.Continue ) ) );
		if (next) {
			var foo = (this.operations().indexOf(this._current) + 1) / this.operations().length;
			this.percentageComplete(foo );
			this._current = this._remaining.length > 0 ? this._remaining.shift() : null;
			if (this._current) {
				try {
					var promise = this._current.execute(this);
					$.when(promise).then(function () { self._checkCompletion(); }, function (error) { self._complete(error); });
				} catch (e) {
					this._complete(e);
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

		this.status(module.ExecutionStatus.None);
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
		var status = cancelled ? module.ExecutionStatus.Canceled : (function () {
			var from = Enumerable.From(self.operations());
			var item = from.FirstOrDefault(null, "$.exception() != null");
			var result = item == null ? module.ExecutionStatus.Completed : self.options().anyExceptionIsFatal() || item.exception() instanceof FatalError ? module.ExecutionStatus.CompletedWithFatalException : module.ExecutionStatus.CompletedWithException;
			return result;
		})();
		this.status(status);

		this.allowClose(status != module.ExecutionStatus.CompletedWithFatalException);
		this.options().allowCancel( false );
		
		var close = this.allowClose() && this.options().closeOnCompletion() && e == null;
		if (close) {
			this.close();
		}
		var propagate = this._current != null && this._current.exception() != null && this._current.exceptionHandlingAction() == module.ExceptionHandlingAction.Throw ? this._current.exception() : null;
		this._current = null;
		if ( propagate != null )
		{
			throw propagate;
		}
	};

	return ctor;
});