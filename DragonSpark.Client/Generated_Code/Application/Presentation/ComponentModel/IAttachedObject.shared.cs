using System;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public interface IAttachedObject : System.Windows.Interactivity.IAttachedObject
    {
        event EventHandler Attached;
        event EventHandler Detached;
    }
}