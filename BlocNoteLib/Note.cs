using System.ComponentModel;
using System.Drawing;
using Windows.UI.Xaml.Media;
using System.Globalization;
using System.Runtime.Serialization;

namespace BlocNoteLib
{
    [DataContract]
    public class Note
    {
        [DataMember]
        private string title = "";
        public string Title
        {
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

        public string ShortText
        {
            get
            {
                int size = 400;
                if (text.Length >= size)
                    return text.Substring(0, size) + "...";
                else
                    return text;
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

        [DataMember]
        private string color = "White";
        public string Color
        {
            get => color;
            set
            {
                color = value;
            }
        }

        public Note(string title, string text, string color)
        {
            Title = title;
            Text = text;

            Color = color;
        }

    }
}
