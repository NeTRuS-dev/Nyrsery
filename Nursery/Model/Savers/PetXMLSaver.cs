using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Nursery.Model.Savers
{
    public class PetXmlSaver : ISaver
    {
        private static readonly string path = "Data/RegisteredPets.xml";

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pet[]));
            using (FileStream fileStram = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStram, Pet.Pets.ToArray());
                fileStram.Close();
            }
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pet[]));
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (serializer.Deserialize(fileStream) is Pet[] _pets)
                    {
                        Pet.Pets = new ObservableCollection<Pet>(_pets);
                    }
                    fileStream.Close();
                }
            }
        }
    }
}