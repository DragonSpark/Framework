using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using System.Text;

namespace DragonSpark.Application.Security.Data;

public sealed class DecryptDataAsText : Alteration<string>
{
	public DecryptDataAsText(IDecrypt select) : this(select, Encoding.UTF8) {}

	public DecryptDataAsText(IDecrypt select, Encoding encoding)
		: base(TextAsData.Default.Then().Subject.Select(select).Select(encoding.GetString)) {}
}