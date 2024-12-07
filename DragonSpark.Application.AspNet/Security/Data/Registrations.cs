﻿using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Data;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IDataProtector>()
		         .Use<DataProtectorInstance>()
		         .Singleton()
		         //
		         .Then.Start<CertificateBasedDataProtector>()
		         .Singleton()
		         //
		         .Then.Start<IEncrypt>()
		         .Forward<Encrypt>()
		         .Singleton()
		         //
		         .Then.Start<IEncryptText>()
		         .Forward<EncryptText>()
		         .Singleton()
		         //
		         .Then.Start<IDecrypt>()
		         .Forward<Decrypt>()
		         .Singleton()
		         //
		         .Then.Start<IDecryptText>()
		         .Forward<DecryptText>()
		         .Singleton()
			;
	}
}