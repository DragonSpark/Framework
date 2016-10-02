using JetBrains.Annotations;
using PostSharp.Patterns.Contracts;
using Serilog.Configuration;
using System.Collections.Generic;

namespace DragonSpark.Diagnostics.Configurations
{
	public class ReadFromKeyValuePairsCommand : ReadFromCommandBase
	{
		public ReadFromKeyValuePairsCommand() : this( new Dictionary<string, string>() ) {}

		public ReadFromKeyValuePairsCommand( IDictionary<string, string> dictionary )
		{
			Dictionary = dictionary;
		}

		[Required, UsedImplicitly]
		public IDictionary<string, string> Dictionary { [return: Required]get; set; }

		protected override void Configure( LoggerSettingsConfiguration configuration ) => configuration.KeyValuePairs( Dictionary );
	}
}