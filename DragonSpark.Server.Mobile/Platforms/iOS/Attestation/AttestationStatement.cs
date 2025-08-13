using System.Security.Cryptography.X509Certificates;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record AttestationStatement(X509Certificate2 Certificate, Array<byte> Receipt);