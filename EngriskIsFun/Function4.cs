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
    public partial class Function4 : Form
    {
        public Function4()
        {
            InitializeComponent();
            this.ClientSize = new Size(848, 441);
            InitializeHangmanStates();
            InitializeWord();
            InitializeVirtualKeyboard();
            InitializeGUI();
            this.BackgroundImage = Image.FromFile("Materials/background.png");
        }

        private int currentIndex = 0;
        private Image[] hangmanStates = new Image[7];
        private PictureBox currentState;

        private void InitializeHangmanStates()
        {
            for (int i = 0; i < 7; i++)
            {
                hangmanStates[i] = Image.FromFile("Materials/Hangman/state" + i + ".png");
            }
            currentState = new PictureBox();
            currentState.Size = new Size(300, 300);
            currentState.Image = hangmanStates[0];
            currentState.Location = new Point(50, 50);

            this.Controls.Add(currentState);
        }

        private Button[] characterList = new Button[5];
        private char[] word = new char[5];
        private Button[] alphabetic = new Button[26];
        private bool gameStart;

        private void InitializeWord()
        {
            int x = 425;
            int y = 50;
            for (int i = 0; i < characterList.Length; i++)
            {
                characterList[i] = new Button();
                characterList[i].Location = new Point(x + i * 75, y);
                characterList[i].Size = new Size(50, 50);
                characterList[i].Font = new Font("Arial", 20);
                characterList[i].Enabled = false;
                characterList[i].TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(characterList[i]);
            }
        }

        private void InitializeVirtualKeyboard()
        {
            int x = 375;
            int y = 200;
            for (int i = 0; i < 9; i++)
            {
                alphabetic[i] = new Button();
                alphabetic[i].Location = new Point(x + i * 50, y);
                alphabetic[i].Size = new Size(40, 40);
                alphabetic[i].Font = new Font("Arial", 15);
                alphabetic[i].Text = new String((char)(i + 'A'), 1);
                this.Controls.Add(alphabetic[i]);
            }
            for (int i = 0; i < 9; i++)
            {
                alphabetic[i + 9] = new Button();
                alphabetic[i + 9].Location = new Point(x + i * 50, y + 50);
                alphabetic[i + 9].Size = new Size(40, 40);
                alphabetic[i + 9].Font = new Font("Arial", 15);
                alphabetic[i + 9].Text = new String((char)(i + 9 + 'A'), 1);
                this.Controls.Add(alphabetic[i + 9]);
            }
            for (int i = 0; i < 8; i++)
            {
                alphabetic[i + 18] = new Button();
                alphabetic[i + 18].Location = new Point(x + i * 50, y + 100);
                alphabetic[i + 18].Size = new Size(40, 40);
                alphabetic[i + 18].Font = new Font("Arial", 15);
                alphabetic[i + 18].Text = new String((char)(i + 18 + 'A'), 1);
                this.Controls.Add(alphabetic[i + 18]);
            }

            foreach (var button in alphabetic)
            {
                button.Click += (sender, args) =>
                {
                    if (!gameStart) return;

                    bool found = false;
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == (button.Text.ToCharArray()[0] + 32))
                        {
                            characterList[i].Text = ((char)(word[i] - 32)).ToString();
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        currentIndex++;
                        currentState.Image = hangmanStates[currentIndex];
                        if (currentIndex == hangmanStates.Length - 1)
                        {
                            MessageBox.Show("Thua rồi!");
                            reset.PerformClick();
                            word = new char[5];
                            gameStart = false;
                        }
                    }
                    else
                    {
                        bool won = false;
                        foreach (var btn in characterList)
                        {
                            if (btn.Text != "")
                            {
                                won = true;
                            }
                            else return;
                        }
                        if (won)
                        {
                            MessageBox.Show("Thắng rồi!");
                        }
                    }
                };
            }
        }

        private static dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

        private Button start = new Button();
        private Button reset = new Button();
        private void InitializeGUI()
        {
            start.Location = new Point(50, 375);
            start.Size = new Size(100, 50);
            start.Text = "Bắt đầu";
            start.Font = new Font("Arial", 12, FontStyle.Regular);
            start.Click += (sender, args) =>
            {
                var words = (from word in db.Words where word.Word1.Length == 5 select word).OrderBy(x => Guid.NewGuid()).ToList();
                //var word = db.Words.Where(x => x.Word1.Length == 5).OrderBy(x => Guid.NewGuid()).Take(100).ToList();
                var rand = new Random();
                for (int i = 0; i < 5; i++)
                {
                    word[i] = words[rand.Next(words.Count)].Word1[i];
                    MessageBox.Show(word[i].ToString());
                }
                gameStart = true;
            };

            reset.Location = new Point(250, 375);
            reset.Size = new Size(100, 50);
            reset.Text = "Chơi lại";
            reset.Font = new Font("Arial", 12, FontStyle.Regular);
            reset.Click += (sender, args) =>
            {
                currentIndex = 0;
                currentState.Image = hangmanStates[currentIndex];
                foreach (var btn in characterList)
                {
                    btn.Text = "";
                }
            };

            this.Controls.Add(start);
            this.Controls.Add(reset);
        }
    }
}
