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
            List<Task> tasks = new List<Task>();

            foreach (User u in settings.Users)
                tasks.Add(AddLikesAsync(u, settings.Groups));

            Console.WriteLine($"waiting for {tasks.Count} tasks to finish");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"success {DateTime.UtcNow}");
        }


        static void AddLikes(User u, List<Group> groups)
        {
            try
            {
                Token t = new Token(u.Login, u.Password, 274556);
                Console.WriteLine($"{u.Login}: authorized");    

                ApiClient cl = new ApiClient(t, 3);

                foreach(Group g in groups)
                {
                    List<WallPost> posts = cl.WallGet(g.Id, g.Count, g.Offset);
                    cl.AddLike(posts);
                    Console.WriteLine($"{u.Login}: {g.Id} success");
                }
                
                Console.WriteLine($"{u.Login}: success");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"{u.Login}: failed");
            }
        }


        static Task AddLikesAsync(User u, List<Group> groups)
        {
            return Task.Run(() => {AddLikes(u, groups);});
        }
    }
}