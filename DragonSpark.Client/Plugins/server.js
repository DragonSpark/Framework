define([ "durandal/app", "durandal/system", "durandal/events", "dragonspark/session", "dragonspark/configuration" ], function ( app, system, events, session, configuration ) {
	// breeze.NamingConvention.camelCase.setAsDefault();

	var references = {};
	
	function clean() {
		for (key in references) {
			if (references[key].referenceCount == 0) {
				delete references[key];
			}
		}
	}
	
	var reference = function( create, item ) {
		var value = null;

		this.referenceCount = 0;

		this.value = function ()
		{
			value = value || create.apply( item, [] );
			
			this.referenceCount++;
			return value;
		};

		this.clear = function () {
			value = null;
			this.referenceCount = 0;

			clean();
		};
	};

	reference.prototype.release = function () {
		this.referenceCount--;
		if (this.referenceCount === 0) {
			this.clear();
		}
	};

	var context = function ( manager, queries )
	{
		var that = this;
		function create()
		{
			var copy = manager.createEmptyCopy();
			copy.importEntities( manager.exportEntities() );
            copy.hasChangesChanged.subscribe( function(args) {
                that.trigger( "application:entityContext:hasChanges", that );
	            var hasChanges = copy.hasChanges();
	            that.hasChanges( hasChanges );
            } );
			return copy;
		}

		that.hasChanges = ko.observable();

		that.manager = create();

		queries.forEach( function( i ) {
			that[ i.SetName ] = new set( that, i.EntityName, i.Path, i.isLocal ? breeze.FetchStrategy.FromLocalCache : breeze.FetchStrategy.FromServer );
		} );

		events.includeIn( that );
	};
	
	var set = function ( owner, entityTypeName, resourceName, fetchStrategy ) {
		function executeQuery(query)
		{
			var result = owner.manager.executeQuery( query.using( fetchStrategy || breeze.FetchStrategy.FromServer ) ).then(function (data) { return data.results; });
			return result;
		}

		function executeCacheQuery(query)
		{
			var result = owner.manager.executeQueryLocally( query );
			return result;
		}
		
		var entityType;
		if (entityTypeName) {
			entityType = owner.manager.metadataStore.getEntityType( entityTypeName );
			entityType.setProperties( { defaultResourceName: resourceName } );

			owner.manager.metadataStore.setEntityTypeForResourceName(resourceName, entityTypeName);
		}

		this.expanding = function( entity, expand )
		{
			var query = breeze.EntityQuery.fromEntities( entity ).expand( expand );
			return this.find( query ).then( function( items ) {
				var item = Enumerable.From( items ).SingleOrDefault();
				return item;
			} );
		};

		this.withId = function( key, expand )
		{
			var that = this;

			if (!entityTypeName)
				throw new Error("Repository must be created with an entity type specified");

			return owner.manager.fetchEntityByKey(entityTypeName, key, true).then(function (data) {
				if ( !data.entity )
				{
					throw new Error("Entity not found!");
				}

				var result = expand ? that.expanding( data.entity, expand ) : data.entity;
				return result;
			});
		};

		this.query = function( predicate, expand )
		{
			var core = breeze.EntityQuery.from( resourceName );
			var expanded = expand ? core.expand( expand ) : core;
			var result = predicate ? expanded.where( predicate ) : expanded;
			return result;
		};

		this.find = function( query )
		{
			var result = executeQuery( query );
			return result;
		};

		this.findInCache = function( query ) 
		{
			var result = executeCacheQuery( query );
			return result;
		};

		this.item = function( query )
		{
			var result = executeQuery( query ).then( function( items ) {
				return Enumerable.From( items ).SingleOrDefault();
			} );
			return result;
		};

		this.all = function ( expand ) 
		{
			var query = this.query( null, expand );
			var result = executeQuery( query );
			return result;
		};
	};
	
	context.prototype.commit = function ()
	{
		var that = this;
		return this.manager.saveChanges().then( function ( saveResult ) {
			that.trigger( "application:server:saved", saveResult.entities );
			return saveResult;
		} );
	};

	context.prototype.rollback = function () {
		this.manager.rejectChanges();
	};

	var service = function( path, queries )
	{
		this.manager = new breeze.EntityManager( path );
		this.queries = queries;
	};
	service.prototype.initialize = function( data )
	{
		var manager = this.manager;
		
		return manager.fetchMetadata().then( function() {
			var paths = Enumerable.From( data.Extensions ).Where( "$.Path != null" ).Select( "$.Path" ).ToArray();
			return system.acquire( paths ).then( function( items ) {
				items.forEach( function( item ) {
					manager.metadataStore.registerEntityTypeCtor( item.name, item.constructor, item.initializer );
				} );

				var result = data.InitializationMethod ? manager.executeQuery( breeze.EntityQuery.from( data.InitializationMethod ) ) : null;
				return result;
			} );
		} );
	};

	service.prototype.create = function()
	{
		var result = new context( this.manager, this.queries );
		return result;
	};
	service.prototype.reference = function( key )
	{
		references[ key ] = references[ key ] || new reference( this.create, this );
		return references[ key ];
	};
	
	function initialize( data )
	{
		data.Hubs.forEach( function( i ) {
			instance.hubs[ i ] = $.connection[ i ];
			define( i + ".client", [], function() { return $.connection[ i ].client; } );
			define( i + ".service", [], function() { return $.connection[ i ].server; } );
			define( i, [], function() { return $.connection[ i ].server; } );
			$.connection[ i ].client.exceptionHandler = function( error )
			{
				$ds.lastServiceError = error;
			};
		} );

		var initializers = [];
		data.EntityServices.forEach( function( i ) {
			var item = instance.entityServices[ i.Location ] = new service( i.Location, i.Queries );
			initializers.push( item.initialize( i ) );
		} );
		var key = Enumerable.From( data.EntityServices ).Select( "$.Location" ).FirstOrDefault();
		instance.entities = key ? instance.entityServices[ key ] : null;
		return Q.all( initializers );
	}
	
	var instance =
	{
		hubs : {},
		entityServices : {},
		entities : null,
		initialize: function()
		{
			$.connection.hub.logging = true;
			var result = session.getConfiguration().then( function( data ) {
				var configured = data.ServerConfiguration ? initialize( data.ServerConfiguration ) : data;
				configuration.apply( data );
				return configured;
			} );
			return result;
		},
		
		refreshConfiguration : function()
		{
			var result = session.getConfiguration().then( configuration.apply );
			return result;
		},
		
		start : function()
		{
			var result = Q( $.connection.hub.start() );
			return result;
		},
		
		refresh : function()
		{
			$.connection.hub.stop( false, true );
			var result = instance.start();
			return result;
		},
		
		restart: function()
		{
			var result = instance.refresh().then( session.getConfiguration ).then( configuration.apply );
			return result;
		},
		
		ensure: function( obj, entityTypeName )
		{
			if ( !obj.entityType || obj.entityType.shortName !== entityTypeName )
			{
				throw new Error( 'Object must be an entity of type ' + entityTypeName );
			}
		}
	};
	return instance;
});