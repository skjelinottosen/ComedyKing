using System.Collections.Generic;

namespace ComedyKing.Model
{
    public class Celebrity : Person
    {
        public string NickName { get; set; }
        public string Profession { get; set; }
        public ICollection<CelebrityInCelebrityJoke> CelebrityJoke { get; set; }
        
    }
}
