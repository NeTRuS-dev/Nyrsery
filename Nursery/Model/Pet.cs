using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Nursery.ViewModel;

namespace Nursery.Model
{

    public class Pet
    {

        public static ObservableCollection<Pet> pets;
        private static readonly string path = "Data/RegisteredPets.xml";
        public string Name { get; set; }
        public string Decription { get; set; }
        public int Age { get; set; }
        public decimal Price { get; set; }
        public bool Vactinade { get; set; }
        public TypeOfPet Type { get; set; }
        public Males Male { get; set; }
        public DateTime LastTimeOfEating { get; set; }
        public bool PayedEating { get; set; }
        public string ImagePath { get; set; }



        public Pet()
        {

        }

        public Pet(string Name, string Decription, int Age, decimal Price, bool Vactinade, TypeOfPet Type, Males male, string ImagePath)
        {
            this.Name = Name;
            this.Decription = Decription;
            this.Age = Age;
            this.Price = Price;
            this.Vactinade = Vactinade;
            this.Type = Type;
            this.Male = male;
            this.PayedEating = false;
            LastTimeOfEating = DateTime.Now;
            this.ImagePath = ImagePath;
            if (pets == null)
            {
                pets = new ObservableCollection<Pet>();
            }
            pets.Add(this);
            Save();
        }

        public string LastTimeOfEatingStr
        {
            get
            {
                return $"{LastTimeOfEating.ToLongDateString()}{LastTimeOfEating.ToShortTimeString()}";
            }
        }
        public string GetVactcina
        {
            get
            {
                return Vactinade ? "Да" : "Нет";
            }
        }

        public string WantToEat
        {
            get
            {

                return PayedEating ? "Да" : "Нет";
            }
        }

        public BitmapImage bitmapImage
        {
            get
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(ImagePath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }


        public static bool CheckLoginDublicate(string log)
        {
            if (pets == null)
            {
                return false;
            }
            else if ((pets.Where(x => x.Name == log)).Count() != 0)
            {
                MessageBox.Show($"Питомец с именем {log} уже есть!!!");
                return true;
            }
            return false;

        }


        public static int GetIndexOfPet(Pet pet)
        {
            int indexOfPet = -1;
            for (int i = 0; i < Pet.pets.Count; i++)
            {
                if (pet.Name == Pet.pets[i].Name)
                {
                    indexOfPet = i;
                    break;

                }
            }
            return indexOfPet;
        }
        public static event Action SelectedPetChanged;
        public static Pet SelectedPet { get; set; } //datacontext для itemscontrols
        public static RealiseCommand SelectPet
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    var tmp = obj as PetExtender;
                    for (int i = 0; i < Pet.pets.Count; i++)
                    {
                        if (tmp.Name== Pet.pets[i].Name)
                        {
                            SelectedPet = Pet.pets[i];

                        }
                    }
                    //SelectedPet = obj as Pet;
                    SelectedPetChanged?.Invoke();
                }));
            }
        }

        internal static void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pet[]));
            using (FileStream fileStram = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStram, pets.ToArray());
                fileStram.Close();
            }
        }
        internal static void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pet[]));
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (serializer.Deserialize(fileStream) is Pet[] _pets)
                    {
                        pets = new ObservableCollection<Pet>(_pets);
                    }
                    fileStream.Close();
                }
            }

        }
    }
}

