using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace sharpvk.Types
{
    public static class Serializer
    {
        public static string ToJson<T>(this Result<T> self) => JsonConvert.SerializeObject(self, sharpvk.Types.Converter.Settings);
    }


    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }


    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);


        //TODO: remove EmptyAttach mockup
        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "audio":
                    return TypeEnum.Audio;
                case "doc":
                    return TypeEnum.Doc;
                case "photo":
                    return TypeEnum.Photo;
                case "video":
                    return TypeEnum.Video;
                case "link":
                    return TypeEnum.EmptyAttach;
                case "wall":
                    return TypeEnum.EmptyAttach;
                case "sticker":
                    return TypeEnum.EmptyAttach;
                case "gift":
                    return TypeEnum.EmptyAttach;
                case "market":
                    return TypeEnum.EmptyAttach;
                case "wall_reply":
                    return TypeEnum.EmptyAttach;
            }
            throw new Exception($"Cannot unmarshal type TypeEnum: {value}");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Audio:
                    serializer.Serialize(writer, "audio");
                    return;
                case TypeEnum.Doc:
                    serializer.Serialize(writer, "doc");
                    return;
                case TypeEnum.Photo:
                    serializer.Serialize(writer, "photo");
                    return;
                case TypeEnum.Video:
                    serializer.Serialize(writer, "video");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }


    public class Result<T>
    {
        [JsonProperty("error", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }

        [JsonProperty("response", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public T Response { get; set; }

        public static Result<T> FromJson(string json) => JsonConvert.DeserializeObject<Result<T>>(json, sharpvk.Types.Converter.Settings);

        public bool IsError()
        {
            if(IsEmpty())
                throw new Exception("Responses is empty");
            else if(Error != null)
                return true;

            return false;
        } 

        public bool IsEmpty()
        {
            return Error == null && Response == null;
        }
    }


    public class ResponseArray<T>
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("items", Required = Required.Always)]
        public List<T> Items { get; set; }
    }


    public class LikesResponse
    {
        [JsonProperty("likes", Required = Required.Always)]
        public int Likes { get; set; }
    }


    public class ChatMembersResponse
    {
        [JsonProperty("items", Required = Required.Always)]
        public List<MemberInfo> MemberInfo { get; set; }

        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("profiles", Required = Required.Always)]
        public List<Profile> Profiles { get; set; }
    }


    public class LongPollServerResponse
    {
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("server", Required = Required.Always)]
        public string Server { get; set; }

        [JsonProperty("ts", Required = Required.Always)]
        public int Ts { get; set; }
    }


    public class Error
    {
        [JsonProperty("error_code", Required = Required.Always)]
        public int ErrorCode { get; set; }

        [JsonProperty("error_msg", Required = Required.Always)]
        public string ErrorMsg { get; set; }

        [JsonProperty("request_params", Required = Required.Always)]
        public List<RequestParam> RequestParams { get; set; }
    }


    public class RequestParam
    {
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }
    }


    public class MemberInfo
    {
        [JsonProperty("member_id", Required = Required.Always)]
        public int MemberId { get; set; }

        [JsonProperty("join_date", Required = Required.Always)]
        public int JoinDate { get; set; }

        [JsonProperty("invited_by", Required = Required.Always)]
        public int InvitedBy { get; set; }

        [JsonProperty("is_admin", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAdmin { get; set; }
    }


public class Profile
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty("last_name", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty("sex", Required = Required.Always)]
        public int Sex { get; set; }

        [JsonProperty("screen_name", Required = Required.Always)]
        public string ScreenName { get; set; }

        [JsonProperty("photo_50", Required = Required.Always)]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100", Required = Required.Always)]
        public string Photo100 { get; set; }

        [JsonProperty("online", Required = Required.Always)]
        public int Online { get; set; }

        [JsonProperty("online_mobile", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? OnlineMobile { get; set; }
    }


    public class Message
    {
        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("out", Required = Required.Always)]
        public int Out { get; set; }

        [JsonProperty("peer_id", Required = Required.Always)]
        public int PeerId { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("conversation_message_id", Required = Required.Always)]
        public int ConversationMessageId { get; set; }

        [JsonProperty("fwd_messages")]
        public List<FwdMessage> FwdMessages { get; set; }

        [JsonProperty("important", Required = Required.Always)]
        public bool Important { get; set; }

        [JsonProperty("random_id", Required = Required.Always)]
        public int RandomId { get; set; }

        [JsonProperty("attachments", Required = Required.Always)]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("is_hidden", Required = Required.Always)]
        public bool IsHidden { get; set; }
    }


//TODO дообъявить все типы и вынести аттачи в отдельное пространство имен    
    public class Attachment
    {
        [JsonProperty("type", Required = Required.Always)]
        public TypeEnum Type { get; set; }

        [JsonProperty("audio", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Audio Audio { get; set; }

        [JsonProperty("doc", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Doc Doc { get; set; }

        [JsonProperty("photo", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public AttachmentPhoto Photo { get; set; }

        [JsonProperty("video", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Video Video { get; set; }

        [JsonProperty("empty", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public EmptyAttach EmptyAttach { get; set; }


        public override string ToString()
        {
            switch(Type)
            {
                case TypeEnum.Audio:
                    return  Audio.ToString();
                case TypeEnum.Doc:
                    return  Doc.ToString();
                case TypeEnum.Photo:
                    return  Photo.ToString();
                case TypeEnum.Video:
                    return  Video.ToString();
                default:
                    throw new Exception($"undefined type {Type}");
            }
        }
    }


    public enum TypeEnum { Audio, Doc, Photo, Video, EmptyAttach };


    public class Audio
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("artist", Required = Required.Always)]
        public string Artist { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }

        [JsonProperty("lyrics_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? LyricsId { get; set; }

        [JsonProperty("genre_id")]
        public int GenreId { get; set; }

        [JsonProperty("is_hq", Required = Required.Always)]
        public bool IsHq { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }


        public override string ToString()
        {
            string str = $"audio{OwnerId}_{Id}";

            if(AccessKey!="")
            return str + $"_{AccessKey}";

            return str;
        }
    }


    public class Doc
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("size", Required = Required.Always)]
        public int Size { get; set; }

        [JsonProperty("ext", Required = Required.Always)]
        public string Ext { get; set; }

        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }


        public override string ToString()
        {
            string str = $"doc{OwnerId}_{Id}";

            if(AccessKey!="")
            return str + $"_{AccessKey}";

            return str;
        }
    }


    public class AttachmentPhoto
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("album_id")]
        public int AlbumId { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("sizes")]
        public List<Size> Sizes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
        

        public override string ToString()
        {
            string str = $"photo{OwnerId}_{Id}";

            if(AccessKey!="")
            return str + $"_{AccessKey}";

            return str;
        }        
    }


    public class Video
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("file_size")]
        public long? FileSize { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }


        public override string ToString()
        {
            string str = $"video{OwnerId}_{Id}";

            if(AccessKey!="")
            return str + $"_{AccessKey}";

            return str;
        }
    }


    public class Size
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }

        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }
    }


    public class EmptyAttach
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
    }


    public class FwdMessage
    {
        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("attachments", Required = Required.Always)]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("update_time", Required = Required.Always)]
        public int UpdateTime { get; set; }
    }
