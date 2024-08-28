using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mission4
{
    public partial class Form1 : Form
    {
        private DateTime[] startTimes = new DateTime[5];
        private DateTime[] stopTimes = new DateTime[5];
        private bool[] isRunning = new bool[5];
        private Timer[] timers = new Timer[5];

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < timers.Length; i++)
            {
                
                int currentIndex = i;
                timers[i] = new Timer();
                timers[i].Interval = 1000;
                timers[i].Tick += (sender, e) => UpdateDuration(currentIndex);
            }
        }

        private void UpdateDuration(int index)
        {
            if (index >= 0 && index < startTimes.Length && isRunning[index])
            {
                TimeSpan duration = DateTime.Now - startTimes[index];
                Controls["durationLabel" + (index + 1)].Text = $"{duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && int.TryParse(btn.Tag.ToString(), out int index))
            {
                startTimes[index] = DateTime.Now;
                isRunning[index] = true;
                timers[index].Start();

                var statusLabel = Controls["StatusLabel" + (index + 1)] as Label;
                if (statusLabel != null)
                {
                    statusLabel.Text = "Start";
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && int.TryParse(btn.Tag.ToString(), out int index))
            {
                stopTimes[index] = DateTime.Now;
                isRunning[index] = false;
                timers[index].Stop();

                TimeSpan duration = stopTimes[index] - startTimes[index];
                Controls["StatusLabel" + (index + 1)].Text = "Stop";

                int menit = (int)Math.Ceiling(duration.TotalMinutes);
                int charge = (menit / 10 + (menit % 10 == 0 ? 0 : 1)) * 1000;

                dataGridView1.Rows.Add(
                "Komputer " + (index + 1),
                startTimes[index].ToString("HH:mm:ss"),
                stopTimes[index].ToString("HH:mm:ss"),
                duration.ToString(),
                charge
            );
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                StringBuilder billInfo = new StringBuilder();

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                if (selectedRow.Cells[0].Value != null)
                {
                    string namaKomputer = selectedRow.Cells[0].Value.ToString();
                    string durasiPenggunaan = selectedRow.Cells[3].Value.ToString();
                    string biaya = selectedRow.Cells[4].Value.ToString();

                    billInfo.AppendLine($"Nama Komputer: {namaKomputer}");
                    billInfo.AppendLine($"Durasi Penggunaan: {durasiPenggunaan}");
                    billInfo.AppendLine($"Biaya: Rp {biaya}");
                }

                MessageBox.Show(billInfo.ToString(), "Tagihan");
            }
            else
            {
                MessageBox.Show("Silakan pilih baris terlebih dahulu.", "Peringatan");
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
