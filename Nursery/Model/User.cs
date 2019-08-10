using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using System.Numerics;

namespace Nursery.Model
{
    public class User
    {
        private bool wantToBeWorker;
        public static ObservableCollection<User> users;
        private static readonly string path = "Data/RegisteredUsers.xml";
        private static decimal balanceOfOrganization;
        private ObservableCollection<Pet> myPets;
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Addres { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        private decimal money;
        public Males Male { get; set; }
        public StatusOfPeople Status { get; set; }
        public DateTime DateOfBorn { get; set; }
        public string Login { get; set; }
        public ObservableCollection<Pet> MyPets { get => myPets; set => myPets = value; }
        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }

        public User()
        {

        }
        public User(string FirstName, string SecondName, string LastName, string Addres, string PhoneNumber, string Login, string Password, StatusOfPeople Status, Males Male, DateTime DateOfBorn, string SecretQuestion, string SecretAnswer)
        {
            myPets = new ObservableCollection<Pet>();
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.LastName = LastName;
            this.Addres = Addres;
            this.PhoneNumber = PhoneNumber;
            this.Password = GetSHA256(Password, Login);
            this.Login = Login;
            this.Status = Status;
            this.Male = Male;
            this.DateOfBorn = DateOfBorn;
            this.Money = 0;
            this.SecretQuestion = SecretQuestion;
            this.SecretAnswer = GetSHA256(SecretAnswer, Login);
            if (Status._Status == StatusEnum.adminisrator || Status._Status == StatusEnum.superadmin)
            {
                this.Money = balanceOfOrganization;
            }
            if (users == null)
            {
                users = new ObservableCollection<User>();
            }
            users.Add(this);
            Save();
        }

        public static int GetIndexOfUser(User user)
        {
            int indexOfUser = -1;
            for (int i = 0; i < User.users.Count; i++)
            {
                if (user.Login == User.users[i].Login)
                {
                    indexOfUser = i;
                    break;

                }
            }
            return indexOfUser;
        }

        public bool WantToBeWorker
        {
            get
            {

                return wantToBeWorker;
            }
            set
            {
                wantToBeWorker = value;
            }
        }
        public string Age
        {
            get
            {
                return $"{DateTime.Now.Year - DateOfBorn.Year}";
            }
        }

        public string WantToBeWorkerString
        {
            get
            {
                return WantToBeWorker ? "Есть" : "Нет";
            }
        }



        public decimal Money
        {
            get => money;
            set
            {
                if (Status != null && Status._Status == StatusEnum.adminisrator || Status != null && Status._Status == StatusEnum.superadmin)
                {
                    balanceOfOrganization = value;
                }
                money = value;
            }
        }




        public static bool CheckLoginDublicate(string log)
        {
            if (users == null)
            {
                return false;
            }
            else if ((users.Where(x => x.Login == log)).Count() != 0)
            {
                MessageBox.Show($"Пользователь с  логином {log} уже есть!!!");
                return true;
            }
            return false;

        }

        public bool CheckSecretAnswer(string ans)
        {
            if (VerifySHA256Hash(ans, this.SecretAnswer, this.Login))
            {
                return true;
            }
            return false;
        }


        public void AddPet(Pet pet)
        {
            if (myPets == null)
            {
                myPets = new ObservableCollection<Pet>();

            }
            myPets.Add(pet);
            Save();
        }

        public void SetPassword(string pass, string salt)
        {
            this.Password = GetSHA256(pass, salt);
        }


        //static string GetMd5Hash(string input)
        //{
        //    MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
        //    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        //    StringBuilder sBuilder = new StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }
        //    return sBuilder.ToString();
        //}

        //static bool VerifyMd5Hash(string input, string hash)
        //{
        //    string hashOfInput = GetMd5Hash(input);
        //    StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        //    return (0 == comparer.Compare(hashOfInput, hash));
        //}

        private static string GetSHA256(string openText, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] openTextInBytes = Encoding.UTF8.GetBytes(openText);
            byte[] saltInBytes = Encoding.UTF8.GetBytes(salt);
            byte[] openTextWithSaltInBytes = new byte[openTextInBytes.Length + saltInBytes.Length];
            for (int i = 0; i < openTextInBytes.Length; i++)
            {
                openTextWithSaltInBytes[i] = openTextInBytes[i];
            }
            for (int i = 0; i < saltInBytes.Length; i++)
            {
                openTextWithSaltInBytes[openTextInBytes.Length + i] = saltInBytes[i];
            }
            return Convert.ToBase64String(algorithm.ComputeHash(openTextWithSaltInBytes));
        }
        private static bool VerifySHA256Hash(string input, string hash, string salt)
        {
            string hashOfInput = GetSHA256(input, salt);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (comparer.Compare(hashOfInput, hash) == 0);
        }




        internal static User SignIn(string login, string password)
        {
            if (users != null)
            {
                foreach (var item in users)
                {
                    if (item.Login == login && VerifySHA256Hash(password, item.Password, login))
                    {

                        return item;
                    }
                }
            }
            return null;
        }

        internal static void Save()
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Status._Status == StatusEnum.adminisrator || users[i].Status._Status == StatusEnum.superadmin)
                {
                    users[i].Money = balanceOfOrganization;
                }
            }
            XmlSerializer serializer = new XmlSerializer(typeof(User[]));
            using (FileStream fileStram = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStram, users.ToArray());
                fileStram.Close();
            }
        }
        internal static void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(User[]));
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (serializer.Deserialize(fileStream) is User[] _users)
                    {
                        users = new ObservableCollection<User>(_users);
                    }
                    fileStream.Close();
                }
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Status._Status == StatusEnum.adminisrator || users[i].Status._Status == StatusEnum.superadmin)
                    {
                        users[i].Money = balanceOfOrganization;
                    }
                }
            }

        }


    }

}