using DragonSpark.Commands;
using JetBrains.Annotations;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	public class ApplyExportsCommand<T> : CommandBase<IExportProvider> where T : class, ICommand
	{
		public static ApplyExportsCommand<T> Default { get; } = new ApplyExportsCommand<T>();
		ApplyExportsCommand() {}
		/*[Required, Service, UsedImplicitly]
		public IExportProvider Exports { [return: Required]get; set; }*/

		[UsedImplicitly]
		public string ContractName { get; set; }

		public override void Execute( IExportProvider parameter )
		{
			var exports = parameter.GetExports<T>( ContractName );
			
			foreach ( var export in exports )
			{
				export.Execute( parameter );
			}
		}
	}
}