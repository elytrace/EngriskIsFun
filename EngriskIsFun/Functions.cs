using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EngriskIsFun
{
    public class Functions
    {
        //public static Panel panel = new Panel();
        //private static System.Timers.Timer timer = new System.Timers.Timer();

        //public static void StartTransition()
        //{
        //    panel.BackgroundImage = Image.FromFile("Materials/background.png");
        //    panel.Size = new Size(848, 7);
        //    timer.Interval = 10;
        //    timer.Elapsed += TransitionAnimation;
        //    timer.Start();
        //}

        //private static void TransitionAnimation(object sender, ElapsedEventArgs e)
        //{
        //    panel.Invoke((Action)delegate
        //    {
        //        panel.Size = new Size(panel.Size.Width, panel.Size.Height + 7);
        //        if (panel.Size.Height >= 441)
        //        {
        //            timer.Stop();
        //            ShowComponents();
        //        }
        //    });
        //}

        //private static TextBox input = new TextBox();
        //private static Button search = new Button();
        //private static TextBox output = new TextBox();

        //public static void InitializeComponents()
        //{
        //    input.Location = new Point(50, 55);
        //    input.Size = new Size(200, 30);
        //    input.Multiline = true;
        //    input.Font = new Font("Arial", 15, FontStyle.Regular);
        //    input.TextAlign = HorizontalAlignment.Left;

        //    search.Location = new Point(280, 53);
        //    search.Size = new Size(90, 34);
        //    search.Text = "SEARCH";

        //    output.Location = new Point(50, 100);
        //    output.Multiline = true;
        //    output.Size = new Size(400, 300);
        //    input.Font = new Font("Arial", 15, FontStyle.Regular);
        //    output.WordWrap = true;
            
        //    panel.Controls.Add(input);
        //    panel.Controls.Add(search);
        //    panel.Controls.Add(output);

            // *********** IMPROVE LATER *********** //
            //bw.DoWork += GetDictionary;
            //bw.RunWorkerCompleted += SaveWordList;
            //if (!bw.IsBusy && !savedWordList)
            //{
            //    bw.RunWorkerAsync();
            //}
        //}

        #region GetAlLWords

        //private static BackgroundWorker bw = new BackgroundWorker();
        //private static bool savedWordList = false;

        //private static void GetDictionary(object sender, DoWorkEventArgs e)
        //{
        //    var request = WebRequest.Create(Constants.WORD_LIST_URL);
        //    request.Method = "GET";

        //    var webResponse = request.GetResponse();
        //    var webStream = webResponse.GetResponseStream();

        //    var reader = new StreamReader(webStream);
        //    var data = reader.ReadToEnd();

        //    var list = JsonConvert.DeserializeObject<List<string>>(data);

        //    dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();
        //    foreach (var item in list)
        //    {
        //        Word word = new Word();
        //        word.Word1 = item;

        //        var checker = db.Words.Where(a => a.Word1 == item).Select(a => a.Word1).ToList();
        //        if (checker.Count == 0)
        //        {
        //            db.Words.InsertOnSubmit(word);
        //            db.SubmitChanges();
        //        }
        //    }

        //    e.Result = "Đã lưu từ điển\n vào database!";
        //}

        //private static void SaveWordList(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    MessageBox.Show(e.Result.ToString());
        //    savedWordList = true;
        //}

        #endregion
    }
}