//

    public class ChatInfo
    {
        [JsonProperty("conversation", Required = Required.Always)]
        public Conversation Conversation { get; set; }

        [JsonProperty("last_message", Required = Required.Always)]
        public Message LastMessage { get; set; }
    }

    public class Conversation
    {
        [JsonProperty("peer", Required = Required.Always)]
        public Peer Peer { get; set; }

        [JsonProperty("in_read", Required = Required.Always)]
        public int InRead { get; set; }

        [JsonProperty("out_read", Required = Required.Always)]
        public int OutRead { get; set; }

        [JsonProperty("last_message_id", Required = Required.Always)]
        public int LastMessageId { get; set; }

        [JsonProperty("can_write", Required = Required.Always)]
        public CanWrite CanWrite { get; set; }

        [JsonProperty("chat_settings")]
        public ChatSettings ChatSettings { get; set; }

        [JsonProperty("push_settings")]
        public PushSettings PushSettings { get; set; }
    }


    public class CanWrite
    {
        [JsonProperty("allowed", Required = Required.Always)]
        public bool Allowed { get; set; }
    }


    public class ChatSettings
    {
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("members_count")]
        public int MembersCount { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("photo")]
        public ChatPhoto ChatPhoto { get; set; }

        [JsonProperty("active_ids")]
        public List<long> ActiveIds { get; set; }
    }


    public class ChatPhoto
    {
        [JsonProperty("photo_50", Required = Required.Always)]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100", Required = Required.Always)]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200", Required = Required.Always)]
        public string Photo200 { get; set; }
    }


    public class Peer
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty("local_id", Required = Required.Always)]
        public int LocalId { get; set; }
    }


    public class PushSettings
    {
        [JsonProperty("no_sound", Required = Required.Always)]
        public bool NoSound { get; set; }

        [JsonProperty("disabled_until", Required = Required.Always)]
        public int DisabledUntil { get; set; }

        [JsonProperty("disabled_forever", Required = Required.Always)]
        public bool DisabledForever { get; set; }
    }


    public class WallPost
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("marked_as_ads")]
        public int MarkedAsAds { get; set; }

        [JsonProperty("post_type")]
        public string PostType { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("is_pinned")]
        public int IsPinned { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("post_source")]
        public PostSource PostSource { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("likes", Required = Required.Always)]
        public Likes Likes { get; set; }

        [JsonProperty("reposts", Required = Required.Always)]
        public Reposts Reposts { get; set; }

        [JsonProperty("views", Required = Required.Always)]
        public Views Views { get; set; }
    }


    public class Comments
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("groups_can_post", Required = Required.Always)]
        public bool GroupsCanPost { get; set; }

        [JsonProperty("can_post", Required = Required.Always)]
        public int CanPost { get; set; }
    }


    public class Likes
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("user_likes", Required = Required.Always)]
        public int UserLikes { get; set; }

        [JsonProperty("can_like", Required = Required.Always)]
        public bool CanLike { get; set; }

        [JsonProperty("can_publish", Required = Required.Always)]
        public bool CanPublish { get; set; }
    }


    public class PostSource
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }
    }


    public class Reposts
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("user_reposted", Required = Required.Always)]
        public int UserReposted { get; set; }
    }


    public class Views
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }
    }
}
