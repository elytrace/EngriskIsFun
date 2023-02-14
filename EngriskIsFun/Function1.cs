using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            this.BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);

        }

        private TextBox input = new CustomeTextBox("Điền từ ít hơn 7 chữ thôi nhé");
        private Button search = new Button();
        private ListBox suggestions = new ListBox();
        //private Text

        private bool downloaded;

        private void RetrieveConfig()
        {
            downloaded = bool.Parse(File.ReadAllLines("Configs/config.txt")[0]);
        }

        private void InitializeUI()
        {
            input.Location = new Point(50, 55);
            input.Size = new Size(200, 30);
            input.Multiline = true;
            input.Font = new Font("Arial", 10, FontStyle.Regular);
            input.TextAlign = HorizontalAlignment.Left;
            if (downloaded)
                input.TextChanged += OnInputChanged;

            search.Location = new Point(280, 53);
            search.Size = new Size(90, 34);
            search.Text = "SEARCH";
            search.Click += Search;

            Button downloadWordList = new Button();
            downloadWordList.Location = new Point(718, 10);
            downloadWordList.Size = new Size(120, 34);
            downloadWordList.Text = "Tải từ điển";
            downloadWordList.Focus();
            downloadWordList.Click += DownloadDictionary;

            suggestions.Location = new Point(50, 85);
            suggestions.Width = input.Width;
            suggestions.MaximumSize = new Size(suggestions.Width, 200);
            suggestions.Font = new Font("Arial", 10, FontStyle.Regular);
            suggestions.Hide();
            suggestions.SelectedValueChanged += (sender, args) =>
            {
                input.Text = suggestions.SelectedItem.ToString();
            };

            

            Controls.Add(input);
            Controls.Add(search);
            Controls.Add(suggestions);
            Controls.Add(downloadWordList);

        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
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
            UtilityTools.DoGetRequest(Constants.WORD_DEFINITION_URL + input.Text.ToString(), json =>
            {
                JObject jObject = JsonConvert.DeserializeObject<List<JObject>>(json)[0];

                // display to screen

                UtilityTools.SaveDefinitions(jObject);
            });
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
    }
}
