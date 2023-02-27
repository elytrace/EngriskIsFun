using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class Function3 : UserControl
    {
        private static Function3 _instance;
        public static Function3 Instance
        {
            get
            {
                if (_instance == null) _instance = new Function3();
                return _instance;
            }
        }
        public Form parent { get; set; }
        public Function3()
        {
            InitializeComponent();
            this.Text = "The Hangman";
            this.ClientSize = new Size(848, 441); 
            InitializeHangmanStates();
            InitializeVirtualKeyboard();
            InitializeGUI();
            this.BackgroundImage = Image.FromFile("Materials/Backgrounds/hangman.png");
            BackgroundImageLayout = ImageLayout.Stretch;
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
                _instance = null;
                this.parent.Controls.Remove(this);
            };

            this.Controls.Add(back);
        }

        private int currentIndex = 0;
        private const int MAXSTATE = 11;
        private Image[] hangmanStates = new Image[MAXSTATE];
        private int level;
        private PictureBox currentState;
        private Label lives = new Label();

        private void InitializeHangmanStates()
        {
            for (int i = 0; i < MAXSTATE; i++)
            {
                hangmanStates[i] = Image.FromFile("Materials/Hangman/state" + i + ".png");
            }
            currentState = new PictureBox();
            currentState.Size = new Size(300, 300);
            currentState.Image = hangmanStates[0];
            currentState.Location = new Point(50, 50);
            currentState.BorderStyle = BorderStyle.FixedSingle;

            lives.Size = new Size(100, 20);
            lives.Location = new Point(150, 330);
            lives.TextAlign = ContentAlignment.MiddleCenter;
            lives.Text = "0/0";
            lives.BackColor = Color.White;

            this.Controls.Add(lives);
            this.Controls.Add(currentState);
        }

        private void SetHangmanState()
        {
            currentIndex = (level - 4) * 2;
            currentState.Image = hangmanStates[(level - 4) * 2];
        }

        private Button[] characterList;
        private string chosenWord;
        private Button[] keyboard = new Button[26];
        private bool gameStart;

        private void InitializeWord()
        {
            if(characterList != null) 
                foreach(var btn in characterList)
                    this.Controls.Remove(btn);

            characterList = new Button[level];
            int x = 400;
            int y = 50;
            for (int i = 0; i < characterList.Length; i++)
            {
                characterList[i] = new Button();
                characterList[i].Location = new Point(x + i * (50 + (399 - 50 * characterList.Length) / (characterList.Length-1)), y);
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
                keyboard[i] = new Button();
                keyboard[i].Location = new Point(x + i * 50, y);
                keyboard[i].Size = new Size(40, 40);
                keyboard[i].Font = new Font("Arial", 15);
                keyboard[i].Text = new String((char)(i + 'A'), 1);
                this.Controls.Add(keyboard[i]);
            }
            for (int i = 0; i < 9; i++)
            {
                keyboard[i + 9] = new Button();
                keyboard[i + 9].Location = new Point(x + i * 50, y + 50);
                keyboard[i + 9].Size = new Size(40, 40);
                keyboard[i + 9].Font = new Font("Arial", 15);
                keyboard[i + 9].Text = new String((char)(i + 9 + 'A'), 1);
                this.Controls.Add(keyboard[i + 9]);
            }
            for (int i = 0; i < 8; i++)
            {
                keyboard[i + 18] = new Button();
                keyboard[i + 18].Location = new Point(x + i * 50, y + 100);
                keyboard[i + 18].Size = new Size(40, 40);
                keyboard[i + 18].Font = new Font("Arial", 15);
                keyboard[i + 18].Text = new String((char)(i + 18 + 'A'), 1);
                this.Controls.Add(keyboard[i + 18]);
            }

            foreach (var button in keyboard)
            {
                button.Click += (sender, args) =>
                {
                    if(!gameStart)
                    {
                        MessageBox.Show("Ấn Bắt đầu để chơi!");
                        return;
                    }
                    button.Enabled = false;
                    if (!gameStart) return;

                    bool found = false;
                    for (int i = 0; i < chosenWord.Length; i++)
                    {
                        if (chosenWord[i] == (button.Text.ToCharArray()[0] + 32))
                        {
                            characterList[i].Text = ((char)(chosenWord[i] - 32)).ToString();
                            found = true;
                            Sound.Play(Sound.CORRECT);
                        }
                    }
                    if (!found)
                    {
                        currentIndex++;
                        currentState.Image = hangmanStates[currentIndex];
                        if (currentIndex == MAXSTATE-1)
                        {
                            Sound.Play(Sound.LOSE);
                            if (MessageBox.Show("Từ phải tìm là " + chosenWord.ToUpper() + "\nXem định nghĩa của từ nhé bạn?", "Thua rồi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Popup popup = new Popup(Popup.LOSE_GAME, chosenWord, 0) { Owner = this.ParentForm };
                                if (!popup.IsDisposed) popup.ShowDialog(this);
                            }
                            else
                            {
                                chosenWord = "";
                                gameStart = false;
                            }
                        }
                        else
                        {
                            Sound.Play(Sound.INCORRECT);
                        }
                        lives.Text = (int.Parse(lives.Text[0].ToString()) + 1).ToString() + "/" + GetDifficulty();
                    }
                    else
                    {
                        bool won = false;
                        foreach (var btn in characterList)
                        {
                            if (btn.Text != "") won = true;
                            else return;
                        }
                        if (won)
                        {
                            Sound.Play(Sound.WIN);
                            MessageBox.Show("Thắng rồi!");
                        }
                    }
                };
            }
        }

        private int GetDifficulty()
        {
            return (6 - level) * 3 + level;
        }

        private dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

        private Button start = new Button();
        private void InitializeGUI()
        {
            start.Location = new Point(150, 375);
            start.Size = new Size(100, 50);
            start.Text = "Bắt đầu";
            start.Font = new Font("Arial", 12, FontStyle.Regular);
            start.Click += (sender, args) =>
            {
                string json = null;
                var wordList = (from word in db.WordsLessThan7s where word.Text.Length <= 6 & word.Text.Length >= 4 select word).ToList();
                var rand = new Random();

                while (json == null)
                {
                    chosenWord = wordList[rand.Next(wordList.Count)].Text;
                    UtilityTools.DoGetRequest(Constants.WORD_DEFINITION_URL + chosenWord, result =>
                    {
                        json = result;
                    }, null);
                }
                gameStart = true;
                level = chosenWord.Length;
                lives.Text = "0/" + GetDifficulty();
                start.Text = "Chơi lại";
                InitializeWord();
                SetHangmanState();
                foreach (var button in keyboard)
                    button.Enabled = true;
                MessageBox.Show("Trò chơi bắt đầu! Tìm từ có " + level + " chữ cái!");
            };

            this.Controls.Add(start);
        }
    }
}
