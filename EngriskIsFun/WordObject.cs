using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngriskIsFun
{
    public class WordObject
    {
        public string word;
        public List<Phonetics> phonetics = new List<Phonetics>();
        public List<Definitions> definitions = new List<Definitions>();

        public WordObject(string word)
        {
            this.word = word;
            UtilityTools.DoGetRequest(Constants.WORD_DEFINITION_URL + word, json =>
            {
                var result = JsonConvert.DeserializeObject<List<JObject>>(json)[0];
                Parse(result);
            }, null);
        }

        public void Parse(JObject jObject)
        {
            word = jObject["word"].ToString();

            var phoneticObject = JsonConvert.DeserializeObject<List<JObject>>(jObject["phonetics"].ToString());
            foreach(var item in phoneticObject)
            {
                Phonetics p = new Phonetics
                {
                    audio = item.ContainsKey("audio") ? item["audio"].ToString() : null,
                    text = item.ContainsKey("text") ? item["text"].ToString() : null
                };
                phonetics.Add(p);
            }

            var meanings = JsonConvert.DeserializeObject<List<JObject>>(jObject["meanings"].ToString());
            foreach(var block in meanings)
            {
                var definitions = JsonConvert.DeserializeObject<List<JObject>>(block["definitions"].ToString());
                foreach(var item in definitions)
                {
                    Definitions d = new Definitions
                    {
                        partOfSpeech = block["partOfSpeech"].ToString(),
                        text = item["definition"].ToString(),
                        example = item.ContainsKey("example") ? item["example"].ToString() : null
                    };
                    this.definitions.Add(d);
                }
            }
        }

        public class Phonetics
        {
            public string audio;
            public string text;
        }

        public class Definitions
        {
            public string partOfSpeech;
            public string text;
            public string example;
        }
    }
}
