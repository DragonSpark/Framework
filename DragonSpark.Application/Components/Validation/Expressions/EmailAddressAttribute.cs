using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace DragonSpark.Application.Components.Validation.Expressions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmailAddressAttribute : DataTypeAttribute
{
	public EmailAddressAttribute() : base(DataType.EmailAddress) {}

	public override bool IsValid(object? value) => value is string candidate && MailAddress.TryCreate(candidate, out _);
}