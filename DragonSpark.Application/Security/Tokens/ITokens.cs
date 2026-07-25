using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Tokens;

public interface ITokens : ISelect<Uri, string?>, ICommand<Pair<Uri, string>>, ICommand;