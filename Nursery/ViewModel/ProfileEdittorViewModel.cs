using Nursery.Model;
using Nursery.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Nursery.ViewModel
{
    class ProfileEdittorViewModel : BaseViewModel
    {
        private int indexOfUser;
        private User currentUser;
        private Males male;
        private ObservableCollection<Males> males = new ObservableCollection<Males>() { new Males("Мужской"), new Males("Женский"), new Males("Не определился") };
        public ObservableCollection<Males> Males { get => males; }

        private DateTime startTime;
        private DateTime endTime;
       // private DateTime dateOfBorn;

        public DateTime StartTime { get { return new DateTime(1941, 6, 22); } set { startTime = value; OnPropertyChanged(); } }
        public DateTime EndTime { get { return new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day); } set { endTime = value; OnPropertyChanged(); } }
        public DateTime DateOfBorn { get => currentUser.DateOfBorn; set { currentUser.DateOfBorn = value; OnPropertyChanged(); } }
        private bool inNewWindow;


        public ProfileEdittorViewModel(Window sender, User currentUser, bool innewwindow)
        {
            window = sender;
            this.currentUser = currentUser;
            SettedMale = Males[indexOfMale(currentUser.Male)];
            DateOfBorn = currentUser.DateOfBorn;
            indexOfUser = User.GetIndexOfUser(currentUser);
           
            inNewWindow = innewwindow;
            if (innewwindow==false)
            {
                window.Closing += LogsInClosing;
            }
            else
            {
                window.Closing += CancelChanges;
            }
        }
        public event Action UsersChanged;

        private void LogsInClosing(object sender, CancelEventArgs e)
        {
            new Log(LogType.logout, new string[] { currentUser.Login });
        }
        private void CancelChanges(object sender, CancelEventArgs e)
        {
            User.Load();
            UsersChanged?.Invoke();

        }

        public User UserForEdit
        {
            get
            {
                return currentUser;
            }
        }
        //profile redact

        private int indexOfMale(Males male)
        {
            for (int i = 0; i < Males.Count; i++)
            {
                if (Males[i].male == male.male)
                {
                    return i;
                }
            }
            return -1;
        }
    
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

        private static bool ConteinsPluses(string text)
        {
            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] == '+')
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
        //private string numbers = "";
        public string Numbers
        {
            get => currentUser.PhoneNumber;
            set
            {
                if ((currentUser.PhoneNumber.Length < 11 || value.Length < currentUser.PhoneNumber.Length || (currentUser.PhoneNumber.Length <= 11 && currentUser.PhoneNumber[0] == '+')) && ConteinsPluses(value))
                {
                    currentUser.PhoneNumber = (IsTextAllowed(value) ? value : currentUser.PhoneNumber);

                }
                OnPropertyChanged();
            }
        }
        

        public RealiseCommand EditProfil
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    
                    currentUser.Male = SettedMale;
                    User.Users[indexOfUser] = currentUser;
                    User.Save();
                    if (inNewWindow==true)
                    {
                        UsersChanged?.Invoke();
                    }
                    MessageBox.Show("Изменения применены!!!");
                    new Log(LogType.edittedProfil,new string[] { User.Users[indexOfUser].Login });
                    if (inNewWindow==false)
                    {
                        nextWindow();
                    }
                    else
                    {
                        window.Close();
                    }


                });
            }
        }

        private void nextWindow()
        {
            Window nextWindow;

            switch (User.Users[indexOfUser].Status.StatusEnumValue)
            {
                case StatusEnum.client:
                    nextWindow = new ClientView(User.Users[indexOfUser]);
                    break;
                case StatusEnum.worker:
                    nextWindow = new WorkerView(User.Users[indexOfUser]);

                    break;
                case StatusEnum.adminisrator:
                    nextWindow = new AdminView(User.Users[indexOfUser]);

                    break;
                case StatusEnum.superadmin:
                    nextWindow = new AdminView(User.Users[indexOfUser]);

                    break;
                default:
                    nextWindow = new ClientView(User.Users[indexOfUser]);
                    break;

            }
            window.Closing -= LogsInClosing;
            Form.Create(window, nextWindow);
        }

        public RealiseCommand GoBack
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    User.Load();
                 
                    if (inNewWindow == false)
                    {
                        nextWindow();
                    }
                    else
                    {
                        UsersChanged?.Invoke();

                        window.Close();
                    }
                });
            }
        }
        //profile redact end
    }
}
