using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DragonSpark.Text
{
	public sealed class Utf8
	{
		const char HighSurrogateStart = '\ud800',
		           HighSurrogateEnd   = '\udbff',
		           LowSurrogateStart  = '\udc00',
		           LowSurrogateEnd    = '\udfff';
		public static Utf8 Default { get; } = new Utf8();

		Utf8() {}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Get(in ReadOnlySpan<char> source, in Span<byte> destination)
		{
			var status = Get(source, destination, out var result);
			return status == OperationStatus.Done
				       ? result
				       : throw new
					         InvalidOperationException($"[{status}] Could not successfully convert value to Utf-8 data: {source.ToString()}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public OperationStatus Get(in ReadOnlySpan<char> source, in Span<byte> destination, out int written)
			=> Get(MemoryMarshal.AsBytes(source), destination, out written);

		// ReSharper disable once CyclomaticComplexity
		// ReSharper disable once MethodTooLong
		// ReSharper disable once CognitiveComplexity
		/// <summary>
		/// ATTRIBUTION: https://github.com/dotnet/corefx/blob/38e5e28646687da306ad1f3e3fc9876e67e031bb/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Transcoding.cs#L29
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="written"></param>
		/// <returns></returns>
		public static unsafe OperationStatus Get(in ReadOnlySpan<byte> source, in Span<byte> destination, out int written)
		{
			fixed (byte* chars = &MemoryMarshal.GetReference(source))
			fixed (byte* bytes = &MemoryMarshal.GetReference(destination))
			{
				var pSrc    = (char*)chars;
				var pTarget = bytes;

				var pEnd                = (char*)(chars + source.Length);
				var pAllocatedBufferEnd = pTarget + destination.Length;

				while (pEnd - pSrc > 13)
				{
					var available = Math.Min((int)((uint)((byte*)pEnd - (byte*)pSrc) >> 1),
					                         (int)(pAllocatedBufferEnd - pTarget));

					var pStop = pSrc + available - 5;
					if (pSrc >= pStop)
						break;

					do
					{
						int ch = *pSrc;
						pSrc++;

						if (ch > 0x7F)
						{
							goto LongCode;
						}

						*pTarget = (byte)ch;
						pTarget++;

						if (((int)pSrc & 0x2) != 0)
						{
							ch = *pSrc;
							pSrc++;
							if (ch > 0x7F)
							{
								goto LongCode;
							}

							*pTarget = (byte)ch;
							pTarget++;
						}

						while (pSrc < pStop)
						{
							ch = *(int*)pSrc;
							var chc = *(int*)(pSrc + 2);
							if (((ch | chc) & unchecked((int)0xFF80FF80)) != 0)
							{
								goto LongCodeWithMask;
							}

							*pTarget       =  (byte)ch;
							*(pTarget + 1) =  (byte)(ch >> 16);
							pSrc           += 4;
							*(pTarget + 2) =  (byte)chc;
							*(pTarget + 3) =  (byte)(chc >> 16);
							pTarget        += 4;
						}

						continue;

						LongCodeWithMask:
						ch = (char)ch;
						pSrc++;

						if (ch > 0x7F)
						{
							goto LongCode;
						}

						*pTarget = (byte)ch;
						pTarget++;
						continue;

						LongCode:
						int chd;
						if (ch <= 0x7FF)
						{
							chd = unchecked((sbyte)0xC0) | (ch >> 6);
						}
						else
						{
							if (!IsOrWithin(ch, HighSurrogateStart, LowSurrogateEnd))
							{
								chd = unchecked((sbyte)0xE0) | (ch >> 12);
							}
							else
							{
								if (ch > HighSurrogateEnd)
								{
									goto InvalidData;
								}

								chd = *pSrc;

								if (!IsOrWithin(chd, LowSurrogateStart, LowSurrogateEnd))
								{
									goto InvalidData;
								}

								pSrc++;

								ch = chd + (ch << 10) +
								     (0x10000
								      - LowSurrogateStart
								      - (HighSurrogateStart << 10));

								*pTarget = (byte)(unchecked((sbyte)0xF0) | (ch >> 18));
								pTarget++;

								chd = unchecked((sbyte)0x80) | (ch >> 12) & 0x3F;
							}

							*pTarget = (byte)chd;
							pStop--;
							pTarget++;

							chd = unchecked((sbyte)0x80) | (ch >> 6) & 0x3F;
						}

						*pTarget = (byte)chd;
						pStop--;

						*(pTarget + 1) = (byte)(unchecked((sbyte)0x80) | ch & 0x3F);

						pTarget += 2;
					} while (pSrc < pStop);
				}

				while (pSrc < pEnd)
				{
					int ch = *pSrc;
					pSrc++;

					if (ch <= 0x7F)
					{
						if (pAllocatedBufferEnd - pTarget <= 0)
							goto DestinationFull;

						*pTarget = (byte)ch;
						pTarget++;
						continue;
					}

					int chd;
					if (ch <= 0x7FF)
					{
						if (pAllocatedBufferEnd - pTarget <= 1)
							goto DestinationFull;

						chd = unchecked((sbyte)0xC0) | (ch >> 6);
					}
					else
					{
						if (!IsOrWithin(ch, HighSurrogateStart, LowSurrogateEnd))
						{
							if (pAllocatedBufferEnd - pTarget <= 2)
								goto DestinationFull;

							chd = unchecked((sbyte)0xE0) | (ch >> 12);
						}
						else
						{
							if (pAllocatedBufferEnd - pTarget <= 3)
								goto DestinationFull;

							if (ch > HighSurrogateEnd)
							{
								goto InvalidData;
							}

							if (pSrc >= pEnd)
								goto NeedMoreData;

							chd = *pSrc;

							if (!IsOrWithin(chd, LowSurrogateStart, LowSurrogateEnd))
							{
								goto InvalidData;
							}

							pSrc++;

							ch = chd + (ch << 10) +
							     (0x10000
							      - LowSurrogateStart
							      - (HighSurrogateStart << 10));

							*pTarget = (byte)(unchecked((sbyte)0xF0) | (ch >> 18));
							pTarget++;

							chd = unchecked((sbyte)0x80) | (ch >> 12) & 0x3F;
						}

						*pTarget = (byte)chd;
						pTarget++;

						chd = unchecked((sbyte)0x80) | (ch >> 6) & 0x3F;
					}

					*pTarget       = (byte)chd;
					*(pTarget + 1) = (byte)(unchecked((sbyte)0x80) | ch & 0x3F);

					pTarget += 2;
				}

				written = (int)(pTarget - bytes);
				return OperationStatus.Done;

				InvalidData:
				written = (int)(pTarget - bytes);
				return OperationStatus.InvalidData;

				DestinationFull:
				written = (int)(pTarget - bytes);
				return OperationStatus.DestinationTooSmall;

				NeedMoreData:
				written = (int)(pTarget - bytes);
				return OperationStatus.NeedMoreData;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsOrWithin(int value, int lowerBound, int upperBound)
			=> (uint)(value - lowerBound) <= (uint)(upperBound - lowerBound);
	}
}