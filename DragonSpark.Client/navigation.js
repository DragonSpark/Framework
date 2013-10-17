define( [ "durandal/system", "durandal/events", "./application", "./configuration", "plugins/router", "plugins/history", "plugins/storage" ], function( system, events, application, configuration, router, history, storage ) {
	function build( target, items ) 
	{
		var result = target.map( items ).buildNavigationModel();
		target.mapUnknownRoutes( instance.data.NotFound.moduleId );
		return result;
	}
	
	function updateDocumentTitle( model, instruction )
	{
		var parts = [], current = model.router || router;

		while ( current )
		{
			var part = mappings[ $(model).getUID() ].title || "Not Found";
			parts.unshift( part );
			current = current.parent;
		}

		parts.unshift( configuration().ApplicationDetails.Title );
			
		document.title = parts.join( " > " );
	};

	var monitor = function( source )
	{
		this.source = source;
		this.nextFragment = null;
		events.includeIn( this );
	};

	monitor.prototype.continue = function( model, instruction )
	{
		var complete = function()
		{
			var fragment = !that.exception && instruction ? instruction.fragment : null;
			var cancelled = instruction == null;
			
			that.complete( fragment, cancelled );
		};

		var that = this;
		if ( !that.exception && model.router && instruction )
		{
			model.router.on( "router:route:activating", function() {
				model.router.off( "router:route:activating", null, that );
				if ( !that.exception )
				{
					that.clear();
					monitors[ $( model.router ).getUID() ].activate();
				}
				else
				{
					model.router.attached();
					complete();
				}
			}, that );
		}
		else
		{
			complete();
		}
	};

	monitor.prototype.activate = function( fragment )
	{
		var that = this;
		if ( !that.running & ( that.running = true ) )
		{
			// var currentException = this.exception;
			application.on( "application:exception", function( args ) {
				args.handled = true;
				that.exception = args.exception;
				/*var model = that.source.activeItem();
				that.continue( model );*/
			}, that );
			
			var source = that.source;
			source.on( "router:navigation:complete", function( currentModel, currentInstruction, item ) {
				that.continue( currentModel, currentInstruction );
			}, that );

			source.on( "router:navigation:cancelled", function( currentModel, currentInstruction, item ) {
				that.continue( currentModel );
			}, that );
			
			fragment = fragment || this.nextFragment;
			if ( fragment != null )
			{
				this.nextFragment = null;
				var navigate = source.navigate( fragment );
				return navigate;
			}
		}
		else
		{
			that.nextFragment = fragment;
		}
		return null;
	};

	monitor.prototype.clear = function()
	{
		application.off( "application:exception", null, this );
		this.source.off( "router:navigation:complete", null, this );
		this.source.off( "router:navigation:cancelled", null, this );
	};

	monitor.prototype.complete = function( fragment, cancelled )
	{
		var that = this,
		    exception = this.exception;
		
		that.running = false;
		
		var complete = function()
		{
			that.clear();

			that.running = false;
				
			instance.trigger( "navigation:complete", instance );
		
			if ( that.source.parent )
			{
				var parent = monitors[$(that.source.parent).getUID()];
				parent.exception = exception;
				parent.complete( null, cancelled );
			}
			else if ( instance.lastAttemptedFragment() != instance.lastKnownFragment() )
			{
				completed.push( instance.lastAttemptedFragment() );
			}
		};

		if ( exception )
		{
			instance.exception = exception;
			this.exception = null;
			this.nextFragment = instance.lastKnownFragment(); // !that.source.parent && !cancelled ?  : null;

			if ( that.source.parent )
			{
				complete();
				return;
			}
		}

		if ( this.nextFragment != null )
		{
			var navigated = this.activate( this.nextFragment );
			if ( navigated == false || navigated === undefined )
			{
				complete();
			}
		}
		else
		{
			complete();
		}
	};
	
	function ensure( model, instruction, source )
	{
		var indexOf = models.indexOf( model );
		if ( indexOf > -1 && !model.router )
		{
			models.remove( indexOf );

			var config = instruction.config;
			var id = config.moduleId;

			var options = { moduleId : id, fromParent : true };
			model.router = initialize( source.createChildRouter().makeRelative( options ) );
			build( model.router, config.children );

			updateSubscriptions( model.router, options );

			configuration.on( "application:configuration:refreshed", function() {
				model.__resetRouter__ = true;
			} );
		}
	}

	function initialize( source )
	{
		source.updateDocumentTitle = updateDocumentTitle;
		var uid = $(source).getUID();
		monitors[uid] = new monitor( source );
		return source;
	}

	function updateSubscriptions( target, options )
	{
		var item = target || router;
		item.on( "router:route:activating", function( model, instruction, source ) {
			mappings[ $(model).getUID() ] = instruction.config;
			ensure( model, instruction, source );

			if ( !source.parent && instruction.config.moduleId != instance.data.NotFound.moduleId )
			{
				storage.set( fragmentKey, instruction.fragment );
				if ( !options )
				{
					monitors[$(source).getUID()].activate();
				}
			}
		} );
		
		if ( options )
		{
			item.on( "router:navigation:attached", function( model, instruction, source ) {
				if ( model && model.__resetRouter__ )
				{
					model.__resetRouter__ = false;
					source.reset();
					source.makeRelative( options );
						
					var children = mappings[ $(model).getUID() ].config.children;
					build( source, children );
					updateSubscriptions( source, options );
				}
			} );
			
			item.on( "router:route:before-config" ).then( function( routeConfiguration ) {
				routeConfiguration.moduleId = routeConfiguration.moduleId == id + "/" + options.id ? moduleId : routeConfiguration.moduleId;
			} );
		}
	}
	
	var mappings = {}, monitors = {}, models = [], completed = [], fragmentKey = "navigation.fragment.lastAttempted",
	    instance =
	    {
		    router : initialize( router ),
		    load : function( routes )
		    {
			    router.deactivate();
			    router.reset();
			    updateSubscriptions();

			    build( router, routes );
		    },

		    data : null,
		    
			lastAttemptedFragment : function()
			{
				var result = storage.get( fragmentKey );
				return result;
			},
		    
			lastKnownFragment : function()
			{
				return Enumerable.From( completed ).LastOrDefault() || "";
			},

		    refresh : function()
		    {
   				var instruction = router.activeInstruction();
				if ( instruction )
				{
					history.fragment = null;
					monitors[ $(router).getUID() ].activate( instruction.fragment );
				}   
		    },
		    
			isActive : function()
			{
				var result = monitors[ $(router).getUID() ].running;
				return result;
			},
		    
			attach : function( model )
			{
				models.push( model );
			},

		    back : function()
		    {
			    var fragment = instance.lastKnownFragment();
			    monitors[ $(router).getUID() ].activate( fragment );
		    }
	    };

	configuration.on( "application:configuration:refreshing", function( module, data ) {
		instance.data = data.Navigation;
		instance.load( data.Navigation.Routes );
	} );

	application.on( "application:refreshed", function() {
		instance.refresh();
	} );

	updateSubscriptions();

	events.includeIn( instance );
	
	return instance;
});