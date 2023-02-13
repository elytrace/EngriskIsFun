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
    public partial class LoadingForm : Form
    {
        public int current;
        public int max;
        public LoadingForm()
        {
            InitializeComponent();
            progressText.Text = "Đang khởi tạo...";
        }

        public void increment()
        {
            progressBar.Maximum = max;
            progressBar.Value = current;
            progressText.Text = $"{current} / {max}";
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
