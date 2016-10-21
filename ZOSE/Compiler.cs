using System;
using System.Collections.Generic;
using System.Text;

namespace ZOSE
{
    public class GBScript
    {
        public int compileLocation = -1;
        public bool endwith0 = true;
        public GBHL.GBFile output;

        public GBScript(int size)
        {
            output = new GBHL.GBFile(new byte[size]);
        }
    }

    public class Compiler
    {
        GBHL.GBFile gb;
        public GBScript[] scripts;
        private int currentWriteIndex = -1;
        List<int> jumps = new List<int>();
        public List<int> globalJumps;

        public static string[] commands = new string[]
        {
            "writelocation",
            "maketorcheslightable",
            "createpuffnodelay",
            "setinteraction72",
            "jump3byte",
            "jumproomflag",
            "jump2byte",
            "checkitemflag",
            "setvisible",
            "set45",
            "load100",
            "spawncommon",
            "spawnenemy",
            "showpasswordscreen",
            "loadrel",
            "setcoords",
            "set49",
            "load6667",
            "loadsprite",
            "setinteractionfactor",
            "checklinkxtoma",
            "setmemory",
            "ormemory",
            "addsetinteractionfactor",
            "set49extra",
            "settextidjp",
            "showtext",
            "checktext",
            "settextid",
            "showloadedtext",
            "checkabutton",
            "checkroomflag",
            "setroomflag",
            "setglobalflag",
            "disableinput",
            "enableinput",
            "callscript",
            "checkspecialflag",
            "checkenemycount",
            "checkmemorybit",
            "checkmemory",
            "spawnitem",
            "asm15",
            "createpuff",
            "playsound",
            "setmusic",
            "settile",
            "shakescreen",
            "setdelay1",
            "setdelay2",
            "setdelay3",
            "setdelay4",
            "setdelay5",
            "setdelay6",
            "setdelay7",
            "setdelay8",
            "setdelay9",
            "setdelay10",
            "setdelay11",
            "setdelay12",
            "setdelay0",
            "jump3bytemc",
            "jumproomflago",
            "giveitem",
            "jumptilecheck",
            "checktile",
            "forceend",
            "settilepos",
            "setcc8a",
            "spawnenemyhere",
            "unsetroomflag"
        };

        public GBHL.GBFile output
        {
            get { return scripts[currentWriteIndex].output; }
            set { scripts[currentWriteIndex].output = value; }
        }

        public Compiler(GBHL.GBFile g)
        {
            gb = g;
        }

        public bool Compile(string text)
        {
            scripts = new GBScript[1024];
            scripts[0] = new GBScript(1024);
            currentWriteIndex = -1;
            jumps = new List<int>();
            if (text == "")
                return true;
            string[] lines = text.Replace("\r", "").Split('\n');
            foreach (string s in lines)
            {
                if (!CompileLine(s))
                    return false;
            }
            return true;
        }

        public bool CompileLine(string line)
        {
            if (line == "" || line.StartsWith("//"))
                return true;
            int index = line.IndexOf("//");
            if (index > 0)
                line = line.Substring(0, index - 1);
            string[] args = line.Split(' ');
            args[0] = args[0].ToLower();
            if (currentWriteIndex == -1 && args[0] != "writelocation")
                return false;
            int i, j, k, l;
            switch (args[0])
            {
                //Custom
                case "writelocation":
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    currentWriteIndex++;
                    scripts[currentWriteIndex] = new GBScript(1024);
                    scripts[currentWriteIndex].compileLocation = i;
                    break;

                case "forceend":
                    output.WriteByte(0);
                    break;

                case "maketorcheslightable":
                    output.WriteByte(0xE0);
                    output.WriteByte(0x4B);
                    output.WriteByte(0x4F);
                    break;

                case "createpuffnodelay":
                    output.WriteByte(0xE0);
                    output.WriteByte(0xC1);
                    output.WriteByte(0x24);
                    break;

                case "jumptilecheck":
                    if (args.Length < 4)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    i = ParseHex(args[3]);
                    if (j == -1 || i == -1 || k == -1)
                        return false;
                    output.WriteByte(0xFD);
                    output.WriteByte(0x02);
                    output.WriteByte((byte)k);
                    output.WriteByte(0xCF);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)(i / 0x4000));
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    break;

