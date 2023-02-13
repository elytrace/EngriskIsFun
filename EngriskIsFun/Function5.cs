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
            this.BackgroundImage = Image.FromFile("Materials/background.png");
            InitializeBoard();
        }

        private Button[,] board = new Button[10, 10];

        private void InitializeBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = new Button();
                    board[i, j].Location = new Point(50 + j * 25, 50 + i * 25);
                    board[i, j].Size = new Size(30, 30);
                    board[i, j].Text = (i * 10 + j).ToString();
                    this.Controls.Add(board[i, j]);
                }
            }
        }
    }
}
