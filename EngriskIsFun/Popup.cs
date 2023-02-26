﻿using System;
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
    public partial class Popup : Form
    {
        public static int LOSE_GAME = 0;
        public static int WIN_GAME = 1;
        public static int START_GAME = 2;

        public static int END_TEST = 3;
        public Popup(int popupType, string word = "", int score = 0, Action onRestart = null, Action onExit = null)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            this.AutoScroll = false;
            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Maximum = 0;
            this.AutoScroll = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (popupType == LOSE_GAME)
            {
                this.Text = "Định nghĩa";
                this.Size = new Size(320, 250);
                DisplayResult(word);
            }
            if(popupType == END_TEST)
            {
                this.Text = "Kết quả";
                this.Size = new Size(300, 115);
                this.ControlBox = false;
                DisplayScore(score, onRestart, onExit);

            }
        }

        private Panel breakLine = new Panel();
        private void DisplayResult(string input)
        {
            Label word = new Label();
            word.Location = new Point(5, 5);
            word.AutoSize = true;
            word.Text = input;
            word.ForeColor = Color.Navy;
            word.Font = new Font("Arial", 14, FontStyle.Regular);

            WordObject wordObject = new WordObject(input);

            DisplayPhonetics(wordObject);
            DisplayDefinitions(wordObject);

            this.Controls.Add(word);
        }
        private void DisplayPhonetics(WordObject wordObject)
        {
            var phoneticList = wordObject.phonetics;

            Label[] phonetics = new Label[phoneticList.Count];
            PictureBox[] audioIcons = new PictureBox[phoneticList.Count];

            for (int i = 0; i < phonetics.Length; i++)
            {
                phonetics[i] = new Label();
                phonetics[i].Location = new Point(35, 35 + i * 25);
                phonetics[i].AutoSize = true;
                phonetics[i].Text = phoneticList[i].text;
                phonetics[i].Font = new Font("Arial", 10, FontStyle.Regular);

                this.Controls.Add(phonetics[i]);
            }

            for (int i = 0; i < audioIcons.Length; i++)
            {
                if (phoneticList[i].audio == "") continue;

                audioIcons[i] = new PictureBox();
                audioIcons[i].Image = Image.FromFile("Materials/audio.png");
                audioIcons[i].Location = new Point(5, 30 + i * 25);
                audioIcons[i].Size = new Size(25, 25);
                audioIcons[i].SizeMode = PictureBoxSizeMode.CenterImage;
                string url = phoneticList[i].audio;
                audioIcons[i].Click += (sender, args) =>
                {
                    PictureBox pictureBox = (PictureBox)sender;
                    pictureBox.BackColor = Color.Gray;
                    Task.Run(() =>
                    {
                        Task.Delay(300).Wait();
                        pictureBox.Invoke((Action)delegate
                        {
                            pictureBox.BackColor = Color.Transparent;
                        });
                    });
                    UtilityTools.PlayMp3FromUrl(url);
                };

                this.Controls.Add(audioIcons[i]);
            }

            breakLine.Location = new Point(10, 25 + phoneticList.Count * 25 + 10);
            breakLine.Size = new Size(300, 2);
            breakLine.BackColor = Color.Black;
            this.Controls.Add(breakLine);
        }
        private void DisplayDefinitions(WordObject wordObject)
        {
            var definitionList = wordObject.definitions;

            definitionList.Sort((a, b) => String.Compare(a.partOfSpeech, b.partOfSpeech, StringComparison.InvariantCulture));

            int index = 1;
            for (int i = 0; i < definitionList.Count; i++)
            {
                int lastItemY = breakLine.Size.Height + breakLine.Location.Y;
                if (i != 0)
                {
                    lastItemY = this.Controls[this.Controls.Count - 1].Height + this.Controls[this.Controls.Count - 1].Location.Y;
                }

                if (i == 0)
                {
                    Label partOfSpeech = new Label();
                    partOfSpeech.AutoSize = true;
                    partOfSpeech.Font = new Font("Arial", 12, FontStyle.Italic);
                    partOfSpeech.ForeColor = Color.Red;
                    partOfSpeech.Text = definitionList[i].partOfSpeech;
                    partOfSpeech.Location = new Point(10, lastItemY + 10);

                    this.Controls.Add(partOfSpeech);
                    lastItemY = this.Controls[this.Controls.Count - 1].Height + this.Controls[this.Controls.Count - 1].Location.Y;
                }
                else if (definitionList[i - 1].partOfSpeech != definitionList[i].partOfSpeech)
                {
                    index = 1;
                    Label partOfSpeech = new Label();
                    partOfSpeech.AutoSize = true;
                    partOfSpeech.Font = new Font("Arial", 12, FontStyle.Italic);
                    partOfSpeech.ForeColor = Color.Red;
                    partOfSpeech.Text = definitionList[i].partOfSpeech;
                    partOfSpeech.Location = new Point(10, lastItemY + 10);

                    this.Controls.Add(partOfSpeech);
                    lastItemY = this.Controls[this.Controls.Count - 1].Height + this.Controls[this.Controls.Count - 1].Location.Y;
                }

                Label definition = new Label();
                definition.MaximumSize = new Size(this.Width - 20, 0);
                definition.AutoSize = true;
                definition.Font = new Font("Arial", 10, FontStyle.Regular);
                definition.Text = index.ToString() + ". " + definitionList[i].text;
                definition.Location = new Point(10, lastItemY + 10);
                this.Controls.Add(definition);
                lastItemY = this.Controls[this.Controls.Count - 1].Height + this.Controls[this.Controls.Count - 1].Location.Y;

                Label example = new Label();
                example.MaximumSize = new Size(this.Width - 20, 0);
                example.AutoSize = true;
                example.Font = new Font("Arial", 10, FontStyle.Regular);
                if (definitionList[i].example != null)
                    example.Text = "Example: " + definitionList[i].example;
                example.Location = new Point(10, lastItemY + 10);
                example.ForeColor = Color.Green;
                if (definitionList[i].example != null)
                    this.Controls.Add(example);

                index++;
            }
        }

        private void DisplayScore(int score, Action onRestart, Action onExit)
        {
            Label result = new Label();
            result.Text = "Bạn làm đúng " + score.ToString() + "/10 câu!";
            result.Font = new Font("Arial", 16, FontStyle.Regular);
            result.AutoSize = false;
            result.Margin = new Padding(10, 10, 10, 10);
            result.TextAlign = ContentAlignment.TopCenter;
            result.Dock = DockStyle.Fill;

            Button restart = new Button();
            restart.Text = "Chơi lại";
            restart.Location = new Point(25, 30);
            restart.Size = new Size(100, 34);
            restart.Click += (sender, args) =>
            {
                this.Close();
                onRestart.Invoke();
            };
            this.Controls.Add(restart);

            Button exit = new Button();
            exit.Text = "Thoát";
            exit.Location = new Point(150, 30);
            exit.Size = new Size(100, 34);
            exit.Click += (sender, args) =>
            {
                this.Close(); 
                onExit.Invoke();
            };
            this.Controls.Add(exit);
            

            this.Controls.Add(result);
        }
    }
}
