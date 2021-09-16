using System;
using sharpvk.Types;
using System.Collections.Generic;



namespace sharpvk
{
    public class ApiClient
    {
        private RequestSender sender;


        public ApiClient(Token t, int maxReqCount)
        {
            sender = new RequestSender(t, maxReqCount);
        }


        public List<Message> SearchMessages(int count, string template)
        {
            Result<ResponseArray<Message>> msgs = sender.Send<ResponseArray<Message>>(new ApiRequest($"messages.search?count={count}&q={template}"));

            if(msgs.IsError())
                throw new Exception(msgs.Error.ErrorMsg);

            return msgs.Response.Items;
        }


        public List<WallPost> WallGet(int owner_id, int count=1, int offset=0)
        {
            Result<ResponseArray<WallPost>> posts = sender.Send<ResponseArray<WallPost>>(new ApiRequest($"wall.get?extended=0&owner_id={owner_id}&count={count}&offset={offset}"));

            if(posts.IsError())
                throw new Exception(posts.Error.ErrorMsg);

            return posts.Response.Items;
        }


        public int AddLike(WallPost post)
        {
            if(!post.Likes.CanLike)
                return 0;

            Result<LikesResponse> resp = sender.Send<LikesResponse>(new ApiRequest($"likes.add?type=post&owner_id={post.OwnerId}&item_id={post.Id}"));

            if(resp.IsError())
                throw new Exception(resp.Error.ErrorMsg);

            return resp.Response.Likes;
        }


        public void AddLike(List<WallPost> posts)
        {
            foreach(WallPost post in posts)
                AddLike(post);
        }


        public int CopyMessage(Message msg, int uid)
        {
            Result<int> resp = sender.Send<int>(ConvertMessageToReq(msg, uid));

            if(resp.IsError())
                throw new Exception(resp.Error.ErrorMsg);

            return resp.Response;
        }


        private ApiRequest ConvertMessageToReq(Message msg, int uid)
        {
            Dictionary<string, string> prms = new Dictionary<string, string>();
            prms["user_id"] = uid.ToString();
            
            if(msg.Text == "" && msg.Attachments.Count == 0)
                throw new Exception("text ot attachments must be declared in message");

            if(msg.Text != "")
                prms["message"] = msg.Text;

            if(msg.Attachments.Count != 0)
            {
                string att = "";
                foreach(Attachment a in msg.Attachments)
                    att += $",{a.ToString()}";
                prms["attachment"] = att.Substring(1, att.Length-1); 
            }

            return new ApiRequest("messages.send", prms);
        }
    }
}