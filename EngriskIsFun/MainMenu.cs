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
        private string username;
        public MainMenu(string username)
        {
            InitializeComponent();
            this.Text = "Engrisk Iz Fun";
            BackgroundImage = Image.FromFile("Materials/Backgrounds/menu.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            InitializeButtons();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.username = username;
            DisplayPersonalInfo();
        }

        private void DisplayPersonalInfo()
        {
            Label username = new Label();
            username.Font = new Font("Arial", 12, FontStyle.Bold);
            username.ForeColor = Color.Crimson;
            username.BackColor = Color.Transparent;
            username.AutoSize = true;
            username.Text = this.username;
            username.Location = new Point(Right - 150 - 10, Top + 10);
            username.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(username);

            Label greet = new Label();
            greet.Font = new Font("Arial", 12, FontStyle.Regular);
            greet.AutoSize = true;
            greet.Text = "Xin chào,";
            greet.BackColor = Color.Transparent;
            greet.Location = new Point(Right - 240, Top + 10);
            greet.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(greet);
        }

        private Button button1 = new Button();
        private Button button2 = new Button();
        private Button button4 = new Button();
        private Button button5 = new Button();

        private void InitializeButtons()
        {
            button1.Location = new Point(123, 50);
            button1.Name = "button1";
            button1.Size = new Size(240, 150);
            button1.ImageAlign = ContentAlignment.TopCenter;
            button1.TextAlign = ContentAlignment.BottomCenter;
            button1.TabIndex = 1;
            button1.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/dictionary.png"), 100, 100);
            button1.Text = "Từ vựng";
            button1.Font = new Font("Arial", 20, FontStyle.Regular);
            button1.Click += Button1_Click;
            button1.MouseEnter += (sender, args) =>
            {
                Sound.Play(Sound.MOUSE_ENTER);
                button1.Font = new Font("Arial", 30);
            };
            button1.MouseLeave += (sender, args) =>
            {
                button1.Font = new Font("Arial", 20);
            };

            button2.Location = new Point(123 + 122 + 240, 50);
            button2.Name = "button2";
            button2.Size = new Size(240, 150);
            button2.TabIndex = 2;
            button2.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/exam.png"), 100, 100);
            button2.ImageAlign = ContentAlignment.TopCenter;
            button2.TextAlign = ContentAlignment.BottomCenter;
            button2.Text = "Kiểm tra";
            button2.Font = new Font("Arial", 20, FontStyle.Regular);
            button2.Click += Button2_Click;
            button2.MouseEnter += (sender, args) =>
            {
                Sound.Play(Sound.MOUSE_ENTER);
                button2.Font = new Font("Arial", 30);
            };
            button2.MouseLeave += (sender, args) =>
            {
                button2.Font = new Font("Arial", 20);
            };


            button4.Location = new Point(123, 250);
            button4.Name = "button4";
            button4.Size = new Size(240, 150);
            button4.TabIndex = 4;
            button4.Text = "Hangman";
            button4.ImageAlign = ContentAlignment.TopCenter;
            button4.TextAlign = ContentAlignment.BottomCenter;
            button4.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/hangman.png"), 100, 100);
            button4.Font = new Font("Arial", 20, FontStyle.Regular);
            button4.Click += Button3_Click;
            button4.MouseEnter += (sender, args) =>
            {
                Sound.Play(Sound.MOUSE_ENTER);
                button4.Font = new Font("Arial", 30);
            };
            button4.MouseLeave += (sender, args) =>
            {
                button4.Font = new Font("Arial", 20);
            };

            button5.Location = new Point(123 + 122 + 240, 250);
            button5.Name = "button5";
            button5.Size = new Size(240, 150);
            button5.TabIndex = 5;
            button5.Text = "Puzzles";
            button5.ImageAlign = ContentAlignment.TopCenter;
            button5.TextAlign = ContentAlignment.BottomCenter;
            button5.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/puzzle.png"), 100, 100);
            button5.Font = new Font("Arial", 20, FontStyle.Regular);
            button5.Click += Button4_Click;
            button5.MouseEnter += (sender, args) =>
            {
                Sound.Play(Sound.MOUSE_ENTER);
                button5.Font = new Font("Arial", 30);
            };
            button5.MouseLeave += (sender, args) =>
            {
                button5.Font = new Font("Arial", 20);
            };

            this.Controls.Add(button5);
            this.Controls.Add(button4);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!this.Controls.Contains(Function1.Instance))
            {
                this.Controls.Add(Function1.Instance);
                Function1.Instance.Dock = DockStyle.Fill;
                Function1.Instance.BringToFront();
            }
            else
                Function1.Instance.BringToFront();
            Function1.Instance.parent = this;
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (!this.Controls.Contains(Function2.Instance))
            {
                this.Controls.Add(Function2.Instance);
                Function2.Instance.Dock = DockStyle.Fill;
                Function2.Instance.BringToFront();
            }
            else
                Function2.Instance.BringToFront();
            Function2.Instance.parent = this;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!this.Controls.Contains(Function3.Instance))
            {
                this.Controls.Add(Function3.Instance);
                Function3.Instance.Dock = DockStyle.Fill;
                Function3.Instance.BringToFront();
            }
            else
                Function3.Instance.BringToFront();
            Function3.Instance.parent = this;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (!this.Controls.Contains(Function4.Instance))
            {
                this.Controls.Add(Function4.Instance);
                Function4.Instance.Dock = DockStyle.Fill;
                Function4.Instance.BringToFront();
            }
            else
                Function4.Instance.BringToFront();
            Function4.Instance.parent = this;
        }
    }

}
