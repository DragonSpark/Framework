using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using JetBrains.Annotations;
using PostSharp.Patterns.Contracts;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	public class ApplyExportCommands<T> : CommandBase<object> where T : class, ICommand
	{
		[Required, Service, UsedImplicitly]
		public IExportProvider Exports { [return: Required]get; set; }

		[UsedImplicitly]
		public string ContractName { get; set; }

		public override void Execute( object parameter )
		{
			var exports = Exports.GetExports<T>( ContractName );
			
			foreach ( var export in exports )
			{
				export.Execute( parameter );
			}
		}
	}
}