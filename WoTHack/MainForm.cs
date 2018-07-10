using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Input;
using WhiteMagic;

namespace WoTHack
{
    public partial class MainForm : Form
    {
        ProcessDebugger pd = null;

        List<ProcessInfo> wotProcesses = new List<ProcessInfo>();
        List<Key> keyBinds = new List<Key>();
        Key toggleTreesKey = Key.None;

        Timer timer = new Timer();

        private static byte[] hash = { 0xAD, 0x4C, 0xE8, 0x35, 0x14, 0x88, 0xE6, 0x79, 0x74, 0xB0, 0x5C, 0xE8, 0x8E, 0xF7, 0x4A, 0x21 };

        public MainForm()
        {
            InitializeComponent();
            ResetButtonText(false);

            timer.Interval = 200;
            timer.Tick += new EventHandler(OnTimerTick);

            keyBinds = Enum.GetValues(typeof(Key)).Cast<Key>().ToList();
            treeToggleKeyComboBox.DataSource = keyBinds;
            treeToggleKeyComboBox.SelectedIndex = keyBinds.IndexOf(Key.E);
            toggleTreesKey = (Key)treeToggleKeyComboBox.SelectedValue;
        }

        static bool toggleState = false;

        void OnTimerTick(object obj, EventArgs args)
        {
            if (pd == null)
                return;

            var newState = Keyboard.IsKeyDown(toggleTreesKey);
            if (newState == toggleState)
                return;

            toggleState = newState;

            if (!noTreesCheckBox.Checked || !inBothModesCheckBox.Checked)
                return;

            pd.RefreshMemory();
            if (pd.HasExited)
                return;

            using (var suspender = pd.MakeSuspender())
            {
                if (toggleState)
                {
                    AlwaysSniperBP.Enabled = false;
                    TreeRaidusBp.Enabled = false;

                    AlwaysSniperBP.WriteVals(false, pd);
                    TreeRaidusBp.WriteVals(10, 15, pd);
                }
                else
                {
                    AlwaysSniperBP.Enabled = true;
                    TreeRaidusBp.Enabled = true;

                    AlwaysSniperBP.WriteVals(true, pd);
                    TreeRaidusBp.WriteVals(1000, 1000, pd);
                }
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            var processes = MagicHelpers.FindProcessesByName("WorldOfTanks");
            wotProcesses = processes.Select(it => new ProcessInfo(it.Id, it.ProcessName)).ToList();

            processesComboBox.DataSource = wotProcesses;
            processesComboBox.DisplayMember = "DisplayName";
        }

        private void processesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ResetButtonText(false);
            if (pd != null)
            {
                pd.StopDebugging();
                pd.Join();
                pd = null;
            }
        }

        private static void ShowError(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DetachHacks()
        {
            timer.Stop();

            if (pd != null)
            {
                pd.StopDebugging();
                pd.Join();
                pd = null;
            }
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            if (pd != null)
            {
                try
                {
                    DetachHacks();
                    ResetButtonText(false);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
                return;
            }

            var procInfo = processesComboBox.SelectedItem as ProcessInfo;
            if (procInfo == null)
            {
                ShowError("Select a process!");
                return;
            }

            try
            {
                if (Process.GetProcessById(procInfo.Id) == null)
                {
                    ShowError(string.Format("Can't find process '{0}'!", procInfo.DisplayName));
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowError(string.Format("Error: {0}", ex.Message));
                return;
            }

            try
            {
                pd = new ProcessDebugger(procInfo.Id);
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(File.ReadAllBytes(pd.Process.MainModule.FileName));
                    Debug.WriteLine(pd.Process.MainModule.FileVersionInfo);
                    Debug.WriteLine(string.Format("MD5: {0}", string.Join(", ", hash.Select(it => string.Format("0x{0:X2}", it)).ToArray())));
                    if (!hash.SequenceEqual(MainForm.hash))
                        throw new Exception("Hash does not match. Probably newer version of WoT executable.");
                }
                pd.Run();
                var now = DateTime.Now;
                while (!pd.WaitForComeUp(50) && now.MSecToNow() < 1000)
                { }
                ToggleHacks();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                return;
            }

            ResetButtonText(true);
        }

        private void ResetButtonText(bool attached)
        {
            if (attached)
                startStopButton.Text = "Detach";
            else
                startStopButton.Text = "Attach";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DetachHacks();
            }
            catch (Exception)
            {
            }
        }

        private void noTreesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            inBothModesCheckBox.Enabled = noTreesCheckBox.Checked;

            ToggleHacks();
        }

        private void inBothModesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToggleHacks();
        }

        private void ToggleHacks()
        {
            if (pd == null)
                return;

            pd.RemoveBreakPoints();
            timer.Stop();

            timer.Start();
            if (noTreesCheckBox.Checked)
                pd.AddBreakPoint(new TreeRaidusBp());
            if (noTreesCheckBox.Checked && inBothModesCheckBox.Checked)
                pd.AddBreakPoint(new AlwaysSniperBP());
        }

        private void treeToggleKeyComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            toggleTreesKey = (Key)treeToggleKeyComboBox.SelectedValue;
        }
    }
}
