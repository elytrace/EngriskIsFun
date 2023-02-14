using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EngriskIsFun
{
    class UtilityTools
    {
        public static Bitmap ScaleImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void RewriteLineInFile(string filePath, string oldLine, string newLine)
        {
            string[] arrLine = File.ReadAllLines(filePath);

            for (int i = 0; i < arrLine.Length; i++)
            {
                if (arrLine[i].StartsWith(oldLine))
                {
                    arrLine[i] = newLine;
                    File.WriteAllLines(filePath, arrLine);
                    return;
                }
            }
        }

        public static void DoGetRequest(string url, Action<string> onComplete)
        {
            //try
            //{
                var requestWordList = WebRequest.Create(url);
                requestWordList.Method = "GET";

                var webResponse = requestWordList.GetResponse();
                var webStream = webResponse.GetResponseStream();

                var reader = new StreamReader(webStream);
                var data = reader.ReadToEnd();

                onComplete.Invoke(data);
            //}
            //catch(Exception)
            //{
            //    return;
            //}
        }

        public static Word ParseWord(JObject jObject)
        {
            if (!jObject.ContainsKey("word")) return null;
            Word word = new Word();

            word.Text = jObject["word"].ToString();

            return word;
        }

        private static  dbEngriskIsFunDataContext db = new dbEngriskIsFunDataContext();
        public static void SaveDefinitions(JObject jObject)
        {
            if (!jObject.ContainsKey("word")) return;
            //List<Definition> definitions = new  List<Definition>();

            var meanings = JsonConvert.DeserializeObject<List<JObject>>(jObject["meanings"].ToString());
            foreach(var block in meanings)
            {
                Definition d = new Definition();
                d.PartOfSpeech = block["partOfSpeech"].ToString();
                var definitions = JsonConvert.DeserializeObject<List<JObject>>(block["definitions"].ToString());
                foreach(var variant in definitions)
                {
                    d.Text = variant["definition"].ToString();
                    d.Example = variant.ContainsKey("example") ? variant["example"].ToString() : null;
                    d.WordID = db.Words.Where(a => a.Text == jObject["word"].ToString()).Select(a => a.WordID).Take(1).ToList()[0];
                    db.Definitions.InsertOnSubmit(d);

                    // add synonyms and antonyms later
                }
            }
            db.SubmitChanges();
        }
    }
}
