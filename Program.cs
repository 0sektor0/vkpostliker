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
            UsersList users = AccountReader.GetUsers("./data/users.json");
            foreach (User u in users.Users)
                AddLike(u, 50, 0);
            Console.WriteLine("done");
        }


        static void AddLike(User u, int count, int offset)
        {
            Token t = null;

            try
            {
                t = new Token(u.Login, u.Password, 274556);
                Console.WriteLine($"{u.Login}: authorized");
            }
            catch
            {
                Console.WriteLine($"{u.Login}: not authorized");
            }

            ApiClient cl = new ApiClient(t, 3);
            //List<WallPost> posts = cl.WallGet(-129223693, count, offset);
            List<WallPost> posts = cl.WallGet(-121519170, count, offset);
            Console.WriteLine($"posts: {posts.Count}");

            cl.AddLike(posts);
        }
    }
}