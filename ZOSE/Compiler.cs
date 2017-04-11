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
            "forceend",
            "maketorcheslightable",
            "createpuffnodelay",
            "jumptilecheck",
            "checktile",
            "setinteraction72",
            "jump3byte",
            "jumproomflag",
            "jump3bytemc",
            "jumproomflago",
            "unsetroomflag",
            "jump2byte",
            "setvisible",
            "setstate",
            "set45",
            "setstate2",
            "loadscript",
            "spawncommon",
            "spawninteraction",
            "spawnenemy",
            "showpasswordscreen",
            "loadrel",
            "jumptable_memoryaddress",
            "setcoords",
            "set49",
            "setangle",
            "turntofacelink",
            "setspeed",
            "checkcounter2iszero",
            "load6667",
            "setcollisionradii",
            "setinteractionfactor",
            "writeinteractionbyte",
            "loadsprite",
            "setanimation",
            "checklinkxtoma",
            "cplinkx",
            "setmemory",
            "writememory",
            "ormemory",
            "getrandombits",
            "addsetinteractionfactor",
            "addinteractionbyte",
            "setzspeed",
            "set49extra",
            "setangleandanimation",
            "settextidjp",
            "rungenericnpc",
            "rungenericnpclowindex",
            "showtext",
            "showtextlowindex",
            "checktext",
            "showtextnonexitable",
            "showtextnonexitablelowindex",
            "makeabuttonsensitive",
            "settextid",
            "showloadedtext",
            "checkabutton",
            "showtextdifferentforlinked",
            "checkcfc0bit",
            "xorcfc0bit",
            "checkroomflag",
            "jumpifroomflagset",
            "setroomflag",
            "orroomflag",
            "jumpifc6xxset",
            "writec6xx",
            "jumpifglobalflagset",
            "setglobalflag",
            "unsetglobalflag",
            "setdisabledobjectsto91",
            "setdisabledobjectsto00",
            "setdisabledobjectsto11",
            "disablemenu",
            "enablemenu",
            "disableinput",
            "enableinput",
            "callscript",
            "retscript",
            "jumpiftextoptioneq",
            "jumpalways",
            "jumptable_interactionbyte",
            "jumpifmemoryset",
            "jumpiftradeitemeq",
            "jumpifnoenemies",
            "jumpiflinkvariableneq",
            "jumpifmemoryeq",
            "jumpifinteractionbyteeq",
            "checkitemflag",
            "checkroomflag40",
            "checkspecialflag",
            "checkroomflag80",
            "checkcollidedwithlink_onground",
            "checkpalettefadedone",
            "checkenemycount",
            "checknoenemies",
            "checkmemorybit",
            "checkflagset",
            "checkinteractionbyteeq",
            "checkmemory",
            "checkmemoryeq",
            "checknotcollidedwithlink_ignorez",
            "setcounter1",
            "checkheartdisplayupdated",
            "checkrupeedisplayupdated",
            "checkcollidedwithlink_ignorez",
            "spawnitem",
            "giveitem",
            "jumpifitemobtained",
            "asm15",
            "createpuff",
            "playsound",
            "setmusic",
            "setcc8a",
            "setdisabledobjects",
            "spawnenemyhere",
            "settilepos",
            "settileat",
            "settile",
            "settilehere",
            "updatelinkrespawnposition",
            "shakescreen",
            "initcollisions",
            "movenpcup",
            "movenpcright",
            "movenpcdown",
            "movenpcleft",
            "setdelay0",
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

        public Tuple<int,int> Compile(string text)
        {
            scripts = new GBScript[1024];
            scripts[0] = new GBScript(1024);
            currentWriteIndex = -1;
            jumps = new List<int>();
            if (text == "")
                return null;
            string[] linesOrig = text.Split('\n');
            string[] lines = text.Replace("\r", "").Split('\n');
            int charIndex=0;
            int i=0;
            foreach (string s in lines)
            {
                if (!CompileLine(s.Trim()))
                    return new Tuple<int,int>(charIndex, s.Length);
                charIndex += linesOrig[i++].Length + 1;
            }
            return null;
        }

        void WriteWord(ushort word)
        {
            output.WriteByte((byte)word);
            output.WriteByte((byte)(word >> 8));
        }

        void WriteWordBE(ushort word)
        {
            output.WriteByte((byte)(word >> 8));
            output.WriteByte((byte)word);
        }

        ushort GetPointer(int address) {
            if (address < 0x4000)
                return (ushort)address;
            else
                return (ushort)((address&0x3fff) + 0x4000);
        }
        void WritePointer(int ptr)
        {
            WriteWord(GetPointer(ptr));
        }

        void WriteByte(byte b)
        {
            output.WriteByte(b);
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
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    currentWriteIndex++;
                    scripts[currentWriteIndex] = new GBScript(1024);
                    scripts[currentWriteIndex].compileLocation = i;
                    break;

                case "forceend":
                    if (args.Length != 1)
                        return false;
                    WriteByte(0);
                    break;

                case "maketorcheslightable":
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xE0);
                    WriteByte(0x4B);
                    WriteByte(0x4F);
                    break;

                case "createpuffnodelay":
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xE0);
                    WriteByte(0xC1);
                    WriteByte(0x24);
                    break;

                case "jumptilecheck":
                    if (args.Length != 4)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    i = ParseHex(args[3]);
                    if (j == -1 || i == -1 || k == -1)
                        return false;
                    WriteByte(0xFD);
                    WriteByte(0x02);
                    WriteByte((byte)k);
                    WriteByte(0xCF);
                    WriteByte((byte)j);
                    WriteByte((byte)(i / 0x4000));
                    WriteByte((byte)i);
                    WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    break;

                case "checktile":
                    if (args.Length != 3)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (j == -1 || k == -1)
                        return false;
                    WriteByte(0xD5);
                    WriteByte((byte)k);
                    WriteByte(0xCF);
                    WriteByte((byte)j);
                    break;

                case "setinteraction72":
                    if (args.Length != 2)
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
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xFD);
                    WriteByte(0);
                    WriteByte((byte)(i / 0x4000));
                    WriteByte((byte)i);
                    WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jumproomflag": //FD 01
                    if (args.Length != 3)
                        return false;
                    j = ParseHex(args[1]);
                    i = ParseHex(args[2]);
                    if (j == -1 || i == -1)
                        return false;
                    WriteByte(0xFD);
                    WriteByte(1);
                    WriteByte((byte)j);
                    WriteByte((byte)(i / 0x4000));
                    WriteByte((byte)i);
                    WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    //scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jump3bytemc": //FD 02
                    if (args.Length != 4)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    i = ParseHex(args[3]);
                    if (j == -1 || i == -1 || k == -1)
                        return false;
                    WriteByte(0xFD);
                    WriteByte(2);
                    WriteByte((byte)k);
                    WriteByte((byte)(k >> 8));
                    WriteByte((byte)j);
                    WriteByte((byte)(i / 0x4000));
                    WriteByte((byte)i);
                    WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    //scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "jumproomflago": //FD 03
                    if (args.Length != 5)
                        return false;
                    k = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    l = ParseHex(args[3]);
                    i = ParseHex(args[4]);
                    if (j == -1 || i == -1 || k == -1 || l == -1)
                        return false;
                    WriteByte(0xFD);
                    WriteByte(3);
                    WriteByte((byte)k);
                    WriteByte((byte)j);
                    WriteByte((byte)l);
                    WriteByte((byte)(i / 0x4000));
                    WriteByte((byte)i);
                    WriteByte((byte)((i % 0x4000 >> 8) + 0x40));
                    break;

                case "unsetroomflag": //FD 04
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    i &= 0xFF;
                    byte b = (byte)(~(1 << i));
                    WriteByte(0xFD);
                    WriteByte(0x04);
                    WriteByte(b);
                    WriteByte((byte)j);
                    WriteByte((byte)k);
                    break;

                case ".db":
                case "db":
                    if (args.Length < 2)
                        return false;
                    i = 1;
                    while (i < args.Length) {
                        j = ParseHex(args[i]);
                        if (j == -1)
                            return false;
                        WriteByte((byte)j);
                        i++;
                    }
                    break;

                case ".dw":
                case "dw":
                    if (args.Length < 2)
                        return false;
                    i = 1;
                    while (i < args.Length) {
                        j = ParseHex(args[i]);
                        if (j == -1)
                            return false;
                        WriteWord((ushort)j);
                        i++;
                    }
                    break;

                //Final

                case "jump2byte": //01-7F
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    i = GetPointer(i);
                    WriteByte((byte)(i>>8));
                    WriteByte((byte)(i&0xff));
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "setvisible": //80
                case "setstate":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x80);
                    WriteByte((byte)i);
                    break;

                case "set45": // 81
                case "setstate2":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x81);
                    WriteByte((byte)i);
                    break;

                //no 82

                case "loadscript": //83
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x83);
                    WriteByte((byte)(i/0x4000));
                    WritePointer(i);
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "spawncommon": //84
                case "spawninteraction":
                    if (args.Length != 5)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    l = ParseHex(args[4]);
                    if (i == -1 || j == -1 || k == -1 || l == -1)
                        return false;
                    WriteByte(0x84);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WriteByte((byte)k);
                    WriteByte((byte)l);
                    break;

                case "spawnenemy": //85
                    if (args.Length != 5)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    l = ParseHex(args[4]);
                    if (i == -1 || j == -1 || k == -1 || l == -1)
                        return false;
                    WriteByte(0x85);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WriteByte((byte)k);
                    WriteByte((byte)l);
                    break;

                case "showpasswordscreen": //86
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x86);
                    WriteByte((byte)i);
                    break;

                case "loadrel": //87
                case "jumptable_memoryaddress":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x87);
                    WriteWord((ushort)i);
                    break;

                case "setcoords": //88
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0x88);
                    WriteByte((byte)i); //yy
                    WriteByte((byte)j); //xx
                    break;

                case "set49": //89
                case "setangle":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x89);
                    WriteByte((byte)i);
                    break;

                case "turntofacelink": // 8a
                    if (args.Length != 1)
                        return false;
                    WriteByte(0x8a);
                    break;

                case "setspeed": // 8b
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x8b);
                    WriteByte((byte)i);
                    break;

                case "checkcounter2iszero": // 8c, d8
                    if (args.Length == 1) {
                        WriteByte(0x8c);
                    }
                    else if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0xd8);
                        WriteByte((byte)i);
                    }
                    else
                        return false;
                    break;

                case "load6667": //8D
                case "setcollisionradii":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0x8D);
                    WriteByte((byte)i); //yy
                    WriteByte((byte)j); //xx
                    break;

                case "setinteractionfactor": //8E
                case "writeinteractionbyte":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0x8E);
                    WriteByte((byte)i); //yy
                    WriteByte((byte)j); //xx
                    break;

                case "loadsprite": //8F
                case "setanimation":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x8F);
                    WriteByte((byte)i);
                    break;

                case "checklinkxtoma": //90
                case "cplinkx":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x90);
                    WriteByte((byte)i);
                    break;

                case "setmemory": //91
                case "writememory":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    WriteByte(0x91);
                    WriteByte((byte)i);
                    WriteByte((byte)(i >> 8));
                    WriteByte((byte)k);
                    break;

                case "ormemory": //92
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    WriteByte(0x92);
                    WriteByte((byte)i);
                    WriteByte((byte)(i >> 8));
                    WriteByte((byte)k);
                    break;

                case "getrandombits": // 93
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0x93);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    break;

                case "addsetinteractionfactor": //94
                case "addinteractionbyte":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0x94);
                    WriteByte((byte)i); //yy
                    WriteByte((byte)j); //xx
                    break;

                case "setzspeed": // 95
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x95);
                    WriteWord((ushort)i); //yy
                    break;

                case "set49extra": //96
                case "setangleandanimation":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x96);
                    WriteByte((byte)i);
                    break;

                case "settextidjp": //97
                case "rungenericnpc":
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0x97);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0x97);
                        WriteByte((byte)i); //yy
                        WriteByte((byte)j); //xx
                    }
                    else
                        return false;
                    scripts[currentWriteIndex].endwith0 = false;
                    break;
                    
                case "rungenericnpclowindex": // 97
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x97);
                    WriteByte((byte)i);
                    scripts[currentWriteIndex].endwith0 = false;
                    break;

                case "showtext": //98
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0x98);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0x98);
                        WriteByte((byte)i); //yy
                        WriteByte((byte)j); //xx
                    }
                    else
                        return false;
                    break;

                case "showtextlowindex": // 98
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x98);
                    WriteByte((byte)i);
                    break;

                case "checktext": //99
                    if (args.Length != 1)
                        return false;
                    WriteByte(0x99);
                    break;

                case "showtextnonexitable": //9a
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0x9a);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0x9a);
                        WriteByte((byte)i); //yy
                        WriteByte((byte)j); //xx
                    }
                    else
                        return false;
                    break;

                case "showtextnonexitablelowindex": // 9a
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0x9a);
                    WriteByte((byte)i);
                    break;

                case "makeabuttonsensitive": // 9b
                    if (args.Length != 1)
                        return false;
                    WriteByte(0x9b);
                    break;

                case "settextid": //9C
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0x9C);
                        WriteWord((ushort)i); //yy
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0x9C);
                        WriteByte((byte)j); //xx
                        WriteByte((byte)i); //yy
                    }
                    else
                        return false;
                    break;

                case "showloadedtext": //9D
                    if (args.Length != 1)
                        return false;
                    WriteByte(0x9D);
                    break;

                case "checkabutton": //9E
                    if (args.Length != 1)
                        return false;
                    WriteByte(0x9E);
                    break;

                case "showtextdifferentforlinked": // 9F
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0x9f);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WriteByte((byte)k);
                    break;

                case "checkcfc0bit": // a0-a7
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1 || i >= 8)
                        return false;
                    WriteByte((byte)(0xa0 | i));
                    break;

                case "xorcfc0bit": // a8-af
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1 || i >= 8)
                        return false;
                    WriteByte((byte)(0xa8 | i));
                    break;

                case "checkroomflag": //B0
                case "jumpifroomflagset":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xB0);
                    WriteByte((byte)i);
                    WritePointer(j);
                    break;

                case "setroomflag": //B1
                case "orroomflag":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xB1);
                    WriteByte((byte)i);
                    break;

                // no b2

                case "jumpifc6xxset": // b3
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0xB3);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WritePointer(k);
                    break;

                case "writec6xx": // b4
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xB4);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    break;

                case "jumpifglobalflagset": // b5
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xB5);
                    WriteByte((byte)i);
                    WritePointer(j);
                    break;

                case "setglobalflag": //B6
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1 || i >= 0x80)
                        return false;
                    WriteByte(0xB6);
                    WriteByte((byte)i);
                    break;

                case "unsetglobalflag": //B6
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1 || i >= 0x80)
                        return false;
                    WriteByte(0xB6);
                    WriteByte((byte)(i | 0x80));
                    break;

                // no b7

                case "setdisabledobjectsto91": //B8
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xB8);
                    break;

                case "setdisabledobjectsto00": //B9
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xB9);
                    break;

                case "setdisabledobjectsto11": //BA
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xBA);
                    break;

                case "disablemenu": //BB
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xBB);
                    break;

                case "enablemenu": //BC
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xBC);
                    break;

                case "disableinput": //BD
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xBD);
                    break;

                case "enableinput": //BE
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xBE);
                    break;

                // no bf

                case "callscript": //C0
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xC0);
                    WriteByte((byte)i);
                    WriteByte((byte)(i >> 8));
                    break;

                case "retscript": //C1
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xC1);
                    break;

                // no c2

                case "jumpiftextoptioneq": //C3
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xC3);
                    WriteByte((byte)i);
                    WritePointer(j);
                    break;

                case "jumpalways": // C4
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xC4);
                    WritePointer(i);
                    break;

                // no c5

                case "jumptable_interactionbyte": // C6
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xC6);
                    WriteByte((byte)i);
                    break;

                case "jumpifmemoryset": // C7
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0xC7);
                    WriteWord((ushort)i);
                    WriteByte((byte)j);
                    WritePointer(k);
                    break;

                case "jumpiftradeitemeq": // C8
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xC8);
                    WriteByte((byte)i);
                    WritePointer(j);
                    break;

                case "jumpifnoenemies": //C9
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xC9);
                    WritePointer(i);
                    break;

                case "jumpiflinkvariableneq": //CA
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0xCA);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WritePointer(k);
                    break;

                case "jumpifmemoryeq": //CB
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0xCB);
                    WriteWord((ushort)i);
                    WriteByte((byte)j);
                    WritePointer(k);
                    break;

                case "jumpifinteractionbyteeq": //CC
                    if (args.Length != 4)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    k = ParseHex(args[3]);
                    if (i == -1 || j == -1 || k == -1)
                        return false;
                    WriteByte(0xCC);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    WritePointer(k);
                    break;

                case "checkitemflag": //CD
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xCD);
                    break;

                case "checkroomflag40": //CE
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xCE);
                    break;

                case "checkspecialflag": //CF
                case "checkroomflag80":
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xCF);
                    break;

                case "checkcollidedwithlink_onground": //D0
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xD0);
                    break;

                case "checkpalettefadedone": //D1
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xD1);
                    break;

                case "checkenemycount": //D2
                case "checknoenemies":
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xD2);
                    break;

                case "checkmemorybit": //D3
                case "checkflagset":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    k = ParseHex(args[2]);
                    if (i == -1 || k == -1)
                        return false;
                    WriteByte(0xD3);
                    WriteByte((byte)i);
                    WriteByte((byte)k);
                    WriteByte((byte)(k >> 8));
                    break;

                case "checkinteractionbyteeq": //D4
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xD4);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    break;

                case "checkmemory": //D5
                case "checkmemoryeq":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[2]);
                    k = ParseHex(args[1]);
                    if (i == -1 || k == -1)
                        return false;
                    WriteByte(0xD5);
                    WriteByte((byte)k);
                    WriteByte((byte)(k >> 8));
                    WriteByte((byte)i);
                    break;

                case "checknotcollidedwithlink_ignorez": //D6
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xD6);
                    break;

                case "setcounter1": //D7
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xD7);
                    WriteByte((byte)i);
                    break;

                // d8 implemented with the alternate checkcounter2iszero opcode
                
                case "checkheartdisplayupdated": //D9
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xD9);
                    break;

                case "checkrupeedisplayupdated": //DA
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xDA);
                    break;

                case "checkcollidedwithlink_ignorez": //DB
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xDB);
                    break;

                // no dc

                case "spawnitem": //DD
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0xDD);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0xDD);
                        WriteByte((byte)i); //yy
                        WriteByte((byte)j); //xx
                    }
                    else
                        return false;
                    break;

                case "giveitem": //DE
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0xDD);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0xDD);
                        WriteByte((byte)i); //yy
                        WriteByte((byte)j); //xx
                    }
                    else
                        return false;
                    break;

                case "jumpifitemobtained": //DF
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xDF);
                    WriteByte((byte)i); //yy
                    WritePointer(j); //xx
                    break;

                case "asm15": //0xE0, 0xE1
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0xE0);
                        WriteByte((byte)i);
                        WriteByte((byte)(i >> 8));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        j = ParseHex(args[2]);
                        if (i == -1 || j == -1)
                            return false;
                        WriteByte(0xE1);
                        WriteByte((byte)i);
                        WriteByte((byte)(i >> 8));
                        WriteByte((byte)(j));
                    }
                    else
                        return false;
                    break;

                case "createpuff": //0xE2
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xE2);
                    break;

                case "playsound": //E3
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xE3);
                    WriteByte((byte)i);
                    break;

                case "setmusic": //E4
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xE4);
                    WriteByte((byte)i);
                    break;

                case "setcc8a": //E5
                case "setdisabledobjects":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xE5);
                    WriteByte((byte)i);
                    break;

                case "spawnenemyhere": //E6
                    if (args.Length == 2) {
                        i = ParseHex(args[1]);
                        if (i == -1)
                            return false;
                        WriteByte(0xE6);
                        WriteByte((byte)(i>>8));
                        WriteByte((byte)(i&0xff));
                    }
                    else if (args.Length == 3) {
                        i = ParseHex(args[1]);
                        k = ParseHex(args[2]);
                        if (i == -1 || k == -1)
                            return false;
                        WriteByte(0xE6);
                        WriteByte((byte)i);
                        WriteByte((byte)k);
                    }
                    else
                        return false;
                    break;

                case "settilepos": //E7
                case "settileat":
                    if (args.Length != 3)
                        return false;
                    i = ParseHex(args[1]);
                    j = ParseHex(args[2]);
                    if (i == -1 || j == -1)
                        return false;
                    WriteByte(0xE7);
                    WriteByte((byte)i);
                    WriteByte((byte)j);
                    break;

                case "settile": //E8
                case "settilehere":
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xE8);
                    WriteByte((byte)i);
                    break;

                case "updatelinkrespawnposition":
                    if (args.Length != 1) //E9
                        return false;
                    WriteByte(0xE9);
                    break;

                case "shakescreen": //EA
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xEA);
                    WriteByte((byte)i);
                    break;

                case "initcollisions": //EB
                    if (args.Length != 1)
                        return false;
                    WriteByte(0xEB);
                    break;

                case "movenpcup": //EC
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xEC);
                    WriteByte((byte)i);
                    break;

                case "movenpcright": //ED
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xED);
                    WriteByte((byte)i);
                    break;

                case "movenpcdown": //EE
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xEE);
                    WriteByte((byte)i);
                    break;

                case "movenpcleft": //EF
                    if (args.Length != 2)
                        return false;
                    i = ParseHex(args[1]);
                    if (i == -1)
                        return false;
                    WriteByte(0xEF);
                    WriteByte((byte)i);
                    break;

                case "setdelay0": //F0
                    WriteByte(0xF0);
                    break;

                case "setdelay1": //F1
                    WriteByte(0xF1);
                    break;

                case "setdelay2": //F2
                    WriteByte(0xF2);
                    break;

                case "setdelay3": //F3
                    WriteByte(0xF3);
                    break;

                case "setdelay4": //F4
                    WriteByte(0xF4);
                    break;

                case "setdelay5": //F5
                    WriteByte(0xF5);
                    break;

                case "setdelay6": //F6
                    WriteByte(0xF6);
                    break;

                case "setdelay7": //F7
                    WriteByte(0xF7);
                    break;

                case "setdelay8": //F8
                    WriteByte(0xF8);
                    break;

                case "setdelay9": //F9
                    WriteByte(0xF9);
                    break;

                case "setdelay10": //FA
                    WriteByte(0xFA);
                    break;

                case "setdelay11": //FB
                    WriteByte(0xFB);
                    break;

                case "setdelay12": //FC
                    WriteByte(0xFC);
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
