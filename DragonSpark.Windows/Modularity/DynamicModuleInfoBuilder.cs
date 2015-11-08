using System;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
    [Serializable]
    public class DynamicModuleInfoBuilder : ModuleInfoBuilder
    {
        public DynamicModuleInfoBuilder() : this( new CustomAttributeDataProvider() )
        {}

        public DynamicModuleInfoBuilder( IAttributeDataProvider provider ) : base( provider )
        {}

        /*protected DynamicModuleInfoBuilder( SerializationInfo info, StreamingContext context ) : this( (IAttributeDataProvider)info.GetValue( "provider", typeof(object) ) )
        {}

        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( "provider", Provider );
        }*/

        protected override ModuleInfo Create( Type host, string moduleName, string assemblyQualifiedName )
        {
            var result = new DynamicModuleInfo( moduleName, assemblyQualifiedName )
            {
                InitializationMode = Provider.Get<bool>( typeof(DynamicModuleAttribute), host, "OnDemand" )
                    ? InitializationMode.OnDemand
                    : InitializationMode.WhenAvailable,
                Ref = host.Assembly.CodeBase
            };
            return result;
        }
    }
}