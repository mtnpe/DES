﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kur_phuong
{
    class DES
    {
		public uint getbit(uint K1, int i)
		{
			uint b = K1 >> (32 - i);
			uint bit = b & 0x01;
			return bit;
		}

		public uint getbit28(uint K1, int i)
		//get ith bit from left to right
		{
			uint b = K1 >> (28 - i);
			uint bit = b & 0x01;
			return bit;
		}


		public uint PC1CD(uint K1, uint K2, int chiso1, int chiso2)
		{
			uint[] pc1 = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
			uint pc1k1 = 0;
			for (int i = chiso1; i < chiso2; i++)
			{
				int vitri;
				uint bit;
				if (pc1[i] > 32)
				{
					vitri = (int)pc1[i] - 32;
					bit = getbit(K2, vitri);
				}
				else
				{
					vitri = (int)pc1[i];
					bit = getbit(K1, vitri);
				}
				uint b = bit & 0x01;
				pc1k1 = (pc1k1 << 1) | b;
			}
			return pc1k1;
		}

		public uint ShiftLeft(uint C0, int s)
		{
			uint C1;
			uint sbit, bit28s;
			sbit = (C0 >> (28 - s));
			bit28s = (C0 << s) & 0xFFFFFFF;
			C1 = bit28s | sbit;
			return C1;
		}

		public uint KPC2(uint C1, uint D1, int chiso1, int chiso2)
		{
			int[] pc2 = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
			uint pc2k = 0;
			for (int i = chiso1; i < chiso2; i++)
			{
				int vitri;
				uint bit;
				if (pc2[i] > 28)
				{
					vitri = pc2[i] - 28;
					bit = getbit28(D1, vitri);
				}
				else
				{
					vitri = pc2[i];
					bit = getbit28(C1, vitri);
				}

				uint b = bit & 0x01;
				pc2k = (pc2k << 1) | b;
			}
			return pc2k;
		}

		//tao 16 key
		public void GenKey(uint K1, uint K2, uint[] key1, uint[] key2)
		{
			uint C0, D0;
			C0 = PC1CD(K1, K2, 0, 28);
			D0 = PC1CD(K1, K2, 28, 56);
			int[] s = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
			//key1 ung voi s[0] la s1
			uint C1, D1;
			for (int i = 0; i < 16; i++)
			{
				C1 = ShiftLeft(C0, s[i]);
				D1 = ShiftLeft(D0, s[i]);
				key1[i] = KPC2(C1, D1, 0, 24);
				key2[i] = KPC2(C1, D1, 24, 48);
				C0 = C1; D0 = D1;
			}
		}

		//hoan vi IPM
		public uint IPM(uint M1, uint M2, int chiso1, int chiso2)
		{
			int[] IP = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
			uint ipm1 = 0;
			for (int i = chiso1; i < chiso2; i++)
			{
				int vitri;
				uint bit;
				if (IP[i] > 32)
				{
					vitri = IP[i] - 32;
					bit = getbit(M2, vitri);
				}
				else
				{
					vitri = IP[i];
					bit = getbit(M1, vitri);
				}
				uint b = bit & 0x01;
				ipm1 = (ipm1 << 1) | b;
			}
			return ipm1;
		}

		//ham mo rong R0 de co E[R0]
		public uint ER0(uint R0, int chiso1, int chiso2)
		{
			int[] E = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
			uint er1 = 0;
			for (int i = chiso1; i < chiso2; i++)
			{
				int vitri;
				uint bit;
				vitri = E[i];
				bit = getbit(R0, vitri);
				uint b = bit & 0x01;
				er1 = (er1 << 1) | b;
			}
			return er1;
		}

		public uint SubByte(uint A1, uint A2)
		{
			uint[] S1 = { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 };
			uint[] S2 = { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 };
			uint[] S3 = { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 };
			uint[] S4 = { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 };
			uint[] S5 = { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 };
			uint[] S6 = { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 };
			uint[] S7 = { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 };
			uint[] S8 = { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 };
			uint B;
			uint[] chiso = new uint[9];
			uint[] S = new uint[9];
			//4 cap 6 bit ben trai thuoc A1
			for (int i = 1; i <= 4; i++)
			{
				uint b6i = (A1 >> (24 - 6 * i)) & 0x3F;
				uint bit1 = (b6i >> 5) & 0x01;
				uint bit6 = b6i & 0x01;
				uint hang = bit1 << 1 | bit6;
				uint cot = (b6i >> 1) & 0xF;
				chiso[i] = (hang << 4) | cot;
			}
			//4 cap 6 bit ben phai thuoc A2
			for (int i = 5; i <= 8; i++)
			{
				uint b6i = (A2 >> (24 - 6 * (i - 4))) & 0x3F;
				uint bit1 = (b6i >> 5) & 0x01;
				uint bit6 = b6i & 0x01;
				uint hang = bit1 << 1 | bit6;
				uint cot = (b6i >> 1) & 0xF;
				chiso[i] = (hang << 4) | cot;
			}
			//Tra bang S
			S[1] = S1[chiso[1]]; S[2] = S2[chiso[2]];
			S[3] = S3[chiso[3]]; S[4] = S4[chiso[4]];
			S[5] = S5[chiso[5]]; S[6] = S6[chiso[6]];
			S[7] = S7[chiso[7]]; S[8] = S8[chiso[8]];
			//Ghep 8 cap 4 bit tao thanh B (32 bit)
			B = 0;
			for (int i = 1; i <= 8; i++)
				B = (B << 4) | S[i];
			return B;
		}

		public uint HoanviP(uint B)
		{
			int[] P = { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
			uint fp = 0;
			for (int i = 0; i < 32; i++)
			{
				int vitri;
				uint bit;
				vitri = P[i];
				bit = getbit(B, vitri);
				uint b = bit & 0x01;
				fp = (fp << 1) | b;
			}
			return fp;
		}

		public uint F(uint L0, uint R0, uint key1, uint key2)
		{
			//Buoc 4. Mo rong nua phai R0
			uint ER01, ER02;
			ER01 = ER0(R0, 0, 24);
			ER02 = ER0(R0, 24, 48);
			//Buoc 5. phép XOR, tính A = E[R0] + K1.
			uint A1, A2;
			A1 = key1 ^ ER01;
			A2 = key2 ^ ER02;
			//Buoc 6&7. B3: B = S1(A1) S2(A2) S3(A3) S4(A4) S5(A5) S6(A6) S7(A7) S8(A8)
			uint B;
			B = SubByte(A1, A2);
			//Buoc 8: Hoan vi P
			uint FP;
			FP = HoanviP(B);
			return FP;
		}

		//tinh ham hoan vi IP-1
		public uint HoanviIP_1(uint M1, uint M2, int chiso1, int chiso2)
		{
			int[] IP1 = { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
			uint ipm1 = 0;
			for (int i = chiso1; i < chiso2; i++)
			{
				int vitri;
				uint bit;
				if (IP1[i] > 32)
				{
					vitri = IP1[i] - 32;
					bit = getbit(M2, vitri);
				}
				else
				{
					vitri = IP1[i];
					bit = getbit(M1, vitri);
				}
				uint b = bit & 0x01;
				ipm1 = (ipm1 << 1) | b;
			}
			return ipm1;
		}

		//show tat ca cac ky tu 0 o dau
		public void ShowByte(StreamWriter sw,uint C)
		{
			for (int i = 1; i <= 8; i++)
			{
				uint b = (C >> (32 - 4 * i)) & 0xF;
				sw.Write("{0:X}", b);
			}
		}

		public void MahoaDES(StreamWriter sw,uint M1, uint M2, uint K1, uint K2, ref uint C1, ref uint C2)
		{
			uint[] key1 = new uint[16];
			uint[] key2 = new uint[16];
			GenKey(K1, K2, key1, key2);
			sw.Write("\n16 keys:");
			for (int i = 0; i < 16; i++)
				sw.Write("\nkey{0}: {1:X} {2:X}", i + 1, key1[i], key2[i]);
			//Buoc 3. Thuc hien hoan vi IP
			uint L0, R0;
			L0 = IPM(M1, M2, 0, 32);
			R0 = IPM(M1, M2, 32, 64);
			uint L1 = 0;
			uint R1 = 0;
			uint FP;
			for (int i = 0; i < 16; i++)
			{
				FP = F(L0, R0, key1[i], key2[i]);
				R1 = FP ^ L0;
				L1 = R0; R0 = R1; L0 = L1;
				sw.Write("\nL[{0}]={1:X}", i + 1, L1);
				sw.Write("\tR[{0}]={1:X}", i + 1, R1);
			}
			C1 = HoanviIP_1(R1, L1, 0, 32);
			C2 = HoanviIP_1(R1, L1, 32, 64);
		}

		public void GiaiMaDES(StreamWriter sw,uint M1, uint M2, uint K1, uint K2, ref uint C1, ref uint C2)
		{
			uint[] key1 = new uint[16];
			uint[] key2 = new uint[16];
			GenKey(K1, K2, key1, key2);
			sw.Write("\n16 keys:");
			for (int i = 15; i >= 0; i--)
				sw.Write("\nkey{0}: {1:X} {2:X}", i + 1, key1[i], key2[i]);
			//Buoc 3. Thuc hien hoan vi IP
			uint L0, R0;
			L0 = IPM(M1, M2, 0, 32);
			R0 = IPM(M1, M2, 32, 64);
			uint L1 = 0;
			uint R1 = 0;
			uint FP;
			for (int i = 0; i < 16; i++)
			{
				FP = F(L0, R0, key1[15 - i], key2[15 - i]);
				R1 = FP ^ L0;
				L1 = R0; R0 = R1; L0 = L1;
				sw.Write("\nL[{0}]={1:X}", i + 1, L1);
				sw.Write("\tR[{0}]={1:X}", i + 1, R1);
			}
			C1 = HoanviIP_1(R1, L1, 0, 32);
			C2 = HoanviIP_1(R1, L1, 32, 64);
		}

		//public void Main(string[] args)
		//{
		//	uint K1 = 0x13345779; //4byte
		//	uint K2 = 0x9BBCDFF1;
		//	Console.Write("\nKey: ");
		//	ShowByte(K1); ShowByte(K2);
		//	uint M1 = 0x01234567;
		//	uint M2 = 0x89ABCDEF;
		//	Console.Write("\nInput String: ");
		//	ShowByte(M1); ShowByte(M2);
		//	//giai ma
		//	uint C1 = 0;
		//	uint C2 = 0;
		//	MahoaDES(M1, M2, K1, K2, ref C1, ref C2);
		//	Console.Write("\nOutput String: ");
		//	ShowByte(C1); ShowByte(C2);
		//	//ma hoa
		//	uint MC1 = 0;
		//	uint MC2 = 0;
		//	GiaiMaDES(C1, C2, K1, K2, ref MC1, ref MC2);
		//	Console.Write("\nOutput String: ");
		//	ShowByte(MC1); ShowByte(MC2);
		//}
	}
}
