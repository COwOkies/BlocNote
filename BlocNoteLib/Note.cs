using System.ComponentModel;
using System.Runtime.Serialization;

namespace BlocNoteLib
{
    [DataContract]
    public class Note
    {
        [DataMember]
        private string title = "";
        public string Title { 
            get => title; 
            set
            {
                if (value == null)
                    title = "";
                else
                    title = value;

            }
        }
        [DataMember]
        private string text = "";
        public string Text
        {
            get => text;
            set
            {
                if (value == null)
                    text = "";
                else
                    text = value;

            }
        }
        [DataMember]
        private DateTime date = DateTime.Now;
        public DateTime Date 
        {
            get => date;
            set
            { 
                date = value;
            }
        }

        [DataMember]
        private DateTime dateUpdate = DateTime.Now;
        public DateTime DateUpdate
        {
            get => dateUpdate;
            set
            {
                dateUpdate = value;
            }
        }

        public Note(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}
