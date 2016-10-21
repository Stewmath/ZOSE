using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GBHL;

namespace ZOSE
{
    public partial class Form1 : Form
    {
        Compiler compiler;
        GBFile gb;
        string filename = "";
        Emulator emulator;
        bool saved = true;
        string scriptFilename = "";
        bool[] safeAddresses;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        public Form1(string filename)
        {
            InitializeComponent();
            try
            {
                if (filename != "")
                    OpenROM(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private DialogResult DoSave()
        {
            if (!saved)
            {
                DialogResult d = MessageBox.Show("The script has changed.\n\nDo you want to save the changes?", "Confirm", MessageBoxButtons.YesNoCancel);
                if (d == DialogResult.Cancel || d == DialogResult.No)
                    return d;
                if (scriptFilename == "")
                {
                    saved = true;
                    toolStripMenuItem4_Click(null, null);
                    return DialogResult.No;
                }
            }
            try
            {
                StreamWriter sw = new StreamWriter(File.Open(scriptFilename, FileMode.OpenOrCreate));
                sw.Write(textBox1.Text);
                sw.Close();
                saved = true;
                this.Text = "ZOSE - Zelda Oracles Script Editor";
                return DialogResult.Yes;
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error saving:\n\n" + ex.Message, "Error Saving");
                return DialogResult.Cancel;
            }
        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Open ROM";
            o.Filter = "All Supported Types|*.gbc;*.bin";
            if (o.ShowDialog() != DialogResult.OK)
                return;
            OpenROM(o.FileName);
        }

        private void OpenROM(string file)
        {
            filename = file;
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            byte[] buffer = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            if (buffer.Length < 0x1FFFFF)
            {
                MessageBox.Show("Error! ROM has not been expanded. Cannot proceed. Open it in ZOLE.", "Error");
                return;
            }
            gb = new GBFile(buffer);
            emulator = new Emulator(gb);
            /*gb.BufferLocation = 0x1FFFFD;
            int loc = gb.ReadByte() + gb.ReadByte() * 0x100;
            if (loc == 0)
                loc = WriteASMScript();*/
            if (gb.ReadByte(0x20) == 0)
            {
                if (MessageBox.Show("Warning: ZOSE applies a patch to use interaction 72 which allows us\nto create scripts anywhere we want with ease. However, you\nwill not be able to use the Great Moblin event anymore. Continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                WriteASMScript();
            }
            /*else if (gb.ReadByte(0x3F6A) != 0)
            {
                gb.WriteByte(0x3F6A, 0);
            }*/
            compiler = new Compiler(gb);
            safeAddresses = new bool[gb.Buffer.Length];
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compiler == null)
                return;
            if (!compiler.Compile(textBox1.Text))
            {
                MessageBox.Show("Error compiling.", "Error");
                return;
            }
            foreach (GBScript s in compiler.scripts)
            {
                if (s == null || s.compileLocation == -1)
                    continue;
                gb.BufferLocation = s.compileLocation;
                int length = s.output.BufferLocation;
                if (length >= 0xFF)
                {
                    MessageBox.Show("Warning! Write Location " + s.compileLocation.ToString("X") + " compiles to more than 256 bytes!\nNot all of it will be executed if jumped to by a 3-byte pointer.\nTo fix this, do an unconditional 3-byte jump to somewhere else with the code continuing.\nThe script will not be written until this is fixed for safety purposes.", "Warning");
                    continue;
                }
                s.output.BufferLocation = 0;
                byte[] previous = new byte[gb.Buffer.Length];
                Array.Copy(gb.Buffer, previous, previous.Length);
                bool warn = false;
                for (int i = 0; i < length; i++)
                {
                    if (gb.ReadByte() == 0 || safeAddresses[gb.BufferLocation - 1])
                    {
                        safeAddresses[gb.BufferLocation - 1] = true;
                        gb.BufferLocation--;
                    }
                    else if (!warn)
                    {
                        if (MessageBox.Show("Warning! Compiling script " + s.compileLocation.ToString("X") + " will overwrite other data in the ROM!\nAlthough the data may just be a previously-compiled script, at the same time it may not be.\n\nAre you sure you want to continue?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            gb.Buffer = previous;
                            goto NextScript;
                        }
                        safeAddresses[gb.BufferLocation - 1] = true;
                        gb.BufferLocation--;
                        warn = true;
                    }
                    else
                    {
                        safeAddresses[gb.BufferLocation - 1] = true;
                        gb.BufferLocation--;
                    }
                    gb.WriteByte(s.output.ReadByte());
                }
                gb.WriteByte(0);
            NextScript:
                continue;
            }
            /*gb.BufferLocation = compiler.compileAddress;
            int length = compiler.output.BufferLocation;
            compiler.output.BufferLocation = 0;
            for (int i = 0; i < length; i++)
            {
                gb.WriteByte(compiler.output.ReadByte());
            }*/
            //gb.WriteByte(0);
            //gb.WriteByte(0xC9); //ret
            BinaryWriter br = new BinaryWriter(File.Open(filename, FileMode.Open));
            br.Write(gb.Buffer);
            br.Close();
        }

        public void WriteASMScript()
        {
            if (MessageBox.Show("Warning! Bank C will be replaced by all the default data!\nYour old script pointers will be erased and you'll have to re-compile your script!\nAre you sure you want to continue (Highly recommended to better engine)?", "Warning", MessageBoxButtons.YesNo) != DialogResult.No)
            {
                this.gb.WriteBytes(0x30000, ZOSE.Properties.Resources.bankc);
                this.gb.WriteBytes(0x20, new byte[0x1a]);
                this.gb.WriteBytes(0x2be09, new byte[] { 
                30, 70, 0x1a, 0xfe, 0, 0x20, 0x24, 30, 0x42, 0x21, 0, 0x40, 0x1a, 6, 0, 0x4f, 
                9, 9, 9, 30, 0x44, 0x1a, 0xfe, 0, 0x20, 15, 60, 30, 0x44, 0x12, 0xe5, 0xcd, 
                0x80, 0x26, 0x21, 0, 0xc3, 0xcd, 0x44, 0x25, 0xe1, 0xc3, 0xbc, 0x3f, 0x3d, 0x12, 0xc9
             });
                this.gb.WriteByte(0x3c6f, 9);
                this.gb.WriteByte(0x3c70, 0x7e);
                this.gb.WriteBytes(0x3fbc, new byte[] { 
                30, 0x7d, 0x1a, 0xfe, 0, 40, 7, 0x7a, 0x67, 0x7b, 0x6f, 0x1a, 0x18, 4, 0x3e, 0xff, 
                0xe0, 0x97, 0xea, 0x22, 0x22, 0x2a, 0x47, 0x2a, 0x66, 0x6f, 120, 0xe0, 0x97, 0xea, 0x22, 0x22, 
                0xd5, 0x11, 0, 0xc3, 0x2a, 0x12, 0x13, 0x7a, 0xfe, 0xc4, 0x20, 0xf8, 0xd1, 30, 0x58, 0x1a, 
                0x6f, 0x1c, 0x1a, 0x67, 0xcd, 0x18, 0x25, 0x38, 5, 0xcd, 0x88, 0x25, 0xaf, 0xc9, 0xcd, 0x88, 
                0x25, 0x37, 0xc9
             });
                this.gb.WriteBytes(0x30103, new byte[] { 0x2d, 0x3f });
                this.gb.WriteBytes(0x3f2d, new byte[] { 0xe1, 0x23, 0x2a, 0xe5, 0x21, 0xf3, 0x7f, 0xdf, 0x2a, 0x66, 0x6f, 0xe9 });
                this.gb.WriteBytes(0x3f39, new byte[] { 0x21, 0, 0xc3, 0xe5, 0xcd, 0x44, 0x25, 0xe1, 0xc9, 0xbc, 0x3f });
                this.PatchOtherOpcodes(false);
                BinaryWriter writer = new BinaryWriter(File.Open(this.filename, FileMode.Open));
                writer.Write(this.gb.Buffer);
                writer.Close();
            }

        }

        public void PatchOtherOpcodes(bool write)
        {
            this.PatchOpcode(0, 0x33f94);
            this.gb.WriteBytes(0x33f94, new byte[] { 0xe1, 6, 3, 30, 0x7d, 0x2a, 0x12, 0x13, 5, 0x20, 250, 0xc3, 0x39, 0x3f, 0 });
            this.PatchOpcode(1, 0x33fa3);
            this.gb.WriteBytes(0x33fa3, new byte[] { 0xe1, 0x2a, 0x47, 0xe5, 0xcd, 0x7d, 0x19, 0xe1, 160, 0x20, 0xe7, 0x23, 0x23, 0x23, 0xc9 });
            this.PatchOpcode(2, 0x33fb2);
            this.gb.WriteBytes(0x33fb2, new byte[] { 
            0xe1, 0x2a, 70, 0x4f, 10, 0x23, 0x47, 0x2a, 0xb8, 40, 4, 0x23, 0x23, 0x23, 0xc9, 0xd5, 
            0xcd, 0x95, 0x7f, 0xd1, 0xc9
         });
            this.PatchOpcode(3, 0x33fc7);
            this.gb.WriteBytes(0x33fc7, new byte[] { 
            0xe1, 0x2a, 0x47, 0x2a, 0xe5, 0xcd, 0x8a, 0x19, 0xe1, 0x47, 0x2a, 160, 0x20, 0xc0, 0x23, 0x23, 
            0x23, 0xc9
         });
            this.PatchOpcode(4, 0x33fd9);
            this.gb.WriteBytes(0x33fd9, new byte[] { 0xe1, 0x2a, 0x4f, 0x2a, 0x47, 0x2a, 0xe5, 0xcd, 0x8a, 0x19, 0x79, 0xa6, 0x77, 0xe1, 0xc9 });
            if (write)
            {
                BinaryWriter writer = new BinaryWriter(File.Open(this.filename, FileMode.Open));
                writer.Write(this.gb.Buffer);
                writer.Close();
            }

        }

        public void PatchOpcode(int opcode, int location)
        {
            this.gb.BufferLocation = 0x33ff3 + (opcode * 2);
            this.gb.WriteBytes(this.gb.Get2BytePointer(location));
        }

        public int FindFreeSpace(int amount, byte bank)
        {
            int found = 0;
            gb.BufferLocation = bank * 0x4000;
            if (bank == 0)
                gb.BufferLocation += 0x2000;
            while (found < amount)
            {
                if (gb.BufferLocation == gb.Buffer.Length - 1 || gb.BufferLocation == bank * 0x4000 + 0x3FFF)
                {
                    return -1;
                }
                if (gb.ReadByte() != 0)
                {
                    found = 0;
                    continue;
                }
                found++;
            }

            return gb.BufferLocation - found;
        }

        private void decompileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (compiler == null)
                return;
            frmDecompile f = new frmDecompile();
            if (f.ShowDialog() != DialogResult.OK)
                return;
            compiler.globalJumps = new List<int>();
            int address = (int)f.nAddress.Value;
            string s = compiler.Decompile(address);
            textBox1.Text = s;
        }

        private void decompileInteractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compiler == null)
                return;
            frmDecompileInteraction f = new frmDecompileInteraction();
            if (f.ShowDialog() != DialogResult.OK)
                return;
            compiler.globalJumps = new List<int>();
            if ((int)f.nID.Value >> 8 == 0x20)
            {
                int dungeon = (int)f.nDungeon.Value;
                int id = (int)f.nID.Value;
                gb.BufferLocation = 0x20C2C + (dungeon * 2);
                gb.BufferLocation = 0x20000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                gb.BufferLocation += (id & 0xFF) * 2;
                gb.BufferLocation = 0x30000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
                string s = compiler.Decompile(gb.BufferLocation);
                for (int i = 0; i < compiler.globalJumps.Count; i++)
                {
                    s += "\r\n\r\n" + compiler.Decompile(compiler.globalJumps[i] < 0x8000 ? compiler.globalJumps[i] + 0x2C000 : compiler.globalJumps[i]);
                }
                textBox1.Text = s;
            }
            else
            {
                int address = emulator.EmulateUntilScript((int)f.nID.Value, (int)f.nX.Value, (int)f.nY.Value);
                if (address == -1)
                {
                    MessageBox.Show("No script for that interaction.", "Missing Script");
                    return;
                }
                if (address < 0x100)
                {
                    MessageBox.Show("Unknown opcode " + address.ToString("X") + ".", "Error");
                    return;
                }
                string s = compiler.Decompile(address);
                for (int i = 0; i < compiler.globalJumps.Count; i++)
                {
                    s += "\r\n\r\n" + compiler.Decompile(compiler.globalJumps[i] < 0x8000 ? compiler.globalJumps[i] + 0x2C000 : compiler.globalJumps[i]);
                }
                textBox1.Text = s;
            }
        }

        private void findFreeSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gb == null)
                return;
            frmFreeSpace f = new frmFreeSpace(gb);
            if (f.ShowDialog() != DialogResult.OK)
                return;
            if (f.SpaceLocation == -1)
                return;
            int start = textBox1.SelectionStart;
            if (start == -1)
                start = 0;
            string text = "writelocation " + f.SpaceLocation.ToString("X") + "\r\n";
            textBox1.Text = textBox1.Text.Insert(start, text);
            textBox1.SelectionStart += text.Length;
        }

