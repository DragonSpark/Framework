using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DragonSpark.Extensions;

namespace DragonSpark.IoC.Configuration
{
    public class AssemblySupport
	{
		readonly List<Assembly> applied = new List<Assembly>();

		readonly ICollection<Action<IEnumerable<Assembly>>> actions = new List<Action<IEnumerable<Assembly>>>();

	    AssemblySupport()
	    {}

	    public static AssemblySupport Instance
	    {
		    get { return InstanceField; }
	    }	static readonly AssemblySupport InstanceField = new AssemblySupport();

	    void OnLoad( object sender, AssemblyLoadEventArgs args )
	    {
		    actions.Any().IsTrue( () => Refresh() );
	    }

	    public void Register( Action<IEnumerable<Assembly>> action, bool apply = true )
		{
			actions.Add( action );
			Refresh( apply );
		}

		[MethodImpl( MethodImplOptions.Synchronized )]
		void Refresh( bool apply = true )
		{
			AppDomain.CurrentDomain.AssemblyLoad -= OnLoad;
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where( x => !x.IsDynamic ).SelectMany( x => x.AsItem( x.GetReferencedAssemblies().Select( Load ) ) ).NotNull().Distinct().Except( applied ).OrderBy( x=> x.GetName().Name ).ToArray();

			applied.AddRange( assemblies );

			apply.IsTrue( () => actions.Apply( x => x( assemblies ) ) );
			AppDomain.CurrentDomain.AssemblyLoad += OnLoad;
		}

	    static Assembly Load( AssemblyName arg )
	    {
		    try
		    {
			    return Assembly.Load( arg );
		    }
		    catch ( FileNotFoundException )
		    {
			    return null;
		    }
	    }
	}
}