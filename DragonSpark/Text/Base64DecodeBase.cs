using System;
using System.Text;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text;

public abstract class Base64DecodeBase : Alteration<string>
{
    protected Base64DecodeBase(Func<string, byte[]> select) : this(select, Encoding.UTF8) {}

    protected Base64DecodeBase(Func<string, byte[]> select, Encoding encoding)
        : base(Start.A.Selection(select).Select(encoding.GetString)) {}
}