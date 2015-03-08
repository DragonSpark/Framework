using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public class ViewType : UnityType
    {
        public override void Configure( IUnityContainer container )
        {
            var type = RegistrationType ?? MapTo;
            RegistrationType = typeof(object);
            MapTo = type;
            BuildName = type.FullName;
            base.Configure( container );
        }
    }
}