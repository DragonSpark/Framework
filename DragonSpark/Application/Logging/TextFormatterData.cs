using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace DragonSpark.Application.Logging
{
	public sealed class TextFormatterData : Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TextFormatterData
	{
		public TextFormatterData()
		{
			Type = typeof(TextFormatter);
		}

		public TextFormatterData( string templateData ) : base( templateData )
		{}

		public TextFormatterData( string name, string templateData ) : base( name, templateData )
		{}

		/// <summary>
		/// Returns the <see cref="TypeRegistration"/> entry for this data section.
		/// </summary>
		/// <returns>The type registration for this data section</returns>
		public override IEnumerable<TypeRegistration> GetRegistrations()
		{
			yield return new TypeRegistration<ILogFormatter>(
				() => new TextFormatter(Template))
			             	{
			             		Name = Name,
			             		Lifetime = TypeRegistrationLifetime.Transient
			             	};
		}
	}
}