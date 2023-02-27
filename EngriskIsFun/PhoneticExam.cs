using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
    public partial class PhoneticExam : UserControl
    {
        private static PhoneticExam _instance;
        public static PhoneticExam Instance
        {
            get
            {
                if (_instance == null) _instance = new PhoneticExam();
                return _instance;
            }
        }
        public UserControl parent { get; set; }
        public PhoneticExam()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("Materials/Backgrounds/menu.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            DisplayBackBtn();
            DisplayLoading();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            loading.Show();
            for (int i = 0; i < questionList.Length; i++)
            {
                RetrieveQuestions(i);
                bw.ReportProgress(i + 1);
            }
        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loading.Hide();
            progressBar.Hide();
            DisplayTest();
        }

        BackgroundWorker bw = new BackgroundWorker();
        Button back = new Button();
        ProgressBar progressBar = new ProgressBar();
        Label loading = new Label();

        private void DisplayLoading()
        {
            progressBar.Size = new Size(200, 30);
            progressBar.Location = new Point(this.Width / 2 - progressBar.Width / 2, this.Height / 2 - progressBar.Height / 2);
            progressBar.Maximum = 10;
            progressBar.Value = 0;
            this.Controls.Add(progressBar);

            loading.Text = "Đang khởi tạo...";
            loading.Font = new Font("Arial", 12, FontStyle.Regular);
            loading.Size = new Size(200, 50);
            loading.BackColor = Color.Transparent;
            loading.TextAlign = ContentAlignment.MiddleCenter;
            loading.Location = new Point(this.Width / 2 - loading.Width / 2, this.Height / 2 - loading.Height / 2 + 35);
            this.Controls.Add(loading);
        }
        private void DisplayBackBtn()
        {
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
        private dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();
        private Random rand = new Random();

        private void RetrieveQuestions(int level)
        {
            List<string> wordList = db.WordsLessThan7s.Select(a => a.Text).ToList();
            //if (level == 0 || level == 1) wordList = db.WordsLessThan7s.Select(a => a.Text).ToList();
            //else if (level == 2 || level == 3) wordList = db.WordsLessThan8s.Select(a => a.Text).ToList();
            //else if (level == 4 || level == 5) wordList = db.WordsLessThan9s.Select(a => a.Text).ToList();
            //else if (level == 6) wordList = db.WordsLessThan10s.Select(a => a.Text).ToList();
            //else if (level == 7) wordList = db.WordsLessThan11s.Select(a => a.Text).ToList();
            //else if (level == 8) wordList = db.WordsLessThan13s.Select(a => a.Text).ToList();
            //else wordList = db.WordsMoreThan13s.Select(a => a.Text).ToList();
            var word = wordList[rand.Next(wordList.Count)];
            List<string> audioList = new List<string>();
            while (audioList.Count() == 0)
            {
                word = wordList[rand.Next(wordList.Count)];
                UtilityTools.DoGetRequest(Constants.WORD_DEFINITION_URL + word, result =>
                {
                    var jObject = JsonConvert.DeserializeObject<List<JObject>>(result)[0];
                    var phoneticObject = JsonConvert.DeserializeObject<List<JObject>>(jObject["phonetics"].ToString());
                    foreach(var item in phoneticObject)
                    {
                        if (item.ContainsKey("audio") && item["audio"].ToString() != "") audioList.Add(item["audio"].ToString());
                    }
                }, null);
            }
            var answerList = new string[4];

            for (int i = 0; i < answerList.Length; i++)
            {
                answerList[i] = wordList[rand.Next(wordList.Count)];
                while (answerList[i] == word)
                    answerList[i] = wordList[rand.Next(wordList.Count)];
            }
            rightAnswer[level] = rand.Next(answerList.Length);
            answerList[rightAnswer[level]] = word;
            questionList[level] = new Question()
            {
                audioURL = audioList[0],
                answer = answerList
            };
        }

        class Question
        {
            public string audioURL { get; set; }
            public string[] answer = new string[4];
        }
        private Question[] questionList = new Question[10];
        private int currentQuestion = 0;
        private int[] rightAnswer = new int[10];

        Label questionIndex = new Label();
        Panel questionPanel = new Panel();
        PictureBox question = new PictureBox();
        Button[] answers = new Button[4];
        string url = "";
        private void DisplayTest()
        {
            currentQuestion = 0;
            score = 0;
            questionPanel.Location = new Point(124, 50);
            questionPanel.Size = new Size(600, 125);

            questionIndex.Text = "Câu " + (currentQuestion + 1).ToString() + " / 10";
            questionIndex.Font = new Font("Arial", 16, FontStyle.Regular);
            questionIndex.AutoSize = false;
            questionIndex.TextAlign = ContentAlignment.MiddleCenter;
            questionIndex.Dock = DockStyle.Top;
            questionIndex.ForeColor = Color.Chocolate;
            questionIndex.Margin = new Padding(10, 20, 10, 20);
            questionIndex.Location = new Point(questionPanel.Width / 2 - questionIndex.Width / 2, questionPanel.Height / 2 - questionIndex.Height / 2);
            questionPanel.Controls.Add(questionIndex);

            question.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/audio.png"), 45, 45);
            question.Size = new Size(50, 50);
            question.SizeMode = PictureBoxSizeMode.CenterImage;
            url = questionList[currentQuestion].audioURL;
            question.Click += (sender, args) =>
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
            question.Location = new Point(questionPanel.Width / 2 - question.Width / 2, questionPanel.Height / 2 - question.Height / 2);
            questionPanel.Controls.Add(question);

            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = new Button();
                answers[i].Size = new Size(250, 100);
                answers[i].Location = new Point(124 + (i < 2 ? 0 : 1) * 350, 200 + (i % 2 == 0 ? 0 : 1) * 125);
                answers[i].Text = questionList[currentQuestion].answer[i];
                answers[i].Font = new Font("Arial", 16, FontStyle.Regular);
                answers[i].MouseEnter += (sender, args) =>
                {
                    Sound.Play(Sound.MOUSE_ENTER);
                    Button btn = (Button)sender;
                    btn.Font = new Font("Arial", 26);
                };
                answers[i].MouseLeave += (sender, args) =>
                {
                    Button btn = (Button)sender;
                    btn.Font = new Font("Arial", 16);
                };
                answers[i].Click += (sender, args) =>
                {
                    SetButtonUnclickable();
                    Button btn = (Button)sender;
                    if (answers[rightAnswer[currentQuestion]].Text == btn.Text)
                    {
                        Sound.Play(Sound.CORRECT);
                        btn.BackColor = Color.LawnGreen;
                        Task.Run(() =>
                        {
                            Task.Delay(1500).Wait();
                            btn.BackColor = Color.White;
                            score++;
                        });
                    }
                    else
                    {
                        Sound.Play(Sound.INCORRECT);
                        answers[rightAnswer[currentQuestion]].BackColor = Color.LawnGreen;
                        btn.BackColor = Color.Coral;
                        Task.Run(() =>
                        {
                            Task.Delay(1500).Wait();
                            answers[rightAnswer[currentQuestion]].BackColor = Color.White;
                            btn.BackColor = Color.White;
                        });
                    }
                    Task.Run(() =>
                    {
                        Task.Delay(1500).Wait();
                        MoveToNextQuestion();
                    });
                };
                this.Controls.Add(answers[i]);
            }

            this.Controls.Add(questionPanel);
        }

        private void SetButtonUnclickable()
        {
            foreach (var btn in answers)
            {
                btn.Enabled = false;
            }
        }
        private void SetButtonClickable()
        {
            foreach (var btn in answers)
            {
                btn.Invoke((Action)delegate
                {
                    btn.Enabled = true;
                });
            }
        }

        private int score = 0;

        private void MoveToNextQuestion()
        {
            SetButtonClickable();
            currentQuestion++;

            if (currentQuestion == 10)
            {
                this.Invoke(new Action(() =>
                {
                    Popup popup = new Popup(Popup.END_TEST, "", score, () =>
                    {
                        this.Controls.Clear();
                        DisplayBackBtn();
                        DisplayTest();
                    }, () =>
                    {
                        back.PerformClick();
                    });
                    if (!popup.IsDisposed) popup.ShowDialog(this);
                }));
            }
            else
            {
                questionIndex.Invoke((Action)delegate
                {
                    questionIndex.Text = "Câu " + (currentQuestion + 1).ToString() + " / 10";
                });

                question.Invoke((Action)delegate
                {
                    url = questionList[currentQuestion].audioURL;
                    for (int i = 0; i < 4; i++)
                    {
                        answers[i].Text = questionList[currentQuestion].answer[i];
                    }
                });
            }
        }
    }
}
