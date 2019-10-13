using System.Collections.Generic;

namespace ComedyKing.Model
{
    public class CelebrityJoke : Joke
    {
        public string CelebrityMentioned { get; set; }
        public ICollection<CelebrityInCelebrityJoke> Celebrity { get; set; }
    }
}