                case "checktile":
                    if (args.Length < 3)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (j == -1 || k == -1)
                        return false;
                    output.WriteByte(0xD5);
                    output.WriteByte((byte)k);
                    output.WriteByte(0xCF);
                    output.WriteByte((byte)j);
                    break;

                case "setinteraction72":
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    this.gb.BufferLocation = 0x3FC000 + (((byte)i) * 3);
                    this.gb.WriteByte((byte)(this.scripts[this.currentWriteIndex].compileLocation / 0x4000));
                    this.gb.WriteByte((byte)this.scripts[this.currentWriteIndex].compileLocation);
                    this.gb.WriteByte((byte)(((this.scripts[this.currentWriteIndex].compileLocation % 0x4000) >> 8) + 0x40));
                    break;

                case "jump3byte": //FD 00
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xFD);
                    output.WriteByte(0);
                    output.WriteByte((byte)(i / 0x4000));
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jumproomflag": //FD 01
                    if (args.Length < 3)
                        return false;
                    j = ParseHex(args[1]);
                    i = ParseHex(args[2]);
                    if (j == -1 || i == -1)
                        return false;
                    output.WriteByte(0xFD);
                    output.WriteByte(1);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)(i / 0x4000));
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    //scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jump3bytemc": //FD 02
                    if (args.Length < 4)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    i = ParseHex(args[3]);
                    if (j == -1 || i == -1 || k == -1)
                        return false;
                    output.WriteByte(0xFD);
                    output.WriteByte(2);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)(k >> 8));
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)(i / 0x4000));
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    //scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jumproomflago": //FD 03
                    if (args.Length < 5)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    l = ParseHex(args[3]);
                    i = ParseHex(args[4]);
                    if (j == -1 || i == -1 || k == -1 || l == -1)
                        return false;
                    output.WriteByte(0xFD);
                    output.WriteByte(3);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)l);
                    output.WriteByte((byte)(i / 0x4000));
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    break;

                case "unsetroomflag": //FD 04
                    if (args.Length < 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    i &= 0xFF;
                    byte b = (byte)(~(1 << i));
                    output.WriteByte(0xFD);
                    output.WriteByte(0x04);
                    output.WriteByte(b);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)k);
                    break;

                //So this wasn't what I wanted. It deals with tile setting in a different way.
                /*case "setinstant":
                    output.WriteByte(0x91);
                    output.WriteByte(0xE0);
                    output.WriteByte(0xCC);
                    output.WriteByte(0x01);
                    break;*/

                //Final

                case "jump2byte": //01-7F
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    output.WriteByte((byte)(i >> 8));
                    output.WriteByte((byte)i);
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "checkitemflag": //CD
                    output.WriteByte(0xCD);
                    break;

                case "setvisible": //80
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x80);
                    output.WriteByte((byte)i);
                    break;

                case "set45": //81
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x81);
                    output.WriteByte((byte)i);
                    break;

                //no 82

                case "load100": //83
                    if (args.Length < 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    output.WriteByte(0x83);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)k);
                    break;

                case "spawncommon": //84
                    if (args.Length < 5)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    l = ParseHex(args[4]);
                    if (i == -1 || j == -1 || k == -1 || l == -1)
                        return false;
                    output.WriteByte(0x84);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)l);
                    break;

                case "spawnenemy": //85
                    if (args.Length < 5)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    l = ParseHex(args[4]);
                    if (i == -1 || j == -1 || k == -1 || l == -1)
                        return false;
                    output.WriteByte(0x85);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)j);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)l);
                    break;

                case "showpasswordscreen": //86
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x86);
                    output.WriteByte((byte)i);
                    break;

                case "loadrel": //87
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x87);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)j);
                    break;

                case "setcoords": //88
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x88);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "set49": //89
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x89);
                    output.WriteByte((byte)i);
                    break;

                case "load6667": //8D
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x8D);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "setinteractionfactor": //8E
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x8E);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "loadsprite": //8F
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x8F);
                    output.WriteByte((byte)i);
                    break;

                case "checklinkxtoma": //90
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x90);
                    output.WriteByte((byte)i);
                    break;

                case "setmemory": //91
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    output.WriteByte(0x91);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)(i >> 8));
                    output.WriteByte((byte)k);
                    break;

                case "ormemory": //92
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    output.WriteByte(0x92);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)(i >> 8));
                    output.WriteByte((byte)k);
                    break;

                case "addsetinteractionfactor": //94
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x94);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "set49extra": //96
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0x96);
                    output.WriteByte((byte)i);
                    break;

                case "settextidjp": //97
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x97);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "showtext": //98
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x98);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "checktext": //99
                    output.WriteByte(0x99);
                    break;

                case "settextid": //9C
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0x9C);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "showloadedtext": //9D
                    output.WriteByte(0x9D);
                    break;

                case "checkabutton": //9E
                    output.WriteByte(0x9E);
                    break;

                case "checkroomflag": //B0
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xB0);
                    output.WriteByte((byte)i);
                    break;

                case "setroomflag": //B1
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xB1);
                    output.WriteByte((byte)i);
                    break;

                case "setglobalflag": //B6
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xB6);
                    output.WriteByte((byte)i);
                    break;

                case "disableinput": //BD
                    output.WriteByte(0xBD);
                    break;

                case "enableinput": //BE
                    output.WriteByte(0xBE);
                    break;

                case "callscript": //C0
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xC0);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)(i >> 8));
                    break;

                case "checkspecialflag": //CF
                    output.WriteByte(0xCF);
                    break;

                case "checkenemycount": //D2
                    output.WriteByte(0xD2);
                    break;

                case "checkmemorybit": //D3
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    output.WriteByte(0xD3);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)(k >> 8));
                    break;

                case "checkmemory": //D5
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[2]);
                    k = ParseHex(args[1]);
                    if (i == -1 || k == -1)
                        return false;
                    output.WriteByte(0xD5);
                    output.WriteByte((byte)k);
                    output.WriteByte((byte)(k >> 8));
                    output.WriteByte((byte)i);
                    break;

                case "spawnitem": //DD
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0xDD);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "giveitem": //DE
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0xDE);
                    output.WriteByte((byte)i); //yy
                    output.WriteByte((byte)j); //xx
                    break;

                case "asm15": //0xE0
                    if (args.Length < 2)
                        break;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xE0);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)(i >> 8));
                    break;

                case "createpuff": //0xE2
                    output.WriteByte(0xE2);
                    break;

                case "playsound": //E3
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xE3);
                    output.WriteByte((byte)i);
                    break;

                case "setmusic": //E4
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xE4);
                    output.WriteByte((byte)i);
                    break;

                case "setcc8a": //E5
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xE5);
                    output.WriteByte((byte)i);
                    break;

                case "spawnenemyhere": //E6
                    if (args.Length < 3)
                        break;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    output.WriteByte(0xE6);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)k);
                    break;

                case "settilepos": //E7
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    output.WriteByte(0xE7);
                    output.WriteByte((byte)i);
                    output.WriteByte((byte)j);
                    break;

                case "settile": //E8
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xE8);
                    output.WriteByte((byte)i);
                    break;

                case "shakescreen": //EA
                    if (args.Length < 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    output.WriteByte(0xEA);
                    output.WriteByte((byte)i);
                    break;

                case "setdelay1": //F1
                    output.WriteByte(0xF1);
                    break;

                case "setdelay2": //F2
                    output.WriteByte(0xF2);
                    break;

                case "setdelay3": //F3
                    output.WriteByte(0xF3);
                    break;

                case "setdelay4": //F4
                    output.WriteByte(0xF4);
                    break;

                case "setdelay5": //F5
                    output.WriteByte(0xF5);
                    break;

                case "setdelay6": //F6
                    output.WriteByte(0xF6);
                    break;

                case "setdelay7": //F7
                    output.WriteByte(0xF7);
                    break;

                case "setdelay8": //F8
                    output.WriteByte(0xF8);
                    break;

                case "setdelay9": //F9
                    output.WriteByte(0xF9);
                    break;

                case "setdelay10": //FA
                    output.WriteByte(0xFA);
                    break;

                case "setdelay11": //FB
                    output.WriteByte(0xFB);
                    break;

                case "setdelay12": //FC
                    output.WriteByte(0xFC);
                    break;

                case "setdelay0": //F0
                    output.WriteByte(0xF0);
                    break;

                default:
                    return false;
            }

            return true;
        }

        public string Decompile(int address)
        {
            string s = "//Decompiled script " + address.ToString("X") + "\r\nwritelocation " + address.ToString("X") + "\r\n";
            gb.BufferLocation = address;
            jumps = new List<int>();
            while (true)
            {
                for (int i = 0; i < jumps.Count; i++)
                {
                    if (gb.BufferLocation % 0x4000 + 0x4000 == jumps[i])
                    {
                        s += "//" + GetJumpCommentary(gb, jumps[i]).ToString("X") + "\r\n";
                        jumps.RemoveAt(i);
                        break;
                    }
                }
                byte opcode = gb.ReadByte();
                switch (opcode)
                {
                    case 0:
                        return s.Substring(0, s.Length - 2);
                    case 0xCD:
                        s += "checkitemflag\r\n";
                        break;
                    case 0x80:
                        s += "setvisible " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x81:
                        s += "set45 " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x83:
                        s += "load100 " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x84:
                        s += "spawncommon " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x85:
                        s += "spawnenemy " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x86:
                        s += "showpasswordscreen " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x87:
                        s += "loadrel " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x88:
                        s += "setcoords " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x89:
                        s += "set49 " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x8D:
                        s += "load6667 " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x8E:
                        s += "setinteractionfactor " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x8F:
                        s += "loadsprite " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x90:
                        s += "checklinkxtoma " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x91:
                        s += "setmemory " + ((gb.ReadByte() + (gb.ReadByte() << 8))).ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x92:
                        s += "ormemory " + ((gb.ReadByte() + (gb.ReadByte() << 8))).ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x94:
                        s += "addsetinteractionfactor " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x96:
                        s += "set49extra " + gb.ReadByte().ToString("X");
                        break;
                    case 0x97:
                        s += "settextidjp " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        s += "//Jump to script address 305F0\r\n";
                        s += "\r\n";
                        return s + Decompile(0x305F0);
                    case 0x98:
                        s += "showtext " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x99:
                        s += "checktext\r\n";
                        break;
                    case 0x9C:
                        s += "settextid " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0x9D:
                        s += "showloadedtext\r\n";
                        break;
                    case 0x9E:
                        s += "checkabutton\r\n";
                        break;
                    case 0xB0:
                        s += "checkroomflag " + gb.ReadByte().ToString("X") + " " + "\r\n";
                        break;
                    case 0xB1:
                        s += "setroomflag " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xB6:
                        s += "setglobalflag " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xBD:
                        s += "disableinput\r\n";
                        break;
                    case 0xBE:
                        s += "enableinput\r\n";
                        break;
                    case 0xC0:
                        s += "callscript " + ((gb.ReadByte() + (gb.ReadByte() << 8))).ToString("X") + "\r\n";
                        break;
                    case 0xCF:
                        s += "checkspecialflag\r\n";
                        break;
                    case 0xD2:
                        s += "checkenemycount\r\n";
                        break;
                    case 0xD3:
                        byte bitind = gb.ReadByte();
                        int bitadd = ((gb.ReadByte() + (gb.ReadByte() << 8)));
                        s += "checkmemorybit " + bitind.ToString("X") + " " + bitadd.ToString("X");
                        if (bitadd == 0xCCA0)
                            s += " //triggers stepped on";
                        s += "\r\n";
                        break;
                    case 0xD5:
                        int d5address = ((gb.ReadByte() + (gb.ReadByte() << 8)));
                        if (d5address >= 0xCF00 && d5address < 0xD000)
                            s += "checktile " + (d5address & 0xFF).ToString("X") + " " + gb.ReadByte().ToString("X") + " //checkmemory " + d5address.ToString("X") + " " + gb.Buffer[gb.BufferLocation - 1].ToString("X") + "\r\n";
                        else
                            s += "checkmemory " + d5address.ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xDD:
                        s += "spawnitem " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xDE:
                        s += "giveitem " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE0:
                        int asmaddr = ((gb.ReadByte() + (gb.ReadByte() << 8)));
                        switch (asmaddr)
                        {
                            case 0x4F4B:
                                s += "maketorcheslightable //asm15 4F4B\r\n";
                                break;
                            case 0x24C1:
                                s += "createpuffnodelay //asm15 24C1\r\n";
                                break;
                            default:
                                s += "asm15 " + asmaddr.ToString("X") + "\r\n";
                                break;
                        }
                        break;
                    case 0xE2:
                        s += "createpuff\r\n";
                        break;
                    case 0xE3:
                        s += "playsound " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE4:
                        s += "setmusic " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE5:
                        s += "setcc8a " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE6:
                        s += "spawnenemyhere " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE7:
                        s += "settilepos " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xE8:
                        s += "settile " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xEA:
                        s += "shakescreen " + gb.ReadByte().ToString("X") + "\r\n";
                        break;
                    case 0xFC:
                        s += "setdelay12\r\n";
                        break;
                    case 0xF0:
                        s += "setdelay0\r\n";
                        break;
                    case 0xF1:
                        s += "setdelay1\r\n";
                        break;
                    case 0xF2:
                        s += "setdelay2\r\n";
                        break;
                    case 0xF3:
                        s += "setdelay3\r\n";
                        break;
                    case 0xF4:
                        s += "setdelay4\r\n";
                        break;
                    case 0xF5:
                        s += "setdelay5\r\n";
                        break;
                    case 0xF6:
                        s += "setdelay6\r\n";
                        break;
                    case 0xF7:
                        s += "setdelay7\r\n";
                        break;
                    case 0xF8:
                        s += "setdelay8\r\n";
                        break;
                    case 0xF9:
                        s += "setdelay9\r\n";
                        break;
                    case 0xFA:
                        s += "setdelay10\r\n";
                        break;
                    case 0xFB:
                        s += "setdelay11\r\n";
                        break;
                    case 0xFF:
                        switch (gb.ReadByte())
                        {
                            case 0:
                                int addr3b = gb.ReadByte() * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                                s += "jump3byte " + addr3b.ToString("X") + "\r\n";
                                if (!globalJumps.Contains(addr3b))
                                    globalJumps.Add(addr3b);
                                break;
                            case 1:
                                byte flag = gb.ReadByte();
                                int addr3b2 = gb.ReadByte() * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                                s += "jumproomflag " + flag.ToString("X") + " " + addr3b2.ToString("X") + "\r\n";
                                if (!globalJumps.Contains(addr3b2))
                                    globalJumps.Add(addr3b2);
                                break;
                            case 2:
                                int addr3b3 = gb.ReadByte() + gb.ReadByte() * 0x100;
                                byte cp = gb.ReadByte();
                                int addr3c3 = gb.ReadByte() * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                                if (addr3b3 >= 0xCF00 && addr3b3 < 0xD000)
                                    s += "jumptilecheck " + (addr3b3 & 0xFF).ToString("X") + " " + cp.ToString("X") + " " + addr3c3.ToString("X") + " //jump3bytemc " + addr3b3.ToString("X") + " " + cp.ToString("X") + " " + addr3c3.ToString("X") + "\r\n";
                                else
                                    s += "jump3bytemc " + addr3b3.ToString("X") + " " + cp.ToString("X") + " " + addr3c3.ToString("X") + "\r\n";
                                if (!globalJumps.Contains(addr3c3))
                                    globalJumps.Add(addr3c3);
                                break;
                            case 3:
                                byte map3 = gb.ReadByte();
                                byte group3 = gb.ReadByte();
                                byte cp3 = gb.ReadByte();
                                int addr4 = gb.ReadByte() * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                                s += "jumproomflago " + map3.ToString("X") + " " + group3.ToString("X") + " " + cp3.ToString("X") + " " + addr4.ToString("X") + "\r\n";
                                if (!globalJumps.Contains(addr4))
                                    globalJumps.Add(addr4);
                                break;
                            case 4:
                                byte flag4 = gb.ReadByte();
                                flag4 = (byte)~flag4;
                                byte index4 = 0;
                                while (flag4 > 1)
                                {
                                    flag4 >>= 1;
                                    index4++;
                                }
                                s += "unsetroomflag " + index4.ToString("X") + " " + gb.ReadByte().ToString("X") + " " + gb.ReadByte().ToString("X") + "\r\n";
                                break;
                        }
                        break;
                    default:
                        if (opcode < 0x80)
                        {
                            int addr = (((opcode << 8) + gb.ReadByte()));
                            jumps.Add(addr);
                            s += "jump2byte " + addr.ToString("X") + " //" + GetJumpCommentary(gb, addr).ToString("X");
                            if (globalJumps.Contains(addr))
                                globalJumps.Add(addr);
                            return s;
                        }
                        s += "unknownopcode" + opcode.ToString("X") + "\r\n";
                        break;
                }
            }
        }

        public int GetJumpCommentary(GBHL.GBFile gb, int jump)
        {
            if (jump < 0x4000 || jump >= 0x8000)
                return jump;
            return (((gb.BufferLocation) / 0x4000) * 0x4000) + (jump - 0x4000);
        }

        /*public bool CompileLine(string line)
        {
            if (line == "")
                return true;
            string[] args = line.Split(' ');
            args[0] = args[0].ToLower();
            int i, j, k, l;
            switch (args[0])
            {
                case "start":
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    compileAddress = CalculateStartAddress(i, k);
                    break;

                case "spaceoff":
                    findFreeSpace = false;
                    break;

                case "setwrite":
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    writeAddress = i;
                    output.WriteByte(0x3E);
                    output.WriteByte((byte)(writeAddress / 0x4000));
                    output.WriteByte(0x21);
                    output.WriteByte((byte)(writeAddress));
                    output.WriteByte((byte)(((writeAddress - (writeAddress / 0x4000 * 0x4000)) >> 8) + 0x40));
                    output.WriteByte(0xC3);
                    output.WriteByte((byte)callLocation);
                    output.WriteByte((byte)(callLocation >> 8));
                    switchIndex = output.BufferLocation;
                    break;

                case "checktile":
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    //output.WriteByte(0xE5);
                    output.WriteBytes(new byte[] { 0x21, 0, 0xCF });
                    output.WriteByte(0x3E);
                    output.WriteByte((byte)i);
                    output.WriteByte(0x85);
                    output.WriteByte(0x6F);
                    output.WriteByte(0x3E);
                    output.WriteByte((byte)k);
                    output.WriteByte(0xBE);
                    break;

                case "settile":
                    if (args.Length < 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    //output.WriteByte(0xE5);
                    output.WriteBytes(new byte[] { 0x21, 0, 0xCF });
                    output.WriteByte(0x3E);
                    output.WriteByte((byte)i);
                    output.WriteByte(0x85);
                    output.WriteByte(0x6F);
                    output.WriteByte(0x3E);
                    output.WriteByte((byte)k);
                    output.WriteByte(0x77);
                    break;

                case "retn":
                    output.WriteByte(0xC0);
                    break;

                case "retz":
                    output.WriteByte(0xC8);
                    break;

                case "ret":
                    output.WriteByte(0xC9);
                    break;
            }

            return true;
        }*/

        public int ParseHex(string text)
        {
            try
            {
                int i = int.Parse(text, System.Globalization.NumberStyles.HexNumber);
                return i;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int CalculateStartAddress(int first, int second)
        {
            byte a = (byte)first;
            byte bank = 0;
            if (a < 0x3E) bank = 08;
            else if (a < 0x67) bank = 09;
            else if (a < 0x98) bank = 0x0A;
            else if (a < 0xDC) bank = 0x0B;
            else bank = 0x0C;
            int pointer = 0x3B8B + (a * 2);
            gb.BufferLocation = pointer;
            gb.BufferLocation = bank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
            if (gb.ReadByte() == 0x1E && gb.ReadByte() == 0x42)
            {
                gb.BufferLocation += 2;
                byte b = (byte)(second * 2);
                gb.BufferLocation += b;
                return bank * 0x4000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
            }
            return gb.BufferLocation - 2;
        }
    }
}
