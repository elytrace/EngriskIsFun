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
    public partial class Function4 : UserControl
    {
        private static Function4 _instance;
        public static Function4 Instance
        {
            get
            {
                if (_instance == null) _instance = new Function4();
                return _instance;
            }
        }
        public Form parent { get; set; }
        public Function4()
        {
            InitializeComponent();
            this.ClientSize = new Size(848, 441);
            InitializeBoard(() =>
            {
                panel.Show();
            });
            BackgroundImage = Image.FromFile("Materials/Backgrounds/menu.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            DisplayBackBtn();
            InitializeVirtualKeyboard();
        }
        private void DisplayBackBtn()
        {
            Button back = new Button();
            back.Location = new Point(10, 10);
            back.Size = new Size(80, 34);
            back.Text = "< Back";
            back.Font = new Font("Arial", 10, FontStyle.Regular);
            back.TextAlign = ContentAlignment.MiddleCenter;
            back.Click += (sender, args) =>
            {
                _instance = null;
                this.parent.Controls.Remove(this);
            };

            this.Controls.Add(back);
        }

        private int size = 8;
        private int edge = 40;
        private Button[,] board;
        private Panel panel = new Panel();
        private Random rand = new Random();
        private void InitializeBoard(Action onFinish)
        {
            size = rand.Next(4, 8) + 1;
            edge = 300 / size;
            panel.BorderStyle = BorderStyle.None;
            panel.BackColor = Color.Transparent; 
            board = new Button[size, size];
            panel.Location = new Point(50, 70);
            panel.Size = new Size(300, 300);
            panel.Hide();
            this.Controls.Add(panel);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new Button();
                    board[i, j].Location = new Point(j * edge, i * edge);
                    board[i, j].Size = new Size(edge, edge);
                    board[i, j].Text = (i * size + j).ToString();
                    panel.Controls.Add(board[i, j]);
                }
            }
            onFinish.Invoke();
        }

        private Button[] keyboard = new Button[26];
        private void InitializeVirtualKeyboard()
        {
            int x = 410;
            int y = 250;
            for (int i = 0; i < 9; i++)
            {
                keyboard[i] = new Button();
                keyboard[i].Location = new Point(x + i * 40, y);
                keyboard[i].Size = new Size(40, 40);
                keyboard[i].Font = new Font("Arial", 15);
                keyboard[i].Text = new String((char)(i + 'A'), 1);
                this.Controls.Add(keyboard[i]);
            }
            for (int i = 0; i < 9; i++)
            {
                keyboard[i + 9] = new Button();
                keyboard[i + 9].Location = new Point(x + i * 40, y + 40);
                keyboard[i + 9].Size = new Size(40, 40);
                keyboard[i + 9].Font = new Font("Arial", 15);
                keyboard[i + 9].Text = new String((char)(i + 9 + 'A'), 1);
                this.Controls.Add(keyboard[i + 9]);
            }
            for (int i = 0; i < 8; i++)
            {
                keyboard[i + 18] = new Button();
                keyboard[i + 18].Location = new Point(x + i * 40, y + 80);
                keyboard[i + 18].Size = new Size(40, 40);
                keyboard[i + 18].Font = new Font("Arial", 15);
                keyboard[i + 18].Text = new String((char)(i + 18 + 'A'), 1);
                this.Controls.Add(keyboard[i + 18]);
            }
        }
    }
}
