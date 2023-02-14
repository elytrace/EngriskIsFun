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
    public partial class Function5 : Form
    {
        public Function5()
        {
            InitializeComponent();
            this.ClientSize = new Size(848, 441);
            InitializeBoard();
            this.BackgroundImage = Image.FromFile("Materials/background.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
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
