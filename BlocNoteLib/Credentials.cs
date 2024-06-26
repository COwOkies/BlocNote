using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlocNoteLib
{
    [DataContract]
    public class Credentials
    {
        [DataMember]
        public string discordToken;
        public string DiscordToken { get => discordToken; set => discordToken = value; }

        [DataMember]
        public string channelID;
        public string ChannelID { get => channelID; set => channelID = value; }
        
        public Credentials(string token,string id)
        {
            DiscordToken = token;
            ChannelID = id;
        }
    }
}
