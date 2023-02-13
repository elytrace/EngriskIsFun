using Newtonsoft.Json;
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

        private static TextBox input = new CustomeTextBox("Điền từ ít hơn 7 chữ thôi nhé");
        private static Button search = new Button();
        private static ListBox result = new ListBox();

        private static bool downloaded;

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

            search.Location = new Point(280, 53);
            search.Size = new Size(90, 34);
            search.Text = "SEARCH";

            Button downloadWordList = new Button();
            downloadWordList.Location = new Point(718, 10);
            downloadWordList.Size = new Size(120, 34);
            downloadWordList.Text = "Tải từ điển";
            downloadWordList.Focus();
            downloadWordList.Click += (sender, args) =>
            {
                if (!downloaded) {
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
                {
                    MessageBox.Show("Từ điển đã tải về rồi, đừng ấn nữa.");
                }
            };

            result.Location = new Point(50, 85);
            result.Width = input.Width;
            result.MaximumSize = new Size(result.Width, 200);
            result.Font = new Font("Arial", 10, FontStyle.Regular);
            result.Hide();
            result.SelectedValueChanged += (sender, args) =>
            {
                input.Text = result.SelectedItem.ToString();
            };

            if(downloaded)
            {
                input.TextChanged += (sender, args) =>
                {
                    result.Items.Clear();
                    if(input.Text.Length < 3)
                    {
                        result.Hide();
                    }
                    else
                    {
                        var suggestedWords = db.Words.Where(a => a.Word1.Contains(input.Text.ToString())).Select(a => a.Word1).ToList();
                        foreach (var word in suggestedWords)
                        {
                            result.Items.Add(word);
                        }
                        if (result.Items.Count > 0) result.Show();
                    }
                    result.Height = result.ItemHeight * (result.Items.Count + 1);
                };
            }

            Controls.Add(input);
            Controls.Add(search);
            Controls.Add(result);
            Controls.Add(downloadWordList);

        }

        private static BackgroundWorker bw = new BackgroundWorker();
        private static LoadingForm form = new LoadingForm();
        private static dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();

        private static void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var request = WebRequest.Create(Constants.WORD_LIST_URL);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            var list = JsonConvert.DeserializeObject<List<string>>(data);
            form.max = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length <= 6)
                {
                    Word word = new Word();
                    word.Word1 = list[i];

                    var checker = db.Words.Where(a => a.Word1 == list[i]).Select(a => a.Word1).ToList();
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
        private static void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form.displayMessage(e.Result.ToString());
            downloaded = true;
            File.WriteAllText("Configs/config.txt", "true");
        }
    }
}
