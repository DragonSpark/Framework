using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public interface IProcessIntegrityToken : IStopAware<string, IntegrityTokenResult>;