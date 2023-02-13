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
            InitializeUI();

            this.BackgroundImage = Image.FromFile("Materials/background.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
        }

        private static TextBox input = new CustomeTextBox("Điền từ ít hơn 7 chữ thôi nhé");
        private static Button search = new Button();

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
                if (!Constants.downloadedWordList) {
                    if (MessageBox.Show("Bạn muốn tải từ điển về máy?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bw.DoWork += GetDictionary;
                        bw.ProgressChanged += SendProgress;
                        bw.RunWorkerCompleted += SaveWordList;
                        if (!bw.IsBusy)
                        {
                            bw.RunWorkerAsync();
                        }
                        //MessageBox.Show("Từ điển sẽ mất một thời gian để tải xuống,\n vui lòng đợi trong giây lát.");
                        MessageBox.Show(Directory.GetCurrentDirectory());
                        
                    }
                    else
                    {
                        // user clicked no
                    }
                }
                else
                {
                    MessageBox.Show("Từ điển đã và đang tải rồi, đừng ấn nữa...");
                }
            };

            Controls.Add(input);
            Controls.Add(search);
            Controls.Add(downloadWordList);

        }

        private static BackgroundWorker bw = new BackgroundWorker();

        private static void GetDictionary(object sender, DoWorkEventArgs e)
        {
            var request = WebRequest.Create(Constants.WORD_LIST_URL);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            var list = JsonConvert.DeserializeObject<List<string>>(data);

            dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();
            foreach (var item in list)
            {
                if (item.Length <= 6)
                {
                    Word word = new Word();
                    word.Word1 = item;

                    var checker = db.Words.Where(a => a.Word1 == item).Select(a => a.Word1).ToList();
                    if (checker.Count == 0)
                    {
                        db.Words.InsertOnSubmit(word);
                        db.SubmitChanges();
                    }
                }
            }

            e.Result = "Đã tải về từ điển!";
        }

        private void SendProgress(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void SaveWordList(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Result.ToString());
                
        }

    }
}
