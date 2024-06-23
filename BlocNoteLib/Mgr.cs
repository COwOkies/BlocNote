using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;

namespace BlocNoteLib
{
    [DataContract]
    public class Mgr
    {
        [DataMember]
        public ObservableCollection<Note> notes = new ObservableCollection<Note>();
        public ObservableCollection<Note> Notes { get => notes; set => notes = value; }

        public Mgr() { }

        private string SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","..","..","..", "..","Save.xml");

        public void SaveResult()
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            var serializerNotes = new DataContractSerializer(typeof(ObservableCollection<Note>));

            using (TextWriter tw = File.CreateText(SavePath))

            using (XmlWriter writer = XmlWriter.Create(tw, settings))
            {
                serializerNotes.WriteObject(writer, notes);
            }
        }

        public ObservableCollection<Note> LoadResult()
        {
            if (!File.Exists(SavePath)) return notes;

            var deserializerNotes = new DataContractSerializer(typeof(ObservableCollection<Note>));
            using (Stream s = File.OpenRead(SavePath))
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

    }
}
