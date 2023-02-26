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
            InitializeBoard();
            BackgroundImage = Image.FromFile("Materials/Backgrounds/menu.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            DisplayBackBtn();
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
                this.parent.Controls.Remove(this);
            };

            this.Controls.Add(back);
        }

        private const int size = 8;
        private int edge = 40;
        private Button[,] board = new Button[size, size];
        private Panel panel = new Panel();

        private void InitializeBoard()
        {
            panel.Location = new Point(50, 50);
            panel.Size = new Size(320, 320);
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
            panel.Show();
        }

        private string[] words = new string[5];
        private int[,] positions = new int[5, 4]; // head.x, head.y, direction, length
        private const int HORIZONTAL = 0;
        private const int VERTICAL = 1;
        private const int DIAGONAL = 2;

        private void GenerateModel()
        {
            for(int i = 0; i < 5; i++)
            {
                
            }
        }
    }
}
