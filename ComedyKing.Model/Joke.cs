using System;

namespace ComedyKing.Model
{
    public abstract class Joke
    {
        public int JokeID { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public int Rate { get; set; }
    }
}

