using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Nursery.Model.Savers
{
    public class LogXmlSaver : ISaver
    {
        private static readonly string path = "Data/Logs.xml";

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Log[]));
            using (FileStream fileStram = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStram, Log.LogsList.ToArray());
                fileStram.Close();
            }
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Log[]));
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (serializer.Deserialize(fileStream) is Log[] _logs)
                    {
                        Log.LogsList = new ObservableCollection<Log>(_logs);
                    }
                    fileStream.Close();
                }

            }

        }
    }
}