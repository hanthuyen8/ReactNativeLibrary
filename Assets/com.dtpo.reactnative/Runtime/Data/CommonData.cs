using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    [SerializeField]
    public class CommonData
    {
        [JsonProperty("action")]
        public string Action;

        [JsonProperty("data")]
        public string Data;
    }

    public class RecordAudioData
    {
        [JsonProperty("action")]
        public const string Action = "sendRecord";

        [JsonProperty("data")]
        public Data RecordData;

        public RecordAudioData(string keyWord, string path)
        {
            RecordData = new Data
            {
                KeyWord = keyWord,
                Path = path
            };
        }

        [Serializable]
        public class Data
        {
            [JsonProperty("keyword")]
            public string KeyWord;

            [JsonProperty("path")]
            public string Path;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
