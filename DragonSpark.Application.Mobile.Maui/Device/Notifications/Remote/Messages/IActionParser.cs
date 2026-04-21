using System.Windows.Input;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public interface IActionParser : ISelect<string, ICommand?>;