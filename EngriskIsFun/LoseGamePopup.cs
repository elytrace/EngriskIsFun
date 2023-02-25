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
    public partial class LoseGamePopup : Form
    {
        public LoseGamePopup(string word)
        {
            InitializeComponent();
            this.Text = "Định nghĩa";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(320, 250);
            this.BackColor = Color.White;

            this.AutoScroll = false;
            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Maximum = 0;
            this.AutoScroll = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            DisplayResult(word);
        }

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

        private Panel breakLine = new Panel();

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
                    UtilityTools.PlayMp3FromUrl(url, () =>
                    {
                        pictureBox.BackColor = Color.White;
                    });
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
    }
}
