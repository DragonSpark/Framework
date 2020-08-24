using System;
using System.Net;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class ResourceExistsValidation : IValidatingValue<string>
	{
		public static ResourceExistsValidation Default { get; } = new ResourceExistsValidation();

		ResourceExistsValidation() : this(TimeSpan.FromMilliseconds(2500)) {}

		readonly TimeSpan _timeout;

		public ResourceExistsValidation(TimeSpan timeout) => _timeout = timeout;

		/// <summary>
		/// ATTRIBUTION: https://stackoverflow.com/questions/1979915/can-i-check-if-a-file-exists-at-a-url/12013240
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public async ValueTask<bool> Get(string parameter)
		{
			if (!string.IsNullOrEmpty(parameter))
			{
				var request = WebRequest.Create(parameter);
				request.Timeout = (int)_timeout.TotalMilliseconds;
				request.Method  = "HEAD";

				try
				{
					await request.GetResponseAsync();
				}
				catch
				{
					return false;
				}
			}

			return true;
		}
	}
}