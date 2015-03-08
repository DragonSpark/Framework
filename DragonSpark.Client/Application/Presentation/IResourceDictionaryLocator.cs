using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace DragonSpark.Application.Presentation
{
    public interface IResourceDictionaryLocator
    {
        IEnumerable<ResourceDictionary> Locate( Assembly assembly );
    }
}