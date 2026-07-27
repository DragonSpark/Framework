using System.Windows.Input;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public interface IActionRegistration : DragonSpark.Text.IText, ISelect<string?, ICommand>;