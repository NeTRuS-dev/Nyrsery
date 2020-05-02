using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Nursery.Model.Savers
{
    public class UserXmlSaver : ISaver
    {
        private static readonly string path = "Data/RegisteredUsers.xml";

        public void Save()
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(User[]));
            using (FileStream fileSteam = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileSteam, User.Users.ToArray());
                fileSteam.Close();
            }
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(User[]));
            if (!File.Exists(path)) return;
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                if (serializer.Deserialize(fileStream) is User[] _users)
                {
                    User.Users = new ObservableCollection<User>(_users);
                }

                fileStream.Close();
            }
        }
    }
}