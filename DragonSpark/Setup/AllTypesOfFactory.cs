using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using System;
using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Contracts;
using Activator = DragonSpark.Activation.Activator;
using Type = System.Type;

namespace DragonSpark.Setup
{
	[Persistent]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly Assembly[] assemblies;
		readonly Activator.Get activator;

		public AllTypesOfFactory( [Required]Assembly[] assemblies, [Required]Activator.Get activator )
		{
			this.assemblies = assemblies;
			this.activator = activator;
			this.activator = activator;
		}

		public T[] Create<T>() => Create( typeof(T) ).Cast<T>().ToArray();

		protected override Array CreateItem( Type parameter )
		{
			var types = assemblies.SelectMany( assembly => assembly.ExportedTypes );
			var result = activator().ActivateMany( parameter, types ).ToArray();
			return result;
		}
	}
}