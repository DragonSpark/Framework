using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class AccessPlatform : TableVariable<object, object>
{
    public AccessPlatform(HostBuilderContext context) : this(context.Properties.ToTable()!) {}

    public AccessPlatform(ITable<object, object?> store)
        : base(A.Type<AccessPlatform>().FullName.Verify(), store) {}
}