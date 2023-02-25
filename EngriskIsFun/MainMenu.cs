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
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            this.Text = "Trang chủ";
            BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            InitializeButtons();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            DisplayPersonalInfo();
        }

        private void DisplayPersonalInfo()
        {
            Label greet = new Label();
            greet.Location = new Point(Right + 10, Top + 10);
            greet.AutoSize = true;
            greet.Text = "Xin chào, ";
            this.Controls.Add(greet);
        }

        private Button button1 = new Button();
        private Button button2 = new Button();
        private Button button3 = new Button();
        private Button button4 = new Button();
        private Button button5 = new Button();

        private void InitializeButtons()
        {
            button1.Location = new Point(12, 165);
            button1.Name = "button1";
            button1.Size = new Size(240, 85);
            button1.TabIndex = 1;
            button1.Text = "Tra từ";
            button1.Font = new Font("Arial", 20, FontStyle.Regular);
            button1.Click += Button1_Click;
            button1.MouseEnter += (sender, args) =>
            {
                button1.Font = new Font("Arial", 40);
            };
            button1.MouseLeave += (sender, args) =>
            {
                button1.Font = new Font("Arial", 20);
            };

            button2.Location = new Point(12, 255);
            button2.Name = "button2";
            button2.Size = new Size(240, 85);
            button2.TabIndex = 2;
            button2.Text = "button2";

            button3.Location = new Point(12, 345);
            button3.Name = "button3";
            button3.Size = new Size(240, 85);
            button3.TabIndex = 3;
            button3.Text = "button3";

            button4.Location = new Point(258, 165);
            button4.Name = "button4";
            button4.Size = new Size(240, 125);
            button4.TabIndex = 4;
            button4.Text = "HANGMAN";
            button4.Font = new Font("Arial", 20, FontStyle.Regular);
            button4.Click += Button4_Click;
            button4.MouseEnter += (sender, args) =>
            {
                button4.Font = new Font("Arial", 30);
            };
            button4.MouseLeave += (sender, args) =>
            {
                button4.Font = new Font("Arial", 20);
            };

            button5.Location = new Point(258, 305);
            button5.Name = "button5";
            button5.Size = new Size(240, 125);
            button5.TabIndex = 5;
            button5.Text = "PUZZLE";
            button5.Font = new Font("Arial", 20, FontStyle.Regular);
            button5.Click += Button5_Click;
            button5.MouseEnter += (sender, args) =>
            {
                button5.Font = new Font("Arial", 30);
            };
            button5.MouseLeave += (sender, args) =>
            {
                button5.Font = new Font("Arial", 20);
            };

            this.Controls.Add(button5);
            this.Controls.Add(button4);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //StartTransition();
            Function1 form = new Function1();
            this.Visible = false;
            if (!form.IsDisposed) form.ShowDialog();
            this.Visible = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Function4 form = new Function4();
            this.Visible = false;
            if (!form.IsDisposed) form.ShowDialog();
            this.Visible = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Function5 form = new Function5();
            this.Visible = false;
            if (!form.IsDisposed) form.ShowDialog();
            this.Visible = true;
        }
    }

}
