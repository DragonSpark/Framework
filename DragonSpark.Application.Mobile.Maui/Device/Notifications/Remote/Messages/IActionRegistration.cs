using System.Windows.Input;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public interface IActionRegistration : IText, ISelect<string?, ICommand>;