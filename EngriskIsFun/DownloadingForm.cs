using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class DownloadingForm : Form
    {
        public int current;
        public int max;
        public DownloadingForm()
        {
            InitializeComponent();
            progressText.Text = "Đang khởi tạo...";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        public void increment()
        {
            progressBar.Maximum = max;
            progressBar.Value = current;
            // progressText.Text = $"{current} / {max}";
            progressText.Text = current.ToString() + " / " + max.ToString();
        }

        public void displayMessage(string message)
        {
            progressBar.Hide();
            progressText.Size = progressBar.Size;
            progressText.Location = progressBar.Location;
            progressText.Text = message;
            progressText.TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
