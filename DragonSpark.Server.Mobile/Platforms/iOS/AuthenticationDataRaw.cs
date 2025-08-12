using System.Runtime.InteropServices;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct AuthenticationDataRaw
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] RpIdHash;
    public                               byte Flags;
    [MarshalAs(UnmanagedType.U4)] public uint Counter;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] Aaguid;
    [MarshalAs(UnmanagedType.U2)] public ushort CredentialIdLength;
}