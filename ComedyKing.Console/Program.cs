using System;
using System.Collections.Generic;
using ComedyKing.DataAccess;
using ComedyKing.Model;

using Microsoft.EntityFrameworkCore;

namespace ComedyKing
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateObjects();
            Console.ReadLine();
        }

        private static void CreateObjects()
        {
            try
            {
              
                using (var data = new CelebrityInCelebrityJokeContext())
                {
                    var comedyJohn = new User()
                    {
                        UserName = "ComedyJohn",
                        Password = "drossap123",
                        FirstName = "John",
                        LastName = "Alabama",
                        DateOfBirth = new DateTime(1987, 12, 12)

                    };

                   
                    var fiftyCent = new Celebrity()
                    {
                        NickName = "50 Cent",
                        FirstName = "Curtis",
                        LastName = "James Jackson III",
                        DateOfBirth = new DateTime(1975, 7, 6),
                        Profession = "Rapper,singer and songwriter"


                    };

                    var gameWasOn = new CelebrityJoke()
                    {
                        Text = "Q: Why did 50 Cent turn off the TV? \nA: The Game was on.",
                        CelebrityMentioned = fiftyCent.NickName,
                        Author = comedyJohn.FirstName +" "+ comedyJohn.LastName,
                        Rate = 3
                    };

                    var nickelbackConcert = new CelebrityJoke()
                    {
                        Text = "Q: What concert costs 45 cents? \nA: 50 cent featuring Nickelback.",
                        CelebrityMentioned = fiftyCent.NickName,
                        Author = comedyJohn.FirstName + " " + comedyJohn.LastName,
                        Rate = 3
                    };

                    fiftyCent.CelebrityJoke = new List<CelebrityInCelebrityJoke>()
                    {
                        new CelebrityInCelebrityJoke
                        {
                          Celebrity = fiftyCent,
                          CelebrityJoke = gameWasOn
                        },

                        new CelebrityInCelebrityJoke
                        {
                           Celebrity = fiftyCent,
                           CelebrityJoke = nickelbackConcert
                        }

                    };
                    
                    data.Jokes.Add(gameWasOn);
                    data.Jokes.Add(nickelbackConcert);
                    data.Celebrities.Add(fiftyCent);
                                  
                    data.SaveChanges();
                    Console.WriteLine("Saved");
                    Console.ReadLine();
                }
            }

            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
