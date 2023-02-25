using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class Function1 : Form
    {
        public Function1()
        {
            InitializeComponent();
            RetrieveConfig();
            InitializeUI();
            this.Text = "Từ điển";
            this.BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        public Function1(string word)
        {
            InitializeComponent();
            RetrieveConfig();
            InitializeUI();

            this.BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            input.Text = word;
            search.PerformClick();
        }

        private TextBox input = new CustomeTextBox("Nhập từ cần tìm kiếm");
        private Button search = new Button();
        private ListBox suggestions = new ListBox();
        private Panel result = new Panel();
        private Label loading = new Label();

        private bool downloaded;

        private void RetrieveConfig()
        {
            downloaded = bool.Parse(File.ReadAllLines("Configs/config.txt")[0]);
        }

        private void InitializeUI()
        {
            Control control = new Control();
            control.BackColor = Color.White;
            control.Size = new Size(200, 30);
            control.Location = new Point(50, 55);

            input.Width = control.Width;
            input.AutoSize = true;
            input.Left = 10;
            input.Top = (control.Height / 2 - input.Height / 2);
            input.Font = new Font("Arial", 10, FontStyle.Regular);
            input.TextAlign = HorizontalAlignment.Left;
            input.KeyDown += Input_KeyDown;
            input.BorderStyle = BorderStyle.None;
            if (downloaded)
                input.TextChanged += OnInputChanged;
            control.Controls.Add(input);

            search.Location = new Point(280, 53);
            search.Size = new Size(90, 34);
            search.Text = "Xác nhận";
            search.Click += Search;

            Button download = new Button();
            download.Location = new Point(718, 10);
            download.Size = new Size(120, 34);
            download.Text = "Tải từ điển";
            download.Focus();
            download.Click += DownloadDictionary;

            suggestions.Location = new Point(50, 85);
            suggestions.Width = input.Width;
            suggestions.MaximumSize = new Size(suggestions.Width, 200);
            suggestions.Font = new Font("Arial", 10, FontStyle.Regular);
            suggestions.Hide();
            suggestions.SelectedValueChanged += (sender, args) =>
            {
                input.Text = suggestions.SelectedItem.ToString();
            };

            result.Location = new Point(50, 120);
            result.Size = new Size(320, 250);
            result.BackColor = Color.White;
            result.AutoScroll = false;
            result.HorizontalScroll.Enabled = false;
            result.HorizontalScroll.Visible = false;
            result.HorizontalScroll.Maximum = 0;
            result.AutoScroll = true;
            

            loading.Size = new Size(100, 50);
            loading.Location = new Point(result.Width / 2 - loading.Width / 2 + result.Location.X, result.Height / 2 - loading.Height / 2 + result.Location.Y);
            loading.TextAlign = ContentAlignment.MiddleCenter;
            loading.Text = "LOADING...";
            loading.BackColor = Color.White;
            loading.ForeColor = Color.Black;
            loading.Font = new Font("Arial", 10, FontStyle.Regular);
            loading.Hide();
            
            Controls.Add(loading);
            Controls.Add(suggestions);
            Controls.Add(download);
            Controls.Add(control);
            Controls.Add(search);
            Controls.Add(result);

        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                input.Text = input.Text.ToString();
                search.PerformClick();
            }
        }

        private void OnInputChanged(object sender, EventArgs e)
        {
            suggestions.Items.Clear();
            if (input.Text.Length < 4)
            {
                suggestions.Hide();
            }
            else
            {
                var suggestedWords = db.WordsLessThan7s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsLessThan8s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsLessThan9s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsLessThan10s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsLessThan11s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsLessThan13s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                suggestedWords = db.WordsMoreThan13s.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords) suggestions.Items.Add(word);
                if (suggestions.Items.Count > 0) suggestions.Show();
            }
            suggestions.Height = suggestions.ItemHeight * (suggestions.Items.Count + 1);
        }

        private void Search(object sender, EventArgs e)
        {
            suggestions.Hide();
            loading.Show();
            if(downloaded) input.TextChanged -= OnInputChanged;

            BackgroundWorker retrieveDictionary = new BackgroundWorker();
            retrieveDictionary.DoWork += (s, args) =>
            {
                UtilityTools.DoGetRequest(Constants.WORD_DEFINITION_URL + input.Text.ToString(), json =>
                {
                    if(json != null)
                    {
                        args.Result = json;
                    }
                }, () =>
                {
                    MessageBox.Show("Không tìm được định nghĩa!");
                });
            };
            retrieveDictionary.RunWorkerCompleted += (s, args) =>
            {
                loading.Hide();
                if (downloaded) input.TextChanged += OnInputChanged;

                if (args.Result == null) return;
                JObject jObject = JsonConvert.DeserializeObject<List<JObject>>(args.Result.ToString())[0];

                if (!jObject.ContainsKey("word")) return;

                WordObject word = new WordObject(input.Text);
                if (downloaded)
                {
                    SaveDefinitions(word);
                    SavePhonetics(word);
                }

                DisplayResult(word);
            };

            retrieveDictionary.RunWorkerAsync();
            
        }

        private void DisplayResult(WordObject wordObject)
        {
            result.Controls.Clear();

            Label word = new Label();
            word.Location = new Point(5, 5);
            word.AutoSize = true;
            word.Text = input.Text;
            word.ForeColor = Color.Navy;
            word.Font = new Font("Arial", 14, FontStyle.Regular);

            DisplayPhonetics(wordObject);
            DisplayDefinitions(wordObject);

            result.Controls.Add(word);
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

                result.Controls.Add(phonetics[i]);
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

                result.Controls.Add(audioIcons[i]);
            }

            breakLine.Location = new Point(10, 25 + phoneticList.Count * 25 + 10);
            breakLine.Size = new Size(300, 2);
            breakLine.BackColor = Color.Black;
            result.Controls.Add(breakLine);
        }

        private Panel breakLine = new Panel();

        private void DisplayDefinitions(WordObject wordObject)
        {
            var definitionList = wordObject.definitions;

            definitionList.Sort((a, b) => String.Compare(a.partOfSpeech, b.partOfSpeech, StringComparison.InvariantCulture));

            int index = 1;
            for(int i = 0; i < definitionList.Count; i++)
            {
                int lastItemY = breakLine.Size.Height + breakLine.Location.Y;
                if (i != 0)
                {
                    lastItemY = result.Controls[result.Controls.Count - 1].Height + result.Controls[result.Controls.Count - 1].Location.Y;
                }

                if(i == 0)
                {
                    Label partOfSpeech = new Label();
                    partOfSpeech.AutoSize = true;
                    partOfSpeech.Font = new Font("Arial", 12, FontStyle.Italic);
                    partOfSpeech.ForeColor = Color.Red;
                    partOfSpeech.Text = definitionList[i].partOfSpeech;
                    partOfSpeech.Location = new Point(10, lastItemY + 10);

                    result.Controls.Add(partOfSpeech);
                    lastItemY = result.Controls[result.Controls.Count - 1].Height + result.Controls[result.Controls.Count - 1].Location.Y;
                }
                else if (definitionList[i-1].partOfSpeech != definitionList[i].partOfSpeech)
                {
                    index = 1;
                    Label partOfSpeech = new Label();
                    partOfSpeech.AutoSize = true;
                    partOfSpeech.Font = new Font("Arial", 12, FontStyle.Italic);
                    partOfSpeech.ForeColor = Color.Red;
                    partOfSpeech.Text = definitionList[i].partOfSpeech;
                    partOfSpeech.Location = new Point(10, lastItemY + 10);

                    result.Controls.Add(partOfSpeech);
                    lastItemY = result.Controls[result.Controls.Count - 1].Height + result.Controls[result.Controls.Count - 1].Location.Y;
                }

                Label definition = new Label();
                definition.MaximumSize = new Size(result.Width - 20, 0);
                definition.AutoSize = true;
                definition.Font = new Font("Arial", 10, FontStyle.Regular);
                definition.Text = index.ToString() + ". " + definitionList[i].text;
                definition.Location = new Point(10, lastItemY + 10);
                result.Controls.Add(definition);
                lastItemY = result.Controls[result.Controls.Count - 1].Height + result.Controls[result.Controls.Count - 1].Location.Y;

                Label example = new Label();
                example.MaximumSize = new Size(result.Width - 20, 0);
                example.AutoSize = true;
                example.Font = new Font("Arial", 10, FontStyle.Regular);
                if(definitionList[i].example != null) 
                    example.Text = "Example: " + definitionList[i].example;
                example.Location = new Point(10, lastItemY + 10);
                example.ForeColor = Color.Green;
                if (definitionList[i].example != null)
                    result.Controls.Add(example);

                index++;
            }
        }

        private void DownloadDictionary(object sender, EventArgs e)
        {
            if (!downloaded)
            {
                if (MessageBox.Show("Bạn muốn tải từ điển về máy?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bw.DoWork += Bw_DoWork;
                    bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                    bw.ProgressChanged += Bw_ProgressChanged;
                    bw.WorkerReportsProgress = true;

                    if (!bw.IsBusy)
                    {
                        bw.RunWorkerAsync();
                    }

                    if (!form.Disposing) form.Show();
                }
                else
                {
                    // user clicked no
                }
            }
            else
                MessageBox.Show("Từ điển đã tải về rồi, đừng ấn nữa.");
        }

        private BackgroundWorker bw = new BackgroundWorker();
        private LoadingForm form = new LoadingForm();
        private dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> list = new List<string>();
            UtilityTools.DoGetRequest(Constants.WORD_LIST_URL, data =>
            {
                list = JsonConvert.DeserializeObject<List<string>>(data);
                form.max = list.Count;
            }, null);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length < 7)
                {
                    var word = new WordsLessThan7 { Text = list[i] };

                    var checker = db.WordsLessThan7s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan7s.InsertOnSubmit(word);
                        
                    }
                }
                else if(list[i].Length < 8)
                {
                    var word = new WordsLessThan8 { Text = list[i] };
                    var checker = db.WordsLessThan8s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan8s.InsertOnSubmit(word);
                    }
                }
                else if (list[i].Length < 9)
                {
                    var word = new WordsLessThan9 { Text = list[i] };
                    var checker = db.WordsLessThan9s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan9s.InsertOnSubmit(word);
                    }
                }
                else if (list[i].Length < 10)
                {
                    var word = new WordsLessThan10 { Text = list[i] };
                    var checker = db.WordsLessThan10s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan10s.InsertOnSubmit(word);
                    }
                }
                else if (list[i].Length < 11)
                {
                    var word = new WordsLessThan11 { Text = list[i] };
                    var checker = db.WordsLessThan11s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan11s.InsertOnSubmit(word);
                    }
                }
                else if (list[i].Length < 13)
                {
                    var word = new WordsLessThan13 { Text = list[i] };
                    var checker = db.WordsLessThan13s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsLessThan13s.InsertOnSubmit(word);
                    }
                }
                else
                {
                    var word = new WordsMoreThan13 { Text = list[i] };
                    var checker = db.WordsMoreThan13s.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.WordsMoreThan13s.InsertOnSubmit(word);
                    }
                }
                bw.ReportProgress(i);
            }
            db.SubmitChanges();

            e.Result = "Đã tải về từ điển!";
        }
        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            form.current = e.ProgressPercentage;
            form.increment();
        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form.displayMessage(e.Result.ToString());
            downloaded = true;
            File.WriteAllText("Configs/config.txt", "true");
            input.TextChanged += OnInputChanged;
        }

        private void SaveDefinitions(WordObject wordObject)
        {
            long wordId;
            int length = wordObject.word.Length;
            if(length < 7) wordId = db.WordsLessThan7s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if(length < 8) wordId = db.WordsLessThan8s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if(length < 9) wordId = db.WordsLessThan9s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if(length < 10) wordId = db.WordsLessThan10s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if(length < 11) wordId = db.WordsLessThan11s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if(length < 13) wordId = db.WordsLessThan13s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else wordId = db.WordsMoreThan13s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            var wordList = db.Definitions.Where(a => a.WordID == wordId).Select(a => a.Text).ToList();
            if (wordList.Count > 0) return;

            foreach (var item in wordObject.definitions)
            {
                Definition d = new Definition
                {
                    PartOfSpeech = item.partOfSpeech,
                    WordID = wordId,
                    Text = item.text,
                    Example = item.example
                };
                db.Definitions.InsertOnSubmit(d);
                db.SubmitChanges();
            }
        }
        private void SavePhonetics(WordObject wordObject)
        {
            long wordId;
            int length = wordObject.word.Length;
            if (length < 7) wordId = db.WordsLessThan7s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if (length < 8) wordId = db.WordsLessThan8s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if (length < 9) wordId = db.WordsLessThan9s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if (length < 10) wordId = db.WordsLessThan10s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if (length < 11) wordId = db.WordsLessThan11s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else if (length < 13) wordId = db.WordsLessThan13s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            else wordId = db.WordsMoreThan13s.Where(a => a.Text == wordObject.word).Select(a => a.WordID).ToList()[0];
            var wordList = db.Phonetics.Where(a => a.WordID == wordId).Select(a => a.Text).ToList();
            if (wordList.Count > 0) return;


            foreach (var item in wordObject.phonetics)
            {
                Phonetic p = new Phonetic
                {
                    WordID = wordId,
                    Audio = item.audio,
                    Text = item.text
                };
                db.Phonetics.InsertOnSubmit(p);
            }
            db.SubmitChanges();
        }
    }
}
