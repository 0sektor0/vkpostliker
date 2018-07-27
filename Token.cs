using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;



namespace sharpvk
{
    public class Token
    {
        int appid;
        object locker = new object();
        string password;
        string _value;
        string login;
        int scope;

        public DateTime expired_time;
        public bool is_group = false;
        
        public string value
        {
            get
            {
                if (!is_alive)
                    Update();

                return _value;
            }
        }
        
        public bool is_alive
        {
            get
            {
                if (is_group)
                    return true;
                if (DateTime.UtcNow < expired_time)
                    return true;
                else
                    return false;
            }
        }


        public Token(string login, string password, int scope=274556, int appid=5635484)
        {
            this.login = login;
            this.password = password;
            this.scope = scope;
            this.appid = appid;
            Auth();
        }


        private Token(string value, int expires_in)
        {
            _value = value;
            expired_time = DateTime.UtcNow.AddSeconds(expires_in).AddMinutes(-10);
            is_group = expires_in <= 0; //reserve fo future
        }


        private void Auth()
        {
            string html;
            string post_data;
            string[] res = null;
            HttpWebRequest request;
            HttpWebResponse response;
            CookieContainer cookie_container = new CookieContainer();

            //переходим на страницу авторизации
            request = (HttpWebRequest)HttpWebRequest.Create($"https://oauth.vk.com/authorize?client_id={appid}&redirect_uri=https://oauth.vk.com/blank.html&scope={scope}&response_type=token&v=5.53&display=wap");
            request.AllowAutoRedirect = false;
            request.CookieContainer = cookie_container;
            response = (HttpWebResponse)request.GetResponse();

            //считывем код страницы 
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                html = reader.ReadToEnd();
            //составляем пост данные и выдираем csrf токены
            post_data = $"email={login}&pass={password}";
            foreach (Match m in Regex.Matches(html, @"\B<input\stype=""hidden""\sname=""(.+)""\svalue=""(.+)"""))
                post_data += $"&{m.Groups[1]}={m.Groups[2]}";

            //отправляем логин и пароль
            request = (HttpWebRequest)HttpWebRequest.Create("https://login.vk.com/?act=login&soft=1");
            request.CookieContainer = cookie_container;
            request.AllowAutoRedirect = false;
            request.Method = "POST";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(post_data);
            response = GetResponse302(request);

            if (response.Cookies.Count == 0)
                throw new Exception("Invalid login or password");

            //переходим по Location в ответе
            request = (HttpWebRequest)HttpWebRequest.Create(response.Headers["Location"]);
            request.CookieContainer = cookie_container;
            request.AllowAutoRedirect = false;
            response = GetResponse302(request);

            //и еще раз
            request = (HttpWebRequest)HttpWebRequest.Create(response.Headers["Location"]);
            request.CookieContainer = cookie_container;
            request.AllowAutoRedirect = false;
            response = GetResponse302(request);

            res = response.Headers["Location"].Split('=', '&');

            _value = res[1];
            expired_time = DateTime.UtcNow.AddSeconds(Convert.ToInt32(res[3])).AddMinutes(-10);
            is_group = Convert.ToInt32(res[3]) <= 0; //reserve fo future
        }


        public void Update()
        {
            if(!is_alive)
                lock(locker)
                {
                    if(is_alive)
                        return;
                    Auth();
                    Console.WriteLine($"token updated {DateTime.UtcNow}\n{value}");
                }
        }


        //dotnnet core 2.1 rise exception, when get 302 response
        private static HttpWebResponse GetResponse302(HttpWebRequest request)
        {
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException e)
            {
                if (e.Message.Contains("302"))
                {
                    response = (HttpWebResponse)e.Response;
                    return response;
                }

                throw e;
            }
        }
    }
}
