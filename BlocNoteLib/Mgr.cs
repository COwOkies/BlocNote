using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Channels;
using System.Xml;

namespace BlocNoteLib
{
    [DataContract]
    public class Mgr
    {
        [DataMember]
        public ObservableCollection<Note> notes = new ObservableCollection<Note>();
        public ObservableCollection<Note> Notes { get => notes; set => notes = value; }

        [DataMember]
        public Credentials creds = new Credentials("", "");
        public Credentials Creds { get => creds; set => creds = value; }

        public Mgr() {
        }

        private string NotePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","..","..","..", "..","Note.xml");
        private string SavedNotePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "..", "SavedNote.xml");
        private string CredsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "..", "Creds.xml");

        public IList<string> colorList = new List<string> { "Red","Orange","Yellow","Green","Blue","Indigo","Violet", "Black","Gray","White" };
        public IList<string> ColorList { get => colorList; set => colorList = value; }

        public void SaveNotes()
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            var serializerNotes = new DataContractSerializer(typeof(ObservableCollection<Note>));

            using (TextWriter tw = File.CreateText(NotePath))

            using (XmlWriter writer = XmlWriter.Create(tw, settings))
            {
                serializerNotes.WriteObject(writer, notes);
            }
        }

        public void SaveCreds()
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            var serializer = new DataContractSerializer(typeof(Credentials));

            var creds = new Credentials(Creds.discordToken,Creds.channelID);

            using (TextWriter tw = File.CreateText(CredsPath))
            using (XmlWriter writer = XmlWriter.Create(tw, settings))
            {
                serializer.WriteObject(writer, creds);
            }
        }

        public ObservableCollection<Note> LoadNotes()
        {
            if (!File.Exists(NotePath)) return notes;

            var deserializerNotes = new DataContractSerializer(typeof(ObservableCollection<Note>));
            using (Stream s = File.OpenRead(NotePath))
            {
                ObservableCollection<Note> tempList = deserializerNotes.ReadObject(s) as ObservableCollection<Note>;

                if (tempList != null)
                {
                    notes.Clear();
                    foreach (var note in tempList)
                    {
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        public ObservableCollection<Note> LoadSavedNotes()
        {
            if (!File.Exists(SavedNotePath)) return notes;

            var deserializerNotes = new DataContractSerializer(typeof(ObservableCollection<Note>));
            using (Stream s = File.OpenRead(SavedNotePath))
            {
                ObservableCollection<Note> tempList = deserializerNotes.ReadObject(s) as ObservableCollection<Note>;

                if (tempList != null)
                {
                    notes.Clear();
                    foreach (var note in tempList)
                    {
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        public (string, string) LoadCreds()
        {
            if (!File.Exists(CredsPath)) return ("", "");

            var deserializer = new DataContractSerializer(typeof(Credentials));
            using (Stream s = File.OpenRead(CredsPath))
            {
                var creds = deserializer.ReadObject(s) as Credentials;

                if (creds != null)
                {
                    return (creds.DiscordToken, creds.ChannelID);
                }
            }

            return ("", "");
        }

    }
}
