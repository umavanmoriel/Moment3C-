// Ger tillgång till grundläggande klasser
using System;
// Ger tillgång till lista List<T>
using System.Collections.Generic;
// Ger tillgång till fil klassen för att läsa och skriva filer
using System.IO;
// Ger tillgång till JsonSerializer för att spara,läsa inlägg som JSON
using System.Text.Json;


namespace MyApplication
{
    // Klass för inlägg i gästboken
    class SavedPost
    {
        public string UserName;
        public string UserPost;
    }

    class Program
    {
        // Lista med alla inlägg
        static List<SavedPost> posts = new List<SavedPost>();

        static void Main(string[] args)
        {
            // Läser in alla sparade inlägg från fil
            LoadPosts();

            string choice;

            // Loopar visar meny, läser val och utför val
            do
            {
                // Rensar meny
                Console.Clear();

                // Skriver ut titel och meny
                string title = "Gästbok";
                Console.WriteLine(title);
                Console.WriteLine("=== Meny ===");
                Console.WriteLine("1. Skriv i gästboken");
                Console.WriteLine("2. Ta bort inlägg");
                Console.WriteLine("3. Visa alla inlägg");
                Console.WriteLine("0. Avsluta!");

                // Läser in användarens val
                choice = Console.ReadLine() ?? "0";

                // Utför rätt metod beroende på val
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Avsluta!");
                        break;
                    case "1":
                        AddPost();
                        break;
                    case "2":
                        DeletePost();
                        break;
                    case "3":
                        AllPostsShow();
                        break;
                    default:
                        // Visa meddelande vid ogiltigt val
                        Console.WriteLine("Ogiltigt val! Försök igen!");
                        break;
                }

                if (choice != "0")
                {
                    Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka...");
                    Console.ReadKey();
                }

            } while (choice != "0");
        }

        // Lägger till nytt inlägg i gästboken
        static private void AddPost()
        {
            string userName;
            string userPost;

            do
            {
                Console.Write("Användare: ");
                userName = Console.ReadLine();
                //Visar felmeddelande till användare
                if (string.IsNullOrWhiteSpace(userName))
                {
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                }
            //Kontrolerrar att Användarfält ät inte tomt
            } while (string.IsNullOrWhiteSpace(userName));

            do
            {
                Console.Write("Inlägg: ");
                userPost = Console.ReadLine();
                //Visar felmeddelande till användare
                if (string.IsNullOrWhiteSpace(userName))
                {
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                }
            //Kontrolerrar att Textfält ät inte tomt
            } while (string.IsNullOrWhiteSpace(userPost));

            // Skapar nytt objekt av klassen SavedPost och fyller i fälten
            SavedPost postObj = new SavedPost();
            postObj.UserName = userName;
            postObj.UserPost = userPost;

            // Lägger objekt i lista
            posts.Add(postObj);

            // Sparar listan till fil
            SavePosts();
        }

        // Visar alla inlägg i listan med ett index
        static private void AllPostsShow()
        {
            // Skriver ut felmeddelade om listan är tomt
            if (posts.Count == 0)
            {
                Console.WriteLine("Inlägg saknas");
                return;
            }

            // Loopar igenom alla inlägg och skriver de ut
            for (int i = 0; i < posts.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + posts[i].UserName + " - " + posts[i].UserPost);
            }
        }

        // Ta bort ett inlägg genom att fråga efter index
        static private void DeletePost()
        {
            // Skriver ut felmeddelade om listan är tomt
            if (posts.Count == 0)
            {
                Console.WriteLine("Inlägg saknas");
                return;
            }

            // Visar listan med alla inlägg
            AllPostsShow();

            // Frågar efter vilket index som ska tas bort
            Console.WriteLine("Radera inlägg efter index");
            string input = Console.ReadLine();

            // Kontrollerar att användare skrev en siffra och att indexet är mellan 1 och posts.Count
            if (int.TryParse(input, out int userIndex) && userIndex >= 1 && userIndex <= posts.Count)
            {
                posts.RemoveAt(userIndex - 1);
                Console.WriteLine("Inlägget togs bort.");

                // Sparar listan till fil så att borttagningen finns kvar nästa gång
                SavePosts();
            }
            else
            {
                Console.WriteLine("Ogiltigt nummer.");
            }
        }

        // Sparar hela listan till fil som JSON
        static private void SavePosts()
        {
            // Omvandlar listan till JSON
            string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Skriver JSON sträng till filen guestbook.json
            File.WriteAllText("guestbook.json", json);
        }

        // Läser in sparade inlägg från JSON fil
        static private void LoadPosts()
        {
            // Avslutar metoden om filen inte finns
            if (!File.Exists("guestbook.json"))
            {
                return;
            }

            // Läser filen
            string json = File.ReadAllText("guestbook.json");

            // Omvandlar JSON sträng tillbaka till en lista
            List<SavedPost> loaded = JsonSerializer.Deserialize<List<SavedPost>>(json);

            if (loaded != null)
            {
                posts = loaded;
            }
        }
    }
}