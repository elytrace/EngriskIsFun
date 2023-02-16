using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            //this.BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
        }

        private TextBox input = new CustomeTextBox("Điền từ ít hơn 7 chữ thôi nhé");
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
            search.Text = "SEARCH";
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

            Controls.Add(control);
            Controls.Add(search);
            Controls.Add(suggestions);
            Controls.Add(loading);
            Controls.Add(result);
            Controls.Add(download);

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
            if (input.Text.Length < 3)
            {
                suggestions.Hide();
            }
            else
            {
                var suggestedWords = db.Words.Where(a => a.Text.Contains(input.Text.ToString())).Select(a => a.Text).ToList();
                foreach (var word in suggestedWords)
                {
                    suggestions.Items.Add(word);
                }
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
                    args.Result = json;
                });
            };
            retrieveDictionary.RunWorkerCompleted += (s, args) =>
            {
                loading.Hide();
                if (downloaded) input.TextChanged += OnInputChanged;

                JObject jObject = JsonConvert.DeserializeObject<List<JObject>>(args.Result.ToString())[0];

                if (!jObject.ContainsKey("word")) return;

                SaveDefinitions(jObject);
                SavePhonetics(jObject);

                DisplayResult();
            };

            retrieveDictionary.RunWorkerAsync();
            
        }

        private void DisplayResult()
        {
            result.Controls.Clear();

            Label word = new Label();
            word.Location = new Point(5, 5);
            word.AutoSize = true;
            word.Text = input.Text;
            word.Font = new Font("Arial", 14, FontStyle.Regular);

            DisplayPhonetics();

            DisplayDefinitions();

            result.Controls.Add(word);
        }

        private void DisplayPhonetics()
        {
            var wordID = db.Words.Where(a => a.Text == input.Text.ToString()).Select(a => a.WordID).ToList();

            var phoneticQuery = from w in db.Words
                                join p in db.Phonetics
                                on w.WordID equals p.WordID
                                where w.WordID == wordID[0]
                                select new { p.Text, p.Audio };

            var phoneticList = phoneticQuery.ToList();

            Label[] phonetics = new Label[phoneticList.Count];
            PictureBox[] audioIcons = new PictureBox[phoneticList.Count];

            for (int i = 0; i < phonetics.Length; i++)
            {
                phonetics[i] = new Label();
                phonetics[i].Location = new Point(5, 30 + i * 25);
                phonetics[i].AutoSize = true;
                phonetics[i].Text = phoneticList[i].Text;
                phonetics[i].Font = new Font("Arial", 10, FontStyle.Regular);

                result.Controls.Add(phonetics[i]);
            }

            for (int i = 0; i < audioIcons.Length; i++)
            {
                if (phoneticList[i].Audio == "") continue;

                audioIcons[i] = new PictureBox();
                audioIcons[i].Image = Image.FromFile("Materials/audio.png");
                audioIcons[i].Location = new Point(result.Width / 2, 25 + i * 25);
                audioIcons[i].Size = new Size(25, 25);
                audioIcons[i].SizeMode = PictureBoxSizeMode.CenterImage;
                string url = phoneticList[i].Audio;
                audioIcons[i].Click += (sender, args) => UtilityTools.PlayMp3FromUrl(url);

                result.Controls.Add(audioIcons[i]);
            }

            breakLine.Location = new Point(10, 25 + phoneticList.Count * 25 + 10);
            breakLine.Size = new Size(300, 2);
            breakLine.BackColor = Color.Black;
            result.Controls.Add(breakLine);
        }

        private Panel breakLine = new Panel();

        private void DisplayDefinitions()
        {
            var wordID = db.Words.Where(a => a.Text == input.Text.ToString()).Select(a => a.WordID).ToList();

            var definitionQuery = from w in db.Words
                                join d in db.Definitions
                                on w.WordID equals d.WordID
                                where w.WordID == wordID[0]
                                select new { d.PartOfSpeech, d.Text };

            var definitionList = definitionQuery.ToList();

            var partOfSpeechList = db.Definitions.Select(a => a.PartOfSpeech).ToList();

            List<Label>[] definitions = new List<Label>[partOfSpeechList.Count];
            Dictionary<string, int> partOfSpeechMap = new Dictionary<string, int>();

            for(int i = 0; i < partOfSpeechList.Count; i++)
            {
                partOfSpeechMap[partOfSpeechList[i]] = i;
                definitions[i] = new List<Label>();
            }

            for(int i = 0; i < definitionList.Count; i++)
            {
                Label d = new Label();
                d.Location = new Point(5, breakLine.Height + i * 60);
                d.AutoSize = true;
                d.Text = definitionList[i].PartOfSpeech;
                d.Font = new Font("Arial", 12, FontStyle.Italic);

                definitions[partOfSpeechMap[definitionList[i].PartOfSpeech]].Add(d);
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
            });

            Word word = new Word();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length <= 6)
                {
                    word = new Word { Text = list[i] };

                    var checker = db.Words.Where(a => a.Text == list[i]).Select(a => a.Text).ToList();
                    if (checker.Count == 0)
                    {
                        db.Words.InsertOnSubmit(word);
                        db.SubmitChanges();
                    }
                    bw.ReportProgress(i);
                }
            }

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

        private void SaveDefinitions(JObject jObject)
        {
            var wordId = db.Words.Where(a => a.Text == jObject["word"].ToString()).Select(a => a.WordID).ToList()[0];
            var checker = db.Definitions.Where(a => a.WordID == wordId).Select(a => a.Text).ToList();
            if (checker.Count > 0) return;

            var meanings = JsonConvert.DeserializeObject<List<JObject>>(jObject["meanings"].ToString());
            foreach (var block in meanings)
            {
                var definitions = JsonConvert.DeserializeObject<List<JObject>>(block["definitions"].ToString());
                foreach (var def in definitions)
                {
                    Definition d = new Definition();
                    d.PartOfSpeech = block["partOfSpeech"].ToString();
                    d.WordID = wordId;
                    d.Text = def["definition"].ToString();
                    d.Example = def.ContainsKey("example") ? def["example"].ToString() : null;
                    db.Definitions.InsertOnSubmit(d);
                    db.SubmitChanges();

                    // ************** Save Synonyms ************** //
                    var synonyms = JsonConvert.DeserializeObject<List<string>>(def["synonyms"].ToString());
                    foreach (var syn in synonyms)
                    {
                        Synonym synonym = new Synonym();
                        synonym.DefinitionID = d.DefinitionID;
                        synonym.Text = syn;
                        db.Synonyms.InsertOnSubmit(synonym);
                        db.SubmitChanges();
                    }

                    // ************** Save Antonyms ************** //
                    var antonyms = JsonConvert.DeserializeObject<List<string>>(def["antonyms"].ToString());
                    foreach (var ant in antonyms)
                    {
                        Antonym antonym = new Antonym();
                        antonym.DefinitionID = d.DefinitionID;
                        antonym.Text = ant;
                        db.Antonyms.InsertOnSubmit(antonym);
                        db.SubmitChanges();
                    }
                }
            }
        }
        private void SavePhonetics(JObject jObject)
        {
            var wordId = db.Words.Where(a => a.Text == jObject["word"].ToString()).Select(a => a.WordID).ToList()[0];
            var checker = db.Phonetics.Where(a => a.WordID == wordId).Select(a => a.Text).ToList();
            if (checker.Count > 0) return;

            var phonetics = JsonConvert.DeserializeObject<List<JObject>>(jObject["phonetics"].ToString());

            foreach (var block in phonetics)
            {
                Phonetic p = new Phonetic();
                p.WordID = wordId;
                p.Audio = block.ContainsKey("audio") ? block["audio"].ToString() : null;
                p.Text = block.ContainsKey("text") ? block["text"].ToString() : null;
                db.Phonetics.InsertOnSubmit(p);
                db.SubmitChanges();
            }
        }
    }
}
