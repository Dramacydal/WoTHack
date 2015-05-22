using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhiteMagic;

namespace WoTHack
{
    public partial class MainForm : Form
    {
        ProcessDebugger pd = null;

        List<ProcessInfo> wotProcesses = new List<ProcessInfo>();

        Timer timer = new Timer();

        public MainForm()
        {
            InitializeComponent();
            ResetButtonText(false);

            timer.Interval = 500;
            timer.Tick += new EventHandler(OnTimerTick);
            //timer.Start();
        }

        void OnTimerTick(object obj, EventArgs args)
        {
            if (pd == null)
                return;

            using (var suspender = pd.MakeSuspender())
            {
                if (noTreesCheckBox.Checked && inBothModesCheckBox.Checked)
                    pd.Write<byte>(pd.Process.MainModule.BaseAddress.Add(0x21335E0 - 0x400000), 1);
                if (noTreesCheckBox.Checked)
                {
                    pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335E4 - 0x400000), 1000 * 1000);
                    pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335E8 - 0x400000), 1000);
                    pd.Write<float>(pd.Process.MainModule.BaseAddress.Add(0x21335EC - 0x400000), 1000);
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

        private void bpWayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            threadWayRadioButton.Checked = !bpWayRadioButton.Checked;

            ToggleHacks();
        }

        private void threadWayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bpWayRadioButton.Checked = !threadWayRadioButton.Checked;

            ToggleHacks();
        }


        private void ToggleHacks()
        {
            if (pd == null)
                return;

            pd.RemoveBreakPoints();
            timer.Stop();

            if (threadWayRadioButton.Checked)
            {
                timer.Start();
                return;
            }

            if (noTreesCheckBox.Checked)
                pd.AddBreakPoint(new TreeRaidusBp(), pd.Process.MainModule.BaseAddress);
            if (noTreesCheckBox.Checked && inBothModesCheckBox.Checked)
                pd.AddBreakPoint(new AlwaysSniperBP(), pd.Process.MainModule.BaseAddress);
        }
    }
}
