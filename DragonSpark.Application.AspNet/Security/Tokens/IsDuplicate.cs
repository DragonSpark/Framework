using System;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Data.SqlClient;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class IsDuplicate : Condition<Exception>
{
    public static IsDuplicate Default { get; } = new();

    IsDuplicate() : base(x => x.InnerException is SqlException { Number: 2627 or 2601 }) {}
}