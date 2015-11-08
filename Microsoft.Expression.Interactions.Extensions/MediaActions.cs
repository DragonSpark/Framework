// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Microsoft.Expression.Interactions.Extensions
{
	/// <summary>
	/// Play a media element.
	/// </summary>
	public class PlayMedia : TargetedTriggerAction<MediaElement>
	{
		/// <summary>
		/// Play the media.
		/// </summary>
		/// <param name="o"></param>
		protected override void Invoke(object o) {
			if (this.Target != null) {
				this.Target.Play();
			}
		}
	}

	/// <summary>
	/// Pause a media element.
	/// </summary>
	public class PauseMedia : TargetedTriggerAction<MediaElement> {
		/// <summary>
		/// Pause the media
		/// </summary>
		/// <param name="o"></param>
		protected override void Invoke(object o) {
			if (this.Target != null) {
				this.Target.Pause();
			}
		}
	}
#if SILVERLIGHT
	/// <summary>
	/// Togggle between play/pause for a media element.
	/// </summary>
	public class TogglePlayPauseMedia : TargetedTriggerAction<MediaElement> {
		/// <summary>
		/// Toggle between play and pause.
		/// </summary>
		/// <param name="o"></param>
		protected override void Invoke(object o) {
			if (this.Target != null) {
				if (this.Target.CurrentState == MediaElementState.Playing)
					this.Target.Pause();
				else
					this.Target.Play();
			}
		}
	}
#endif

	/// <summary>
	/// Seeks a media element to position 0.
	/// </summary>
	public class RewindMedia : TargetedTriggerAction<MediaElement> {

		/// <summary>
		/// Rewind the media
		/// </summary>
		/// <param name="o"></param>
		protected override void Invoke(object o) {
			if (this.Target != null) {
				this.Target.Position = TimeSpan.Zero;
			}
		}
	}

	/// <summary>
	/// Stops a media element.
	/// </summary>
	public class StopMedia : TargetedTriggerAction<MediaElement> {

		/// <summary>
		/// Stop the media.
		/// </summary>
		/// <param name="o"></param>
		protected override void Invoke(object o) {
			if (this.Target != null) {
				this.Target.Stop();
			}
		}
	}
}