using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public sealed class ResourceExistsValidation : IDepending<FieldIdentifier>
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
		public async ValueTask<bool> Get(FieldIdentifier parameter)
		{
			var input = parameter.GetValue<string>()?.Trim() ?? string.Empty;
			if (!string.IsNullOrEmpty(input))
			{
				var request = WebRequest.Create(input);
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