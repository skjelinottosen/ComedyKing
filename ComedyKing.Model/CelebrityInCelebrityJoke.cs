namespace ComedyKing.Model
{
    public class CelebrityInCelebrityJoke
    {
        public int CelebrityID { get; set; }
        public Celebrity Celebrity { get; set; }
        public int CelebrityJokeID { get; set; }
        public CelebrityJoke CelebrityJoke { get; set; }
    }
}
