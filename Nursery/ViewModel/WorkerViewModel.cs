using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Nursery.Model;
using Nursery.View;

namespace Nursery.ViewModel
{
    internal class WorkerViewModel : BaseViewModel
    {
        private User currentUser;
        internal WorkerViewModel(Window sender, User currentUser)
        {
            window = sender;
            this.currentUser = currentUser;
            window.Title = $"Здравствуйте, {currentUser.FirstName}";
            window.Closing += WindowClosing;

        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            new Log(LogType.logout, new string[] { currentUser.Login });
        }

        public RealiseCommand BackToLogin => (new RealiseCommand((obj) =>
        {
            Form.Create(window, new LoginView());
        }));

        private string SalaryDaysCounter()
        {
            int day = ((26 - DateTime.Now.Day > 0) ? 26 - DateTime.Now.Day : DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day + 26);
            string ret = "";
            int tmp = day % 10;
            if (tmp == 0 || tmp >= 5 && tmp <= 9 || day >= 11 && day <= 19)
            {
                ret = $"{day} дней";
            }
            else if (tmp == 1)
            {
                ret = $"{day} день";

            }
            else if (tmp >= 2 && tmp <= 4)
            {
                ret = $"{day} дня";

            }
            return ret;
        }

        public string DaysToSalary
        {
            get
            {
                return $"Денег нет, но вы держитесь ещё {SalaryDaysCounter()}.";
            }
        }


        private Pet selectedPet;
        public Pet SelectedPet
        {
            get => selectedPet;
            set
            {
                selectedPet = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Pet> Pets
        {
            get
            {
                var tmp = new ObservableCollection<Pet>();
                Pet.Load();
                for (int i = 0; i < Pet.Pets.Count; i++)
                {
                    tmp.Add(Pet.Pets[i]);
                }
                return tmp;
            }
        }


        public RealiseCommand GiveDelicacy
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    SelectedPet.PayedEating = false;
                    new Log(LogType.giveDelicatesToPet, new string[] { currentUser.Login, SelectedPet.Name });

                    SelectedPet.LastTimeOfEating = DateTime.Now;
                    Pet.Save();
                    OnPropertyChanged(nameof(Pets));



                }), ((obj) =>
                {

                    if (SelectedPet == null || SelectedPet.PayedEating == false)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand ToFeed
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {

                    SelectedPet.LastTimeOfEating = DateTime.Now;
                    Pet.Save();
                    new Log(LogType.feedPet, new string[] { currentUser.Login, SelectedPet.Name });

                    OnPropertyChanged(nameof(Pets));

                }), ((obj) =>
                {

                    if (SelectedPet == null)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }
        public RealiseCommand GetVaccination
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {

                    if (SelectedPet.Vactinade == true)
                    {
                        MessageBox.Show("Все прививки уже сделаны!!!");
                        return;
                    }
                    new Log(LogType.getVactcination, new string[] { currentUser.Login, SelectedPet.Name });

                    Pet.Pets[Pet.Pets.IndexOf(SelectedPet)].Vactinade = true;
                    Pet.Save();

                    OnPropertyChanged(nameof(Pets));

                }), ((obj) =>
                {

                    if (SelectedPet == null)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand RedactPet//Редактировать профиль
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    window.Closing -= WindowClosing;

                    Form.Create(window, new PetRedactor(currentUser, SelectedPet));

                }, (obj) => {
                    if (SelectedPet == null)
                    {
                        return false;
                    }
                    return true;
                });
            }
        }


        public RealiseCommand GoToEditter //редактирование профиля
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    window.Closing -= WindowClosing;

                    Form.Create(window, new ProfileRedactor(currentUser));

                });
            }
        }

    }
}
