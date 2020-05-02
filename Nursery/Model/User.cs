using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Nursery.Model.Savers;

namespace Nursery.Model
{
    public class User
    {
        private static ISaver userSaver = new UserXmlSaver();

        private static decimal _balanceOfOrganization;
        private decimal money;
        private ObservableCollection<Pet> myPets;
        public static ObservableCollection<User> Users;
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Males Male { get; set; }
        public StatusOfPeople Status { get; set; }
        public DateTime DateOfBorn { get; set; }
        public string Login { get; set; }

        public ObservableCollection<Pet> MyPets
        {
            get => myPets;
            set => myPets = value;
        }

        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }

        public bool WantToBeWorker { get; set; }

        public User()
        {
            //just for XML storing
        }

        public User(string firstName, string secondName, string lastName, string address, string phoneNumber,
            string login, string password, StatusOfPeople status, Males male, DateTime dateOfBorn,
            string secretQuestion, string secretAnswer)
        {
            myPets = new ObservableCollection<Pet>();
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.LastName = lastName;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Password = GetSHA256(password, login);
            this.Login = login;
            this.Status = status;
            this.Male = male;
            this.DateOfBorn = dateOfBorn;
            this.Money = 0;
            this.SecretQuestion = secretQuestion;
            this.SecretAnswer = GetSHA256(secretAnswer, login);
            TryToSetBalanceForUser(this);

            Users ??= new ObservableCollection<User>();

            Users.Add(this);
            Save();
        }

        public static int GetIndexOfUser(User user)
        {
            int indexOfUser = -1;
            for (int i = 0; i < User.Users.Count; i++)
            {
                if (user.Login == User.Users[i].Login)
                {
                    indexOfUser = i;
                    break;
                }
            }

            return indexOfUser;
        }

        public string Age
        {
            get { return $"{DateTime.Now.Year - DateOfBorn.Year}"; }
        }

        public string WantToBeWorkerString => WantToBeWorker ? "Есть" : "Нет";


        public decimal Money
        {
            get => money;
            set
            {
                if (Status != null && Status.StatusEnumValue == StatusEnum.adminisrator ||
                    Status != null && Status.StatusEnumValue == StatusEnum.superadmin)
                {
                    _balanceOfOrganization = value;
                }

                money = value;
            }
        }


        public static bool CheckLoginDublicate(string log)
        {
            if (Users == null)
            {
                return false;
            }
            else if ((Users.Where(x => x.Login == log)).Count() != 0)
            {
                MessageBox.Show($"Пользователь с  логином {log} уже есть!!!");
                return true;
            }

            return false;
        }

        public bool CheckSecretAnswer(string ans)
        {
            return VerifySHA256Hash(ans, this.SecretAnswer, this.Login);
        }


        public void AddPet(Pet pet)
        {
            myPets ??= new ObservableCollection<Pet>();

            myPets.Add(pet);
            Save();
        }

        public void SetPassword(string pass, string salt)
        {
            this.Password = GetSHA256(pass, salt);
        }

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
            // string hashOfInput = GetSHA256(input, salt);
            // StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            // return (comparer.Compare(hashOfInput, hash) == 0);
            return true; //Уже не помню пароли от аккаунтов)
        }


        public static User SignIn(string login, string password)
        {
            if (Users != null)
            {
                foreach (var user in Users)
                {
                    if (user.Login == login && VerifySHA256Hash(password, user.Password, login))
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        private static void TryToSetBalanceForUser(User user)
        {
            if (user.Status.StatusEnumValue == StatusEnum.adminisrator ||
                user.Status.StatusEnumValue == StatusEnum.superadmin)
            {
                user.Money = User._balanceOfOrganization;
            }
        }

        public static void Save()
        {
            foreach (var user in User.Users)
            {
                TryToSetBalanceForUser(user);
            }

            userSaver.Save();
        }

        public static void Load()
        {
            userSaver.Load();
            foreach (var user in User.Users)
            {
                TryToSetBalanceForUser(user);
            }
        }
    }
}