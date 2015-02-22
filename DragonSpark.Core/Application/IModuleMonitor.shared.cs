using System;

namespace DragonSpark.Application
{
    public interface IModuleMonitor
    {
        event EventHandler Loaded;

        bool Load();

        void Mark( IModule target );
    }
}