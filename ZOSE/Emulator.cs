using System;
using System.Collections.Generic;
using System.Text;

namespace ZOSE
{
	public class Emulator
	{
		GBHL.GBFile gb;
		Stack<int> stack = new Stack<int>(); //An int because we push the PC onto here which is GBFile.BufferLocation (integer)
		byte a, b, c, d, e, f, h, l;
		byte bank;
		bool zero, carry;
		int final = -1;
		byte[] ram; //Note: No bank support!

		private ushort hl
		{
			get { return (ushort)(h * 0x100 + l); }
			set { h = (byte)(value >> 8); l = (byte)(value & 0xFF); }
		}

		private ushort af
		{
			get { return (ushort)(a * 0x100 + f); }
			set { a = (byte)(value >> 8); f = (byte)(value & 0xFF); }
		}

		private ushort bc
		{
			get { return (ushort)(b * 0x100 + c); }
			set { b = (byte)(value >> 8); c = (byte)(value & 0xFF); }
		}

		private ushort de
		{
			get { return (ushort)(d * 0x100 + e); }
			set { d = (byte)(value >> 8); e = (byte)(value & 0xFF); }
		}

		public Emulator(GBHL.GBFile g)
		{
			gb = g;
		}

		public int EmulateUntilScript(int id, int x, int y)
		{
			//Some initialization
			this.a = b = c = d = e = f = h = l = 0;
			//Because this isn't a REAL emulator, we'll set D to... D0
			d = 0xD0;
			zero = carry = false;
			gb.BufferLocation = 0;
			ram = new byte[0x10000];
			stack = new Stack<int>();
			for (int i = 0; i < 0x4000; i++)
				ram[i] = gb.ReadByte();
			for (int i = 0; i < 0x10; i++)
			{
				ram[0xD040 + i * 0x100] = 1; //We'll say we have all 16 interactions active
				ram[0xD041 + i * 0x100] = (byte)(id >> 8);
				ram[0xD042 + i * 0x100] = (byte)id;
				ram[0xD04B + i * 0x100] = (byte)y;
				ram[0xD04D + i * 0x100] = (byte)x;
				//ram[0xD044 + i * 0x100] = 1; //We'll say we have all 16 interactions active
			}
			for (int i = 0; i < 0x20; i++)
				stack.Push(0); //Emulate things for a bit
			ram[0xCD00] = 1;
			ram[0xC6D2] = 0x2F;
			ram[0xCC08] = 0x4F;
			final = -1;
			//Calculate the interaction's assembly address
			byte first = (byte)(id >> 8);
			byte second = (byte)(id);
			byte a = first;
			byte bank = 0;
			if (a < 0x3E) bank = 08;
			else if (a < 0x67) bank = 09;
			else if (a < 0x98) bank = 0x0A;
			else if (a < 0xDC) bank = 0x0B;
			else bank = 0x0C;
			int pointer = 0x3B8B + (a * 2);
			gb.BufferLocation = pointer;
			gb.BufferLocation = bank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);

			this.bank = bank;
			Emulate(gb.BufferLocation);

			if (final == -1)
				return -1;
			if (final < 0x100)
				return final;

			return 0x30000 + (final - 0x4000);
		}

		public int GetAddress()
		{
			int pointer = gb.ReadByte() + gb.ReadByte() * 0x100;
			if (pointer < 0x4000 || pointer > 0x7FFF)
				return pointer;
			return bank * 0x4000 + pointer - 0x4000;
		}

