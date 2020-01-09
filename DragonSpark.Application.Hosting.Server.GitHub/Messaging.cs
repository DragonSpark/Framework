using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	sealed class IsIssueComment : IsAuthenticatedPayload
	{
		public static IsIssueComment Default { get; } = new IsIssueComment();

		IsIssueComment() : base("issue_comment") {}
	}

	sealed class EventMessages<T> : Select<EventMessage, T>
	{
		public static EventMessages<T> Default { get; } = new EventMessages<T>();

		EventMessages() : base(Start.A.Selection<EventMessage>()
		                            .By.Calling(x => x.Payload)
		                            .Select(Serializer.Default.Get().Deserialize<T>)) {}
	}

	public class IsAuthenticatedPayload : IsPayload
	{
		public IsAuthenticatedPayload(string name) : base(new IsAuthenticatedMessage(name)) {}
	}

	public class IsPayload : Condition<EventMessage>
	{
		public IsPayload(ICondition<EventMessage> condition) : base(condition) {}
	}

	sealed class IsAuthenticatedMessage : AllCondition<EventMessage>
	{
		public IsAuthenticatedMessage(string key)
			: base(Start.A.Selection<EventMessage>()
			            .By.Calling(x => x.Header.Event)
			            .Select(Is.EqualTo(key)),
			       Start.A.Condition<EventMessage>(Is.Of<AuthenticatedEventMessage>())
			            .Ensure.Input.IsAssigned.Otherwise.Throw(AuthenticatedMessage.Default)
			      ) {}
	}

	sealed class AuthenticatedMessage : Message<EventMessage>
	{
		public static AuthenticatedMessage Default { get; } = new AuthenticatedMessage();

		AuthenticatedMessage()
			: base(_ => "An authenticated event message was expected, but the provided message was not properly authenticated.") {}
	}

	[ModelBinder(BinderType = typeof(EventMessageBinder))]
	public class EventMessage
	{
		public EventMessage(EventHeader header, string payload)
		{
			Header  = header;
			Payload = payload;
		}

		public EventHeader Header { get; }

		public string Payload { get; }
	}

	public sealed class AuthenticatedEventMessage : EventMessage
	{
		public AuthenticatedEventMessage(EventHeader header, string payload) : base(header, payload) {}
	}

	public sealed class GitHubApplicationSettings
	{
		public uint Id { get; set; }

		public string Token { get; set; }

		public string Key { get; set; }
	}

	sealed class Hasher : IAlteration<string>
	{
		readonly Array<byte> _token;

		public Hasher(GitHubApplicationSettings settings)
			: this(new Array<byte>(Encoding.UTF8.GetBytes(settings.Token))) {}

		public Hasher(Array<byte> token) => _token = token;

		public string Get(string parameter)
		{
			using var sha    = new HMACSHA1(_token);
			var       hash   = sha.ComputeHash(Encoding.UTF8.GetBytes(parameter));
			var       result = $"sha1={BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant()}";
			return result;
		}
	}

	public sealed class EventHeader
	{
		public EventHeader(string @event, string delivery)
		{
			Event    = @event;
			Delivery = delivery;
		}

		public string Event { get; }

		public string Delivery { get; }
	}

	readonly struct PropertyView
	{
		public PropertyView(string @event, string delivery, string signature)
		{
			Event     = @event;
			Delivery  = delivery;
			Signature = signature;
		}

		public string Event { get; }

		public string Delivery { get; }

		public string Signature { get; }
	}

	sealed class PropertyViews : ISelect<IDictionary<string, StringValues>, PropertyView>
	{
		public static PropertyViews Default { get; } = new PropertyViews();

		PropertyViews() : this(EventName.Default.Get, DeliveryId.Default.Get, Signature.Default.Get) {}

		readonly Access _event, _deliveryId, _signature;

		public PropertyViews(Access @event, Access deliveryId, Access signature)
		{
			_event      = @event;
			_deliveryId = deliveryId;
			_signature  = signature;
		}

		public PropertyView Get(IDictionary<string, StringValues> parameter)
			=> new PropertyView(_event(parameter), _deliveryId(parameter), _signature(parameter));
	}

	public delegate string Access(IDictionary<string, StringValues> parameter);

	class Property : ISelect<IDictionary<string, StringValues>, string>
	{
		readonly string _name;

		public Property(string name) => _name = name;

		public string Get(IDictionary<string, StringValues> parameter)
			=> parameter.TryGetValue(_name, out var result) ? (string)result : null;
	}

	sealed class Signature : Property
	{
		public static Signature Default { get; } = new Signature();

		Signature() : base("X-hub-signature") {}
	}

	sealed class DeliveryId : Property
	{
		public static DeliveryId Default { get; } = new DeliveryId();

		DeliveryId() : base("x-gitHub-delivery") {}
	}

	sealed class EventName : Property
	{
		public static EventName Default { get; } = new EventName();

		EventName() : base("x-github-event") {}
	}

	sealed class EventMessages : IOperationResult<HttpRequest, EventMessage>
	{
		readonly Hasher        _hasher;
		readonly PropertyViews _contexts;

		public EventMessages(Hasher hasher) : this(hasher, PropertyViews.Default) {}

		public EventMessages(Hasher hasher, PropertyViews contexts)
		{
			_hasher   = hasher;
			_contexts = contexts;
		}

		public async ValueTask<EventMessage> Get(HttpRequest parameter)
		{
			using var reader        = new StreamReader(parameter.Body);
			var       content       = await reader.ReadToEndAsync();
			var       context       = _contexts.Get(parameter.Headers);
			var       header        = new EventHeader(context.Event, context.Delivery);
			var       authenticated = _hasher.Get(content) == context.Signature;
			var result = authenticated
				             ? new AuthenticatedEventMessage(header, content)
				             : new EventMessage(header, content);
			return result;
		}
	}

	sealed class EventMessageBinder : IModelBinder
	{
		readonly Func<HttpRequest, ValueTask<EventMessage>> _messages;

		public EventMessageBinder(EventMessages messages) : this(messages.Get) {}

		EventMessageBinder(Func<HttpRequest, ValueTask<EventMessage>> messages) => _messages = messages;

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var message = await _messages(bindingContext.HttpContext.Request);

			bindingContext.Result = message != null
				                        ? ModelBindingResult.Success(message)
				                        : ModelBindingResult.Failed();
		}
	}
}