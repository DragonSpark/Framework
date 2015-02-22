using System;

namespace DragonSpark.Application.Forms.Rendering
{
	internal sealed class MD5CryptoServiceProvider : MD5
	{
		private const int BLOCK_SIZE_BYTES = 64;
		private uint[] _H;
		private uint[] buff;
		private ulong count;
		private byte[] _ProcessingBuffer;
		private int _ProcessingBufferCount;
		private static readonly uint[] K = new uint[]
		{
			3614090360u,
			3905402710u,
			606105819u,
			3250441966u,
			4118548399u,
			1200080426u,
			2821735955u,
			4249261313u,
			1770035416u,
			2336552879u,
			4294925233u,
			2304563134u,
			1804603682u,
			4254626195u,
			2792965006u,
			1236535329u,
			4129170786u,
			3225465664u,
			643717713u,
			3921069994u,
			3593408605u,
			38016083u,
			3634488961u,
			3889429448u,
			568446438u,
			3275163606u,
			4107603335u,
			1163531501u,
			2850285829u,
			4243563512u,
			1735328473u,
			2368359562u,
			4294588738u,
			2272392833u,
			1839030562u,
			4259657740u,
			2763975236u,
			1272893353u,
			4139469664u,
			3200236656u,
			681279174u,
			3936430074u,
			3572445317u,
			76029189u,
			3654602809u,
			3873151461u,
			530742520u,
			3299628645u,
			4096336452u,
			1126891415u,
			2878612391u,
			4237533241u,
			1700485571u,
			2399980690u,
			4293915773u,
			2240044497u,
			1873313359u,
			4264355552u,
			2734768916u,
			1309151649u,
			4149444226u,
			3174756917u,
			718787259u,
			3951481745u
		};
		public MD5CryptoServiceProvider()
		{
			this._H = new uint[4];
			this.buff = new uint[16];
			this._ProcessingBuffer = new byte[64];
			this.Initialize();
		}
		~MD5CryptoServiceProvider()
		{
			this.Dispose(false);
		}
		protected override void Dispose(bool disposing)
		{
			if (this._ProcessingBuffer != null)
			{
				Array.Clear(this._ProcessingBuffer, 0, this._ProcessingBuffer.Length);
				this._ProcessingBuffer = null;
			}
			if (this._H != null)
			{
				Array.Clear(this._H, 0, this._H.Length);
				this._H = null;
			}
			if (this.buff != null)
			{
				Array.Clear(this.buff, 0, this.buff.Length);
				this.buff = null;
			}
		}
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			this.State = 1;
			if (this._ProcessingBufferCount != 0)
			{
				if (cbSize < 64 - this._ProcessingBufferCount)
				{
					Buffer.BlockCopy(rgb, ibStart, this._ProcessingBuffer, this._ProcessingBufferCount, cbSize);
					this._ProcessingBufferCount += cbSize;
					return;
				}
				int i = 64 - this._ProcessingBufferCount;
				Buffer.BlockCopy(rgb, ibStart, this._ProcessingBuffer, this._ProcessingBufferCount, i);
				this.ProcessBlock(this._ProcessingBuffer, 0);
				this._ProcessingBufferCount = 0;
				ibStart += i;
				cbSize -= i;
			}
			for (int i = 0; i < cbSize - cbSize % 64; i += 64)
			{
				this.ProcessBlock(rgb, ibStart + i);
			}
			if (cbSize % 64 != 0)
			{
				Buffer.BlockCopy(rgb, cbSize - cbSize % 64 + ibStart, this._ProcessingBuffer, 0, cbSize % 64);
				this._ProcessingBufferCount = cbSize % 64;
			}
		}
		protected override byte[] HashFinal()
		{
			byte[] array = new byte[16];
			this.ProcessFinalBlock(this._ProcessingBuffer, 0, this._ProcessingBufferCount);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = (byte)(this._H[i] >> j * 8);
				}
			}
			return array;
		}
		public override void Initialize()
		{
			this.count = 0uL;
			this._ProcessingBufferCount = 0;
			this._H[0] = 1732584193u;
			this._H[1] = 4023233417u;
			this._H[2] = 2562383102u;
			this._H[3] = 271733878u;
		}
		private void ProcessBlock(byte[] inputBuffer, int inputOffset)
		{
			this.count += 64uL;
			for (int i = 0; i < 16; i++)
			{
				this.buff[i] = (uint)((int)inputBuffer[inputOffset + 4 * i] | (int)inputBuffer[inputOffset + 4 * i + 1] << 8 | (int)inputBuffer[inputOffset + 4 * i + 2] << 16 | (int)inputBuffer[inputOffset + 4 * i + 3] << 24);
			}
			uint num = this._H[0];
			uint num2 = this._H[1];
			uint num3 = this._H[2];
			uint num4 = this._H[3];
			num += (((num3 ^ num4) & num2) ^ num4) + MD5CryptoServiceProvider.K[0] + this.buff[0];
			num = (num << 7 | num >> 25);
			num += num2;
			num4 += (((num2 ^ num3) & num) ^ num3) + MD5CryptoServiceProvider.K[1] + this.buff[1];
			num4 = (num4 << 12 | num4 >> 20);
			num4 += num;
			num3 += (((num ^ num2) & num4) ^ num2) + MD5CryptoServiceProvider.K[2] + this.buff[2];
			num3 = (num3 << 17 | num3 >> 15);
			num3 += num4;
			num2 += (((num4 ^ num) & num3) ^ num) + MD5CryptoServiceProvider.K[3] + this.buff[3];
			num2 = (num2 << 22 | num2 >> 10);
			num2 += num3;
			num += (((num3 ^ num4) & num2) ^ num4) + MD5CryptoServiceProvider.K[4] + this.buff[4];
			num = (num << 7 | num >> 25);
			num += num2;
			num4 += (((num2 ^ num3) & num) ^ num3) + MD5CryptoServiceProvider.K[5] + this.buff[5];
			num4 = (num4 << 12 | num4 >> 20);
			num4 += num;
			num3 += (((num ^ num2) & num4) ^ num2) + MD5CryptoServiceProvider.K[6] + this.buff[6];
			num3 = (num3 << 17 | num3 >> 15);
			num3 += num4;
			num2 += (((num4 ^ num) & num3) ^ num) + MD5CryptoServiceProvider.K[7] + this.buff[7];
			num2 = (num2 << 22 | num2 >> 10);
			num2 += num3;
			num += (((num3 ^ num4) & num2) ^ num4) + MD5CryptoServiceProvider.K[8] + this.buff[8];
			num = (num << 7 | num >> 25);
			num += num2;
			num4 += (((num2 ^ num3) & num) ^ num3) + MD5CryptoServiceProvider.K[9] + this.buff[9];
			num4 = (num4 << 12 | num4 >> 20);
			num4 += num;
			num3 += (((num ^ num2) & num4) ^ num2) + MD5CryptoServiceProvider.K[10] + this.buff[10];
			num3 = (num3 << 17 | num3 >> 15);
			num3 += num4;
			num2 += (((num4 ^ num) & num3) ^ num) + MD5CryptoServiceProvider.K[11] + this.buff[11];
			num2 = (num2 << 22 | num2 >> 10);
			num2 += num3;
			num += (((num3 ^ num4) & num2) ^ num4) + MD5CryptoServiceProvider.K[12] + this.buff[12];
			num = (num << 7 | num >> 25);
			num += num2;
			num4 += (((num2 ^ num3) & num) ^ num3) + MD5CryptoServiceProvider.K[13] + this.buff[13];
			num4 = (num4 << 12 | num4 >> 20);
			num4 += num;
			num3 += (((num ^ num2) & num4) ^ num2) + MD5CryptoServiceProvider.K[14] + this.buff[14];
			num3 = (num3 << 17 | num3 >> 15);
			num3 += num4;
			num2 += (((num4 ^ num) & num3) ^ num) + MD5CryptoServiceProvider.K[15] + this.buff[15];
			num2 = (num2 << 22 | num2 >> 10);
			num2 += num3;
			num += (((num2 ^ num3) & num4) ^ num3) + MD5CryptoServiceProvider.K[16] + this.buff[1];
			num = (num << 5 | num >> 27);
			num += num2;
			num4 += (((num ^ num2) & num3) ^ num2) + MD5CryptoServiceProvider.K[17] + this.buff[6];
			num4 = (num4 << 9 | num4 >> 23);
			num4 += num;
			num3 += (((num4 ^ num) & num2) ^ num) + MD5CryptoServiceProvider.K[18] + this.buff[11];
			num3 = (num3 << 14 | num3 >> 18);
			num3 += num4;
			num2 += (((num3 ^ num4) & num) ^ num4) + MD5CryptoServiceProvider.K[19] + this.buff[0];
			num2 = (num2 << 20 | num2 >> 12);
			num2 += num3;
			num += (((num2 ^ num3) & num4) ^ num3) + MD5CryptoServiceProvider.K[20] + this.buff[5];
			num = (num << 5 | num >> 27);
			num += num2;
			num4 += (((num ^ num2) & num3) ^ num2) + MD5CryptoServiceProvider.K[21] + this.buff[10];
			num4 = (num4 << 9 | num4 >> 23);
			num4 += num;
			num3 += (((num4 ^ num) & num2) ^ num) + MD5CryptoServiceProvider.K[22] + this.buff[15];
			num3 = (num3 << 14 | num3 >> 18);
			num3 += num4;
			num2 += (((num3 ^ num4) & num) ^ num4) + MD5CryptoServiceProvider.K[23] + this.buff[4];
			num2 = (num2 << 20 | num2 >> 12);
			num2 += num3;
			num += (((num2 ^ num3) & num4) ^ num3) + MD5CryptoServiceProvider.K[24] + this.buff[9];
			num = (num << 5 | num >> 27);
			num += num2;
			num4 += (((num ^ num2) & num3) ^ num2) + MD5CryptoServiceProvider.K[25] + this.buff[14];
			num4 = (num4 << 9 | num4 >> 23);
			num4 += num;
			num3 += (((num4 ^ num) & num2) ^ num) + MD5CryptoServiceProvider.K[26] + this.buff[3];
			num3 = (num3 << 14 | num3 >> 18);
			num3 += num4;
			num2 += (((num3 ^ num4) & num) ^ num4) + MD5CryptoServiceProvider.K[27] + this.buff[8];
			num2 = (num2 << 20 | num2 >> 12);
			num2 += num3;
			num += (((num2 ^ num3) & num4) ^ num3) + MD5CryptoServiceProvider.K[28] + this.buff[13];
			num = (num << 5 | num >> 27);
			num += num2;
			num4 += (((num ^ num2) & num3) ^ num2) + MD5CryptoServiceProvider.K[29] + this.buff[2];
			num4 = (num4 << 9 | num4 >> 23);
			num4 += num;
			num3 += (((num4 ^ num) & num2) ^ num) + MD5CryptoServiceProvider.K[30] + this.buff[7];
			num3 = (num3 << 14 | num3 >> 18);
			num3 += num4;
			num2 += (((num3 ^ num4) & num) ^ num4) + MD5CryptoServiceProvider.K[31] + this.buff[12];
			num2 = (num2 << 20 | num2 >> 12);
			num2 += num3;
			num += (num2 ^ num3 ^ num4) + MD5CryptoServiceProvider.K[32] + this.buff[5];
			num = (num << 4 | num >> 28);
			num += num2;
			num4 += (num ^ num2 ^ num3) + MD5CryptoServiceProvider.K[33] + this.buff[8];
			num4 = (num4 << 11 | num4 >> 21);
			num4 += num;
			num3 += (num4 ^ num ^ num2) + MD5CryptoServiceProvider.K[34] + this.buff[11];
			num3 = (num3 << 16 | num3 >> 16);
			num3 += num4;
			num2 += (num3 ^ num4 ^ num) + MD5CryptoServiceProvider.K[35] + this.buff[14];
			num2 = (num2 << 23 | num2 >> 9);
			num2 += num3;
			num += (num2 ^ num3 ^ num4) + MD5CryptoServiceProvider.K[36] + this.buff[1];
			num = (num << 4 | num >> 28);
			num += num2;
			num4 += (num ^ num2 ^ num3) + MD5CryptoServiceProvider.K[37] + this.buff[4];
			num4 = (num4 << 11 | num4 >> 21);
			num4 += num;
			num3 += (num4 ^ num ^ num2) + MD5CryptoServiceProvider.K[38] + this.buff[7];
			num3 = (num3 << 16 | num3 >> 16);
			num3 += num4;
			num2 += (num3 ^ num4 ^ num) + MD5CryptoServiceProvider.K[39] + this.buff[10];
			num2 = (num2 << 23 | num2 >> 9);
			num2 += num3;
			num += (num2 ^ num3 ^ num4) + MD5CryptoServiceProvider.K[40] + this.buff[13];
			num = (num << 4 | num >> 28);
			num += num2;
			num4 += (num ^ num2 ^ num3) + MD5CryptoServiceProvider.K[41] + this.buff[0];
			num4 = (num4 << 11 | num4 >> 21);
			num4 += num;
			num3 += (num4 ^ num ^ num2) + MD5CryptoServiceProvider.K[42] + this.buff[3];
			num3 = (num3 << 16 | num3 >> 16);
			num3 += num4;
			num2 += (num3 ^ num4 ^ num) + MD5CryptoServiceProvider.K[43] + this.buff[6];
			num2 = (num2 << 23 | num2 >> 9);
			num2 += num3;
			num += (num2 ^ num3 ^ num4) + MD5CryptoServiceProvider.K[44] + this.buff[9];
			num = (num << 4 | num >> 28);
			num += num2;
			num4 += (num ^ num2 ^ num3) + MD5CryptoServiceProvider.K[45] + this.buff[12];
			num4 = (num4 << 11 | num4 >> 21);
			num4 += num;
			num3 += (num4 ^ num ^ num2) + MD5CryptoServiceProvider.K[46] + this.buff[15];
			num3 = (num3 << 16 | num3 >> 16);
			num3 += num4;
			num2 += (num3 ^ num4 ^ num) + MD5CryptoServiceProvider.K[47] + this.buff[2];
			num2 = (num2 << 23 | num2 >> 9);
			num2 += num3;
			num += ((~num4 | num2) ^ num3) + MD5CryptoServiceProvider.K[48] + this.buff[0];
			num = (num << 6 | num >> 26);
			num += num2;
			num4 += ((~num3 | num) ^ num2) + MD5CryptoServiceProvider.K[49] + this.buff[7];
			num4 = (num4 << 10 | num4 >> 22);
			num4 += num;
			num3 += ((~num2 | num4) ^ num) + MD5CryptoServiceProvider.K[50] + this.buff[14];
			num3 = (num3 << 15 | num3 >> 17);
			num3 += num4;
			num2 += ((~num | num3) ^ num4) + MD5CryptoServiceProvider.K[51] + this.buff[5];
			num2 = (num2 << 21 | num2 >> 11);
			num2 += num3;
			num += ((~num4 | num2) ^ num3) + MD5CryptoServiceProvider.K[52] + this.buff[12];
			num = (num << 6 | num >> 26);
			num += num2;
			num4 += ((~num3 | num) ^ num2) + MD5CryptoServiceProvider.K[53] + this.buff[3];
			num4 = (num4 << 10 | num4 >> 22);
			num4 += num;
			num3 += ((~num2 | num4) ^ num) + MD5CryptoServiceProvider.K[54] + this.buff[10];
			num3 = (num3 << 15 | num3 >> 17);
			num3 += num4;
			num2 += ((~num | num3) ^ num4) + MD5CryptoServiceProvider.K[55] + this.buff[1];
			num2 = (num2 << 21 | num2 >> 11);
			num2 += num3;
			num += ((~num4 | num2) ^ num3) + MD5CryptoServiceProvider.K[56] + this.buff[8];
			num = (num << 6 | num >> 26);
			num += num2;
			num4 += ((~num3 | num) ^ num2) + MD5CryptoServiceProvider.K[57] + this.buff[15];
			num4 = (num4 << 10 | num4 >> 22);
			num4 += num;
			num3 += ((~num2 | num4) ^ num) + MD5CryptoServiceProvider.K[58] + this.buff[6];
			num3 = (num3 << 15 | num3 >> 17);
			num3 += num4;
			num2 += ((~num | num3) ^ num4) + MD5CryptoServiceProvider.K[59] + this.buff[13];
			num2 = (num2 << 21 | num2 >> 11);
			num2 += num3;
			num += ((~num4 | num2) ^ num3) + MD5CryptoServiceProvider.K[60] + this.buff[4];
			num = (num << 6 | num >> 26);
			num += num2;
			num4 += ((~num3 | num) ^ num2) + MD5CryptoServiceProvider.K[61] + this.buff[11];
			num4 = (num4 << 10 | num4 >> 22);
			num4 += num;
			num3 += ((~num2 | num4) ^ num) + MD5CryptoServiceProvider.K[62] + this.buff[2];
			num3 = (num3 << 15 | num3 >> 17);
			num3 += num4;
			num2 += ((~num | num3) ^ num4) + MD5CryptoServiceProvider.K[63] + this.buff[9];
			num2 = (num2 << 21 | num2 >> 11);
			num2 += num3;
			this._H[0] += num;
			this._H[1] += num2;
			this._H[2] += num3;
			this._H[3] += num4;
		}
		private void ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			ulong num = this.count + (ulong)((long)inputCount);
			int num2 = (int)(56uL - num % 64uL);
			if (num2 < 1)
			{
				num2 += 64;
			}
			byte[] array = new byte[inputCount + num2 + 8];
			for (int i = 0; i < inputCount; i++)
			{
				array[i] = inputBuffer[i + inputOffset];
			}
			array[inputCount] = 128;
			for (int j = inputCount + 1; j < inputCount + num2; j++)
			{
				array[j] = 0;
			}
			ulong length = num << 3;
			this.AddLength(length, array, inputCount + num2);
			this.ProcessBlock(array, 0);
			if (inputCount + num2 + 8 == 128)
			{
				this.ProcessBlock(array, 64);
			}
		}
		internal void AddLength(ulong length, byte[] buffer, int position)
		{
			buffer[position++] = (byte)length;
			buffer[position++] = (byte)(length >> 8);
			buffer[position++] = (byte)(length >> 16);
			buffer[position++] = (byte)(length >> 24);
			buffer[position++] = (byte)(length >> 32);
			buffer[position++] = (byte)(length >> 40);
			buffer[position++] = (byte)(length >> 48);
			buffer[position] = (byte)(length >> 56);
		}
	}
}
