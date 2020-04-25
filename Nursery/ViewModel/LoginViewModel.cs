using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Nursery.Model;
using Nursery.View;
using Nursery.View.PasswordRecovery;

namespace Nursery.ViewModel
{
    internal class LoginViewModel : BaseViewModel
    {
        private string login = "";
        private string pass = "";

        int minpasswordlength = 5;

        internal LoginViewModel(Window caller)
        {
            if (Directory.Exists("Data/") == false)
            {
                Directory.CreateDirectory("Data/");
            }
            window = caller;
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        public RealiseCommand LoginButton => (new RealiseCommand((obj) =>
        {
            User.Load();
            User currentUser = User.SignIn(Login, Pass);
            if (currentUser != null)
            {
                new Log(LogType.login, new string[] { currentUser.Login });

                Window nextWindow;
                // MessageBox.Show($"Здравствуйте {currentUser.FirstName} {currentUser.SecondName} {currentUser.LastName}");
                switch (currentUser.Status.StatusEnumValue)
                {
                    case StatusEnum.client:
                        nextWindow = new ClientView(currentUser);
                        break;
                    case StatusEnum.worker:
                        nextWindow = new WorkerView(currentUser);

                        break;
                    case StatusEnum.adminisrator:
                        nextWindow = new AdminView(currentUser);

                        break;
                    case StatusEnum.superadmin:
                        nextWindow = new AdminView(currentUser);

                        break;
                    default:
                        nextWindow = new ClientView(currentUser);
                        break;

                }
                Form.Create(window, nextWindow);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!!!");
            }


        }, (obj) =>
        {
            if (Login == "" || Pass == "")
            {
                return false;
            }
            return true;
        }));

        //связь с другими окнами
        public RealiseCommand GotoRegistration => (new RealiseCommand((obj) =>
        {

            Form.Create(window, new RegistrationWindow());
        }));

        public RealiseCommand BackToLogin => (new RealiseCommand((obj) =>
        {
            Form.Create(window, new LoginView());
        }));

        public RealiseCommand GotoAbout
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    Window about = new AboutView();
                    about.Owner = window;
                    about.Show();
                });
            }
        }

        //связь с другими окнами конец


        //восстановление пароля
        public RealiseCommand ForgetPassword => (new RealiseCommand((obj) =>
        {
            Window newWindow = new PasswordRecoveryStage1(this);
            Form.Create(window, newWindow);
            window = newWindow;

        }));




        //Этап 1


        private User CheckingUser;
        public RealiseCommand CheckProfilForRecovery => (new RealiseCommand((obj) =>
        {

            User.Load();
            string toCheck = obj.ToString();
            bool finded = false;
            foreach (var item in User.Users)
            {
                if (item.Login == toCheck || item.PhoneNumber == toCheck)
                {
                    Window newWindow = new PasswordRecoveryStage2(this);
                    CheckingUser = item;
                    Form.Create(window, newWindow);
                    window = newWindow;
                    finded = true;
                    break;
                }
            }
            if (!finded)
            {
                MessageBox.Show("Такого пользователя нет!!!");
            }

        }));


        //Этап 2

        public string SecretQuestion
        {
            get
            {
                return CheckingUser.SecretQuestion;
            }
        }

        public RealiseCommand CheckSecretAnswer => (new RealiseCommand((obj) =>
        {

            if (CheckingUser.CheckSecretAnswer(obj.ToString().ToLower()))
            {
                Window newWindow = new PasswordRecoveryStage3(this);
                Form.Create(window, newWindow);
                window = newWindow;
            }
            else
            {
                MessageBox.Show("Неправильный ответ");
            }
        }));

        //Стадия 3
        private string newPass;
        public string NewPass
        {
            get
            {
                return newPass;
            }
            set
            {
                newPass = value;
                OnPropertyChanged();
            }
        }
        private string newPassRep;
        public string NewPassRep
        {
            get
            {
                return newPassRep;
            }
            set
            {
                newPassRep = value;
                OnPropertyChanged();
            }
        }
        public RealiseCommand SwitchPassword => (new RealiseCommand((obj) =>
        {

            if (NewPass == NewPassRep)
            {
                if (NewPass.Length >= minpasswordlength && (NewPass.Any(ch => char.IsLetter(ch)) && NewPass.Any(ch => char.IsDigit(ch))))
                {
                    User.Users[User.GetIndexOfUser(CheckingUser)].SetPassword(NewPass, CheckingUser.Login);
                    User.Save();
                    MessageBox.Show($"{CheckingUser.FirstName}, пароль успешно сменён!!!");
                    Form.Create(window, new LoginView());
                }
                else
                {
                    MessageBox.Show($"{CheckingUser.FirstName}, пароль должен быть не менее 5 символов и содержать как буквы так и цифры!!!");

                }

            }
            else
            {
                MessageBox.Show($"{CheckingUser.FirstName}, пароли не совпадают!!!");

            }

        }, (obj) =>
        {

            if (NewPass == "" || NewPassRep == "")
            {
                return false;
            }
            return true;
        }));


        //восстановление пароля конец
    }
}