        char[] splitchars = new char[] { '\n', ' ', '.' };
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            saved = false;
            this.Text = "ZOSE - Zelda Oracles Script Editor*";
            PerformIntellisenseCheck(true);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!lstIntellisense.Visible || textBox1.SelectionLength > 0)
                return;
            saved = false;
            this.Text = "ZOSE - Zelda Oracles Script Editor*";
            if (e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                int startIndex = textBox1.SelectionStart;
                int first = startIndex;
                int originalCount = 1;
                for (int i = startIndex - 1; i > -1; i--)
                {
                    originalCount++;
                    if (textBox1.Text[i] == ' ' || textBox1.Text[i] == '\r' || textBox1.Text[i] == '\n')
                    {
                        startIndex = i + 1;
                        break;
                    }
                }
                if (startIndex == textBox1.SelectionStart)
                    startIndex = first = 0;
                string command = (string)lstIntellisense.SelectedItem;
                int length = command.Length - (startIndex - first);
                string text = textBox1.Text;
                text = text.Remove(startIndex, textBox1.SelectionStart - startIndex);
                text = text.Insert(startIndex, command + (e.KeyChar == (char)Keys.Space ? " " : "\r\n"));
                textBox1.Text = text;
                textBox1.SelectionStart = startIndex + command.Length + (e.KeyChar == (char)Keys.Space ? 1 : 2);
                textBox1.ScrollToCaret();
            }
        }

        private void PerformIntellisenseCheck(bool checkFull)
        {
            if (textBox1.SelectionStart == 0)
            {
                lstIntellisense.Visible = false;
                return;
            }
            int line = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            string text = textBox1.Text.Replace("\r", "");
            string[] commands = text.Split(splitchars);
            int cstart = -1;
            int currentIndex = 0;
            for (int i = 0; i < commands.Length; i++)
            {
                if (currentIndex + commands[i].Length >= textBox1.SelectionStart - 1)
                {
                    cstart = i;
                    break;
                }
                currentIndex += commands[i].Length + 1 + (textBox1.Text[currentIndex] == '\n' ? 1 : 0); //+1 for the split character
            }
            if (cstart == -1)
            {
                lstIntellisense.Visible = false;
            }
            else
            {
                string command = commands[cstart].ToLower();
                if (command.Length == 0)
                {
                    lstIntellisense.Visible = false;
                    return;
                }
                if ((textBox1.Text[textBox1.SelectionStart - 1] == ' ' || textBox1.Text[textBox1.SelectionStart - 1] == '\n') && lstIntellisense.Visible && lstIntellisense.SelectedIndex != -1)
                {
                    /*string finaltext = textBox1.Text;
                    bool newline = false;
                    if (textBox1.Text[textBox1.SelectionStart - 1] == '\n')
                    {
                        finaltext = finaltext.Remove(textBox1.SelectionStart - 2, 2);
                        finaltext = finaltext.Insert(textBox1.SelectionStart - 2, " ");
                        textBox1.SelectionStart--;
                        newline = true;
                    }
                    int temp = textBox1.SelectionStart;
                    textBox1.SelectionStart--;
                    int originalLength = command.Length;
                    command = (string)lstIntellisense.SelectedItem;
                    int addedchars = 0;
                    for (int i = command.Length - 1 - (originalLength - 1); i > -1 + line; i--) //for (int i = 0; i < commands[currentIndex].Length; i++)
                    {
                        if (currentIndex + i >= textBox1.Text.Length - 1 - line * 2)
                        {
                            finaltext = finaltext.Insert(textBox1.Text.Length - 1 - (newline ? 1 : 0), command[i].ToString());
                            addedchars++;
                        }
                        else if (textBox1.Text[currentIndex + i] != command[i])
                        {
                            finaltext = finaltext.Insert(currentIndex + i, command[i].ToString());
                            addedchars++;
                        }
                    }
                    textBox1.Text = finaltext;
                    textBox1.SelectionStart = temp;
                    textBox1.SelectionStart += addedchars + 1;
                    if (newline)
                    {
                        finaltext = textBox1.Text;
                        finaltext = finaltext.Remove(textBox1.SelectionStart - 1, 1);
                        finaltext = finaltext.Insert(textBox1.SelectionStart - 1, "\r\n");
                        textBox1.Text = finaltext;
                        textBox1.SelectionStart = temp;
                        textBox1.SelectionStart += addedchars + 2;
                    }*/
                    lstIntellisense.Visible = false;
                    return;
                }
                lstIntellisense.Items.Clear();
                if (command == "")
                {
                    lstIntellisense.Visible = false;
                    textBox1.Focus();
                    return;
                }
                int y = 0;
                int x = 0;
                line = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
                if (line == 0)
                {
                    x = textBox1.SelectionStart * 8 + 2;
                }
                int l = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        l++;
                        if (l == line)
                        {
                            x = textBox1.SelectionStart - i - (l);
                            x *= 8;
                            x -= 6;
                            break;
                        }
                    }
                }
                y = line * 16;
                y += 16;
                y -= GetScrollPos(textBox1.Handle, 1) * 16;
                x -= GetScrollPos(textBox1.Handle, 0);
                foreach (string s in Compiler.commands)
                {
                    if (checkFull && s == command)
                    {
                        lstIntellisense.Visible = false;
                        return;
                    }
                    if (s.StartsWith(command))
                    {
                        lstIntellisense.Items.Add(s);
                    }
                }
                if (lstIntellisense.Items.Count == 0)
                {
                    lstIntellisense.Visible = false;
                    textBox1.Focus();
                    return;
                }
                lstIntellisense.Left = x;
                lstIntellisense.Top = y;
                if (lstIntellisense.Top + this.Top + lstIntellisense.Height + 16 > this.Top + this.Height)
                    lstIntellisense.Top -= lstIntellisense.Height + 16;
                if (!lstIntellisense.Visible)
                    lstIntellisense.Visible = true;
                lstIntellisense.SelectedIndex = 0;
                textBox1.Focus();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            lstIntellisense.Items.Clear();
            lstIntellisense.Visible = false;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lstIntellisense.Items.Clear();
            lstIntellisense.Visible = false;
        }

        private void lstIntellisense_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void lstIntellisense_DoubleClick(object sender, EventArgs e)
        {
            if (lstIntellisense.SelectedIndex != -1)
            {
                textBox1_KeyPress(this, new KeyPressEventArgs(' '));
            }
        }

        private void showIntellisenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstIntellisense.Items.Clear();
            if (lstIntellisense.Visible)
            {
                lstIntellisense.Visible = false;
            }
            else
            {
                PerformIntellisenseCheck(false);
            }
            textBox1.Focus();
        }

        private void reApplyASMPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compiler == null)
                return;

        }

        private void applyNewOpcodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatchOtherOpcodes(true);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Title = "Open Script";
            o.Filter = "Script Files (*.zss)|*.zss|Text Files (*.txt)|*.txt|AlL Files (*.*)|*.*";
            if (o.ShowDialog() != DialogResult.OK)
                return;
            if (!saved)
            {
                if (DoSave() == DialogResult.Cancel)
                    return;
            }
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(o.FileName));
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
                scriptFilename = o.FileName;
                saved = true;
                this.Text = "ZOSE - Zelda Oracles Script Editor";
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error opening script:\n\n" + ex.Message);

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (scriptFilename == "")
            {
                toolStripMenuItem4_Click(null, null);
                return;
            }
            saved = true;
            DoSave();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Title = "Save Script";
            s.Filter = "Script Files (*.zss)|*.zss|Text Files (*.txt)|*.txt|AlL Files (*.*)|*.*";
            if (s.ShowDialog() != DialogResult.OK)
                return;
            scriptFilename = s.FileName;
            saved = true;
            DoSave();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                if (DoSave() == DialogResult.Cancel)
                    return;
            }
            scriptFilename = "";
            textBox1.Text = "";
            saved = true;
            this.Text = "ZOSE - Zelda Oracles Script Editor";
        }

        private void decompile72InteractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.gb != null)
            {
                frmDecompile72 decompile = new frmDecompile72();
                if (decompile.ShowDialog() == DialogResult.OK)
                {
                    int address = 0x3fc000 + (((byte)decompile.nID.Value) * 3);
                    this.gb.BufferLocation = address;
                    address = ((this.gb.ReadByte() * 0x4000) + this.gb.ReadByte()) + ((this.gb.ReadByte() - 0x40) * 0x100);
                    if (address < 0)
                    {
                        MessageBox.Show("No script assigned with the specified ID.", "Error");
                    }
                    else
                    {
                        this.compiler.globalJumps = new List<int>();
                        string str = this.compiler.Decompile(address);
                        for (int i = 0; i < this.compiler.globalJumps.Count; i++)
                        {
                            str = str + "\r\n\r\n" + this.compiler.Decompile((this.compiler.globalJumps[i] < 0x8000) ? (this.compiler.globalJumps[i] + 0x2c000) : this.compiler.globalJumps[i]);
                        }
                        this.textBox1.Text = str;
                    }
                }
            }
        }
    }
}
