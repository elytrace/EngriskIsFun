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

            BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            InitializeButtons();
        }

        private Button button1 = new Button();
        private Button button2 = new Button();
        private Button button3 = new Button();
        private Button button4 = new Button();
        private Button button5 = new Button();
        private Button button6 = new Button();
        private Button button7 = new Button();

        private void InitializeButtons()
        {
            button1.Location = new Point(12, 75);
            button1.Name = "button1";
            button1.Size = new Size(240, 85);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.Click += Button1_Click;

            button2.Location = new Point(12, 165);
            button2.Name = "button2";
            button2.Size = new Size(240, 85);
            button2.TabIndex = 1;
            button2.Text = "button2";

            button3.Location = new Point(12, 255);
            button3.Name = "button3";
            button3.Size = new Size(240, 85);
            button3.TabIndex = 2;
            button3.Text = "button3";

            button4.Location = new Point(12, 345);
            button4.Name = "button4";
            button4.Size = new Size(240, 85);
            button4.TabIndex = 3;
            button4.Text = "button4";

            button5.Location = new Point(258, 75);
            button5.Name = "button5";
            button5.Size = new Size(240, 113);
            button5.TabIndex = 4;
            button5.Text = "button5";

            button6.Location = new Point(258, 196);
            button6.Name = "button6";
            button6.Size = new Size(240, 113);
            button6.TabIndex = 5;
            button6.Text = "button6";

            button7.Location = new Point(258, 317);
            button7.Name = "button7";
            button7.Size = new Size(240, 113);
            button7.TabIndex = 6;
            button7.Text = "button7";

            this.Controls.Add(button7);
            this.Controls.Add(button6);
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
    }

}