		public int Emulate(int address)
		{
			int retCount = 0;

			gb.BufferLocation = address;
			byte before;

			while (gb.BufferLocation != 0x2518 && retCount != -1)
			{
				if (gb.BufferLocation == 0xFC2D8)
				{
				}
				if (final != -1)
					return final;
				byte opcode = gb.ReadByte();
				switch (opcode)
				{

					case 00: //NOP
						break;

					case 01: //LD BC,nn
						c = gb.ReadByte();
						b = gb.ReadByte();
						break;

					case 05: //DEC B
						b = (byte)(b - 1);
						if (b == 0)
							zero = true;
						else
							zero = false;
						break;

					case 06: //LD B,n
						b = gb.ReadByte();
						break;

					case 07: //RLCA
						a = RotateLeft(a);
						if ((a & 1) != 0)
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 09: //ADD HL,BC
						hl = (ushort)(hl + bc);
						break;

					case 0xA: //LD A,(BC)
						a = ReadByte(bc);
						break;

					case 0x0E: //LD C,n
						c = gb.ReadByte();
						break;

					case 0x12: //LD (DE),A
						ram[de] = a;
						break;

					case 0x16: //LD D,n
						d = gb.ReadByte();
						break;

					case 0x18: //JR n
						sbyte s = (sbyte)gb.ReadByte();
						gb.BufferLocation += s;
						break;

					case 0x1A: //LD A,(DE)
						a = ReadByte(de);
						break;

					case 0x1C: //INC E
						e = (byte)(e + 1);
						if (e == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x1D: //DEC E
						e = (byte)(e - 1);
						if (e == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x1E: //LD E,n
						e = gb.ReadByte();
						break;

					case 0x20: //JR NZ,n
						sbyte blank2 = (sbyte)gb.ReadByte();
						if (!zero)
						{
							gb.BufferLocation += blank2;
						}
						break;

					case 0x21: //LD HL,nn
						l = gb.ReadByte();
						h = gb.ReadByte();
						break;

					case 0x22: //LDI (HL),A
						ram[hl++] = a;
						break;

					case 0x23: //INC HL
						hl = (ushort)(hl + 1);
						break;

					case 0x24: //INC H
						h = (byte)(h + 1);
						if (h == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x28: //JR Z,n
						sbyte blank3 = (sbyte)gb.ReadByte();
						if (zero)
						{
							gb.BufferLocation += blank3;
						}
						break;

					case 0x2A: //LDI A,(HL)
						a = ReadByte(hl++);
						break;

					case 0x2B: //DEC HL
						hl = (ushort)(hl - 1);
						break;

					case 0x2C: //INC L
						l = (byte)(l + 1);
						if (l == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x2D: //DEC L
						l = (byte)(l - 1);
						if (l == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x2E: //LD L,n
						l = gb.ReadByte();
						break;

					case 0x30: //JR NC,n
						sbyte blank4 = (sbyte)gb.ReadByte();
						if (!carry)
						{
							gb.BufferLocation += blank4;
						}
						break;

					case 0x35: //DEC (HL)
						ram[hl] = (byte)(ReadByte(hl) - 1);
						if (ReadByte(hl) == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x36: //LD (HL),n
						ram[hl] = gb.ReadByte();
						break;

					case 0x37: //SCF
						carry = true;
						break;

					case 0x38: //JR C,n
						sbyte blank5 = (sbyte)gb.ReadByte();
						if (carry)
						{
							gb.BufferLocation += blank5;
						}
						break;

					case 0x3A: //LDD A,(HL)
						a = ReadByte(hl--);
						break;

					case 0x3C: //INC A
						a = (byte)(a + 1);
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x3D: //DEC A
						a = (byte)(a - 1);
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x3E: //LD A,n
						a = gb.ReadByte();
						break;

					case 0x44: //LD B,H
						b = h;
						break;

					case 0x46: //LD B,(HL)
						h = ReadByte(hl);
						break;

					case 0x47: //LD B,A
						b = a;
						break;

					case 0x4F: //LD C,A
						c = a;
						break;

					case 0x5E: //LD E,(HL)
						e = ReadByte(hl);
						break;

					case 0x5F: //LD E,A
						e = a;
						break;

					case 0x62: //LD H,D
						h = d;
						break;

					case 0x66: //LD H,(HL)
						h = ReadByte(hl);
						break;

					case 0x6F: //LD L,A
						l = a;
						break;

					case 0x70: //LD (HL),B
						ram[hl] = b;
						break;

					case 0x71: //LD (HL),C
						ram[hl] = c;
						break;

					case 0x73: //LD (HL),E
						ram[hl] = e;
						break;

					case 0x77: //LD (HL),A
						ram[hl] = a;
						break;

					case 0x78: //LD A,B
						a = b;
						break;

					case 0x79: //LD A,C
						a = c;
						break;

					case 0x7B: //LD A,E
						a = e;
						break;

					case 0x7C: //LD A,H
						a = h;
						break;

					case 0x7D: //LD A,L
						a = l;
						break;

					case 0x7E: //LD A,(HL)
						a = ReadByte(hl);
						break;

					case 0x81: //ADD C
						before = a;
						a = (byte)(a + c);
						if (a < before || (a == before && c != 0))
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x85: //ADD L
						before = a;
						a = (byte)(a + l);
						if (a < before || (a == before && l != 0))
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x87: //ADD A
						before = a;
						a = (byte)(a + a);
						if (a < before || (a == before && before != 0))
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0x91: //SUB C
						before = a;
						a = (byte)(a - c);
						if (a > before || (a == before && c != 0))
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xA6: //AND (HL)
						a = (byte)(a & ReadByte(hl));
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;
						
					case 0xA8: //XOR B
						a = (byte)(a ^ b);
						break;

					case 0xAF: //XOR A
						a = (byte)(a ^ a);
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xB7: //OR A
						a = (byte)(a | a);
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xB8: //CP B
						if (a == b)
							zero = true;
						else
							zero = false;
						if (a < b)
							carry = true;
						else
							carry = false;
						break;

					case 0xB9: //CP C
						if (a == c)
							zero = true;
						else
							zero = false;
						if (a < c)
							carry = true;
						else
							carry = false;
						break;

					case 0xBB: //CP E
						if (a == e)
							zero = true;
						else
							zero = false;
						if (a < e)
							carry = true;
						else
							carry = false;
						break;

					case 0xC0: //RET NZ
						if (!zero)
						{
							retCount--;
							gb.BufferLocation = PopPC();
							break;
						}
						break;

					case 0xC1: //POP BC
						bc = (ushort)stack.Pop();
						break;

					case 0xC2: //JP NZ,nn
						int addr = gb.ReadByte() + gb.ReadByte() * 0x100;
						if (!zero)
						{
							gb.BufferLocation = CalculatePC(addr);
						}
						break;

					case 0xC3: //JP nn
						gb.BufferLocation = CalculatePC(gb.ReadByte() + gb.ReadByte() * 0x100);
						break;

					case 0xC5: //PUSH BC
						stack.Push(bc);
						break;

					case 0xC7: //RST 00
						PushPC();
						Emulate(0);
						break;

					case 0xC8: //RET Z
						if (zero)
						{
							retCount--;
							gb.BufferLocation = PopPC();
							break;
						}
						break;

					case 0xC9: //RET
						retCount--;
						gb.BufferLocation = PopPC();
						break;

					case 0xCB: //VARYING
						switch (gb.ReadByte())
						{
							case 0x37: //SWAP A
								a = (byte)(((a & 0xF) << 4) + (a >> 4));
								break;
							case 0x7E: //BIT 7,(HL)
								byte bit7 = (byte)(1 << 7);
								bit7 = (byte)(ReadByte(hl) & bit7);
								if (bit7 == 0)
									zero = true;
								else
									zero = false;
								break;
							default:

								break;
						}
						break;

					case 0xCD: //Call
						address = GetAddress();
						PushPC();
						if (address == 0x2518)
						{
							final = hl;
							return final;
						}
						if (address == 0x2544)
						{
							final = hl;
							return final;
						}
						int i = Emulate(address);
						/*if (address == 0x2518)
						{
							final = hl;
							return final;
						}
						gb.BufferLocation = PopPC();*/
						break;

					case 0xCF: //RST 08
						PushPC();
						Emulate(0x08);
						break;

					case 0xD0: //RET NC
						if (!carry)
						{
							retCount--;
							gb.BufferLocation = PopPC();
						}
						break;

					case 0xD1: //POP DE
						de = (ushort)stack.Pop();
						break;
					
					case 0xD4: //CALL NC,nn
						address = GetAddress();
						if (!carry)
						{
							PushPC();
							Emulate(address);
						}
						break;
						
					case 0xD5: //PUSH DE
						stack.Push(de);
						break;

					case 0xD6: //SUB A,n
						before = a;
						a = (byte)(a - gb.ReadByte());
						if (a > before)
							carry = true;
						else
							carry = false;
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xD9: //RETI
						retCount--;
						gb.BufferLocation = PopPC();
						break;

					case 0xDC: //CALL C,nn
						address = GetAddress();
						if (carry)
						{
							PushPC();
							Emulate(address);
						}
						break;

					case 0xDF: //RST 18
						PushPC();
						Emulate(0x18);
						break;

					case 0xE0: //LD (FF00+n),a
						ram[0xFF00 + gb.ReadByte()] = a;
						break;

					case 0xE1: //POP HL
						hl = (ushort)stack.Pop();
						break;

					case 0xE5: //PUSH HL
						stack.Push(hl);
						break;

					case 0xE6: //AND A,n
						a &= gb.ReadByte();
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xE9: //JP HL
						gb.BufferLocation = CalculatePC(hl);
						break;

					case 0xEA: //LD (nn),a
						int add = gb.ReadByte() + gb.ReadByte() * 0x100;
						if (add < 0x4000)
						{
							bank = a;
							RecalculatePC();
						}
						else
							ram[add] = a;
						break;

					case 0xEF: //RST 28
						PushPC();
						Emulate(0x28);
						break;

					case 0xF0: //LD A,(FF00+n)
						a = ReadByte(0xFF00 + gb.ReadByte());
						break;

					case 0xF1: //POP AF
						af = (ushort)stack.Pop();
						break;

					case 0xF5: //PUSH AF
						stack.Push(af);
						break;

					case 0xF6: //OR A,n
						a = (byte)(a | gb.ReadByte());
						if (a == 0)
							zero = true;
						else
							zero = false;
						break;

					case 0xFA: //LD A,(nn)
						a = ReadByte(GetAddress());
						break;

					case 0xFE: //CP A,n
						byte bb = gb.ReadByte();
						if (a == bb)
							zero = true;
						else
							zero = false;
						if (a < bb)
							carry = true;
						else
							carry = false;
						break;

					case 0xFF: //RST 38
						PushPC();
						Emulate(0x38);
						break;

					default:
						opcode = (byte)(opcode - 0);
						final = opcode;
						break;
				}
			}

			if (retCount == -1)
				return -1;
			return gb.BufferLocation;
		}

		public byte RotateLeft(byte b)
		{
			byte carry = (byte)(b >> 7);
			return (byte)((b << 1) | carry);
		}

		public byte ReadByte(int address)
		{
			if (address < 0x4000 || address > 0x7FFF)
				return ram[address];
			return gb.Buffer[bank * 0x4000 + address - 0x4000];
		}

		public void RecalculatePC()
		{
			if (gb.BufferLocation < 0x4000)
				return;
			if (gb.BufferLocation > bank * 0x4000 + 0x3FFF)
			{
				gb.BufferLocation %= 0x4000 + bank * 0x4000;
			}
		}

		public int CalculatePC(int address)
		{
			if (address < 0x4000)
				return address;
			return bank * 0x4000 + address - 0x4000;
		}

		public void PushPC()
		{
			int address = gb.BufferLocation;
			if (address < 0x4000)
			{
				stack.Push(address);
				return;
			}
			address = address % 0x4000;
			address += 0x4000;
			stack.Push(address);
		}

		public int PopPC()
		{
			int address = stack.Pop();
			if (address < 0x4000)
				return address;
			address += bank * 0x4000 - 0x4000;
			return address;
		}
	}
}
