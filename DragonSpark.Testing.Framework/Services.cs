using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using DragonSpark.Diagnostics;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DragonSpark.Testing.Framework
{
	public class ServiceLocation : IServiceLocation
	{
		public bool IsAvailable => Locator != null;

		public IServiceLocator Locator { get; private set; }

		public void Assign( IServiceLocator instance )
		{
			Locator = instance;
		}
	}

	public class ServiceLocationHost : IServiceLocationHost
	{
		public static ServiceLocationHost Instance { get; } = new ServiceLocationHost();
		
		public IServiceLocation Location => AmbientValues.Get<IServiceLocation>();
	}

	class AmbientValueRepository<TKey, TValue> : Runtime.AmbientValueRepository<TKey, TValue>
	{
		readonly AsyncLocal<TValue> item = new AsyncLocal<TValue>();

		protected override TValue GetValue( TKey key )
		{
			var result = !Equals( item.Value, default(TValue) ) ? item.Value : Determine( key );
			return result;
		}

		TValue Determine( TKey key )
		{
			var @base = base.GetValue( key );
			var result = !Equals( @base, default(TValue) ) ? @base : FromCallStack();
			return result;
		}

		protected override void OnAdd( TKey key, TValue instance )
		{
			base.OnAdd( key, instance );
			item.Value = instance;
		}

		protected override void OnRemove( TKey key )
		{
			var same = Equals( item.Value, Items[key] );
			item.Value = same ? default(TValue) : item.Value;

			base.OnRemove( key );
		}

		TValue FromCallStack()
		{
			var result = new System.Diagnostics.StackTrace().GetFrames().Select( x => x.GetMethod() ).OfType<TKey>().SingleOrDefault( Items.ContainsKey ).Transform( info => Items[info] );
			return result;
		}
	}

	[Priority( Priority.High )]
	public class ServicesAttribute : BeforeAfterTestAttribute, ICustomization
	{
		static ServicesAttribute()
		{
			var repository = new CompositeAmbientValueRepository( new Runtime.AmbientValueRepository<Type, ITestOutputHelper>(), new AmbientValueRepository<MethodInfo, IServiceLocation>() );
			AmbientValues.Initialize( repository );
			Services.Initialize( ServiceLocationHost.Instance );
		}

		public ServicesAttribute() : this( new ServiceLocation() )
		{}

		public ServicesAttribute( IServiceLocation location )
		{
			Location = location;
		}

		public void Customize( IFixture fixture )
		{
			var locator = new ServiceLocator( fixture );
			Location.Assign( locator );

			var context = fixture.Create<IMethodContext>().MethodUnderTest;
			AmbientValues.Register( context, Location );

			fixture.Inject<IServiceLocationHost>( ServiceLocationHost.Instance );
			fixture.InjectWithImplementation( Location );

			Log.Current.Information( "Hello World!" );
		}

		public override void After( MethodInfo methodUnderTest )
		{
			base.After( methodUnderTest );

			AmbientValues.Clear( methodUnderTest );
			Location.Assign( null );
		}

		protected IServiceLocation Location { get; }
	}
	
	public class AssignAttribute : CustomizeAttribute
	{
		class Customization : ICustomization
		{
			readonly Type type;

			public Customization( Type type )
			{
				this.type = type;
			}

			public void Customize( IFixture fixture )
			{
				var locator = (IServiceLocator)new SpecimenContext(fixture).Resolve(type);
				var location = fixture.Create<IServiceLocation>();
				location.Assign( locator );
			}
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = new Customization( parameter.ParameterType );
			return result;
		}
	}

	public class DefaultServicesAttribute : ServicesAttribute
	{
		public DefaultServicesAttribute() : base( Activation.ServiceLocation.Instance )
		{}
	}
}