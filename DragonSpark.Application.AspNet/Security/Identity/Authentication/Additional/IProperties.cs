using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IProperties : ISelect<object, IEnumerable<KeyValuePair<string, string?>>>;