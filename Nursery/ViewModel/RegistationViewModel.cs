using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Nursery.Model;
using Nursery.View;

namespace Nursery.ViewModel
{
    internal class RegistationViewModel : BaseViewModel
    {
        
        private Males male;
        private ObservableCollection<Males> males = new ObservableCollection<Males>() { new Males("Мужской"), new Males("Женский"), new Males("Не определился") };
        public ObservableCollection<Males> Males { get => males; }
        private DateTime dateOfBorn;
        private DateTime startTime;
        private DateTime endTime;
        public Brush PassBorder { get; set; }
        private string pass;
        private string passrep;
        private bool passAllowed = false;
        private int minLengthOfPass = 5;

        public RegistationViewModel(Window sender)
        {
            window = sender;
            StartItem = statuses[0];
            DateOfBorn = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);
            SettedMale = males[0];
            PassBorder = Brushes.Gray;
        }
        //Пол
        public Males SettedMale
        {
            get
            {
                return male;
            }
            set
            {
                male = value;
                OnPropertyChanged();
            }
        }
        //Пол конец
        //Должность
        private ObservableCollection<StatusOfPeople> statuses = new ObservableCollection<StatusOfPeople>() { new StatusOfPeople(StatusEnum.client), new StatusOfPeople(StatusEnum.worker), new StatusOfPeople(StatusEnum.adminisrator), new StatusOfPeople(StatusEnum.superadmin) };
        public ObservableCollection<StatusOfPeople> Statuses
        {
            get
            {
                return statuses;
            }
        }
        private StatusOfPeople selectedPost; // Должность
        public StatusOfPeople StartItem
        {
            get
            {
                return selectedPost;
            }
            set
            {
                selectedPost = value;
                OnPropertyChanged();
            }
        }
        //Должность конец

        //Дата рождения
     

        public DateTime DateOfBorn { get => dateOfBorn; set { dateOfBorn = value; OnPropertyChanged(); } }
        public DateTime StartTime { get { return new DateTime(1941, 6, 22); } set { startTime = value; OnPropertyChanged(); } }
        public DateTime EndTime { get { return new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day); } set { endTime = value; OnPropertyChanged(); } }


        //Дата рождения конец



        private void AllowPass()
        {
            if (pass.Length < minLengthOfPass)
            {

                BedPasswordText = "Слишком короткий пароль!!!";
                PassBorder = Brushes.Red;
                OnPropertyChanged(nameof(BedPasswordText));
                OnPropertyChanged(nameof(PassBorder));
                passAllowed = false;

                ///рамка
            }
            else if (!(pass.Any(ch => char.IsLetter(ch)) && pass.Any(ch => char.IsDigit(ch))))
            {
                BedPasswordText = "Пароль не содержит буквы и цифры!!!";
                PassBorder = Brushes.Red;
                OnPropertyChanged(nameof(PassBorder));
                OnPropertyChanged(nameof(BedPasswordText));
                passAllowed = false;


            }
            else
            {
                BedPasswordText = "";
                PassBorder = Brushes.Gray;
                OnPropertyChanged(nameof(PassBorder));
                OnPropertyChanged(nameof(BedPasswordText));
                passAllowed = true;

            }
        }

        private void AllowPasswordRepeat()
        {
            if (pass!=passrep)
            {
                BedPasswordRepeatText = "Пароли не совпадают!!!";
                PassBorder = Brushes.Red;
                OnPropertyChanged(nameof(PassBorder));
                OnPropertyChanged(nameof(BedPasswordRepeatText));
                passAllowed = false;


            }
            else
            {
                BedPasswordRepeatText = "";
                PassBorder = Brushes.Gray;
                OnPropertyChanged(nameof(PassBorder));
                OnPropertyChanged(nameof(BedPasswordRepeatText));
                passAllowed = true;

            }
        }

        public string Pass
        {
            get
            {
                return pass;
            }
            set
            {
                pass = value;
                AllowPass();
                if (!string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(passrep))
                {
                    AllowPasswordRepeat();
                }
                OnPropertyChanged();
            }
        }
       
        public string Passrep
        {
            get
            {
                return passrep;
            }
            set
            {
                passrep = value;
                AllowPasswordRepeat();
                OnPropertyChanged();
            }
        }
        //регистрация

        public string BedPasswordText { get; set; }
        public string BedPasswordRepeatText { get; set; }

        
        
        public RealiseCommand Registrate
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    object[] objects = obj as object[];
                    foreach (var item in objects)
                    {
                        if (string.IsNullOrWhiteSpace(item.ToString()))
                        {
                            MessageBox.Show("Не все поля заполнены!!!");
                            return;
                        }
                    }
                    string firstName = objects[0].ToString();
                    string secondName = objects[1].ToString();
                    string lastName = objects[2].ToString();
                    string addres = objects[3].ToString();
                    string phoneNumber = objects[4].ToString();
                    string login = objects[5].ToString();
                    string pass = Pass;
                    string secretQuestion = objects[6].ToString();
                    string secretAnswer = objects[7].ToString().ToLower();


                    User.Load();
                    if (User.CheckLoginDublicate(login))
                    {
                        return;
                    }
                    if (pass != Passrep)
                    {
                        MessageBox.Show("Пароли не совпадают!!!");

                        return;
                    }

                    new Log(LogType.registrate, new string[] { login });

                    new User(firstName, secondName, lastName, addres, phoneNumber, login, pass, statuses[0], SettedMale, DateOfBorn, secretQuestion, secretAnswer);
                    MessageBox.Show("Вы успешно зарегистрировались!!!");
                    Form.Create(window, new LoginView());
                }
                , (obj) =>
                {
                    object[] objects = obj as object[];

                    foreach (var item in objects)
                    {
                        if (string.IsNullOrWhiteSpace(item.ToString()) )
                        {

                            return false;
                        }
                    }
                    if (passAllowed == false || string.IsNullOrWhiteSpace(Pass) || string.IsNullOrWhiteSpace(Passrep))
                    {
                        return false;

                    }
                    return true;
                }
                );
            }
        }
        //регистрация конец
        public RealiseCommand GotoLogin
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    Form.Create(window, new LoginView());

                });
            }
        }
        //Ввод телефона


        private static bool ConteinsPluses(string text)
        {
            for (int i = 1; i < text.Length; i++)
            {
                if (text[i]=='+')
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsTextAllowed(string text)
        {
            return !new Regex("[^0-9/+]+").IsMatch(text);
        }
        private string numbers = "";
        public string Numbers
        {
            get => numbers;
            set
            {
                if ((numbers.Length < 11 || value.Length < numbers.Length || (numbers.Length <= 11 && numbers[0] == '+')) && ConteinsPluses(value))
                {
                    numbers = (IsTextAllowed(value) ? value : numbers);

                }
                OnPropertyChanged();
            }
        }


        //Ввод телефона конец


        

    }
}
