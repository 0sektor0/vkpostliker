using System;
using sharpvk.Types;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using sharpvk;



namespace vkpostliker
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = AccountReader.GetUsers("./data/settings.json");
            foreach(Group g in settings.Groups)
                foreach (User u in settings.Users)
                    AddLike(u, g);

            Console.WriteLine($"success {DateTime.UtcNow}");
        }


        static void AddLike(User u, Group g)
        {
            try
            {
                Token t = new Token(u.Login, u.Password, 274556);
                Console.WriteLine($"{u.Login}: authorized");    

                ApiClient cl = new ApiClient(t, 3);
                List<WallPost> posts = cl.WallGet(g.Id, g.Count, g.Offset);
                Console.WriteLine($"posts: {posts.Count}");

                cl.AddLike(posts);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}