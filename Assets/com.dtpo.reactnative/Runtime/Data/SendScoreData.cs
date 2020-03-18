using Newtonsoft.Json;
using System;

namespace Game
{
    [Serializable]
    public struct SendScoreData
    {
        public string Word;
        public int NumWrong;
        public int Pass; //0: numwrong=3, 1: numwrong
        public int Skill;   //0: choi, 1: voice
        public int Type; //0: word, 1: sentence

    }
}
