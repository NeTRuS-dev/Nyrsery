using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Nursery.Model;
using Nursery.View.ClientFrames.Helpers;
using Nursery.View.ClientFrames;
using System.Threading;
using System.Text.RegularExpressions;
using System.Numerics;
using Nursery.View;
using System.ComponentModel;

namespace Nursery.ViewModel
{
    internal class ClientViewModel : BaseViewModel
    {
        private Page allPets;
        private Page myPets;
        private Page allPetsHelper;
        private Page myPetsHelper;
        private Page currentPage;
        public Page CurrentPage { get => currentPage; set { currentPage = value; OnPropertyChanged(); } }
        private Page currentPageHelper;
        public Page CurrentPageHelper { get => currentPageHelper; set { currentPageHelper = value; OnPropertyChanged(); } }
        private double frameOpacity;
        public double FrameOpacity { get => frameOpacity; set { frameOpacity = value; OnPropertyChanged(); } }

        private User currentUser;
        public ClientViewModel(Window sender, User currentUser)
        {
            window = sender;
            this.currentUser = currentUser;
            window.Title = $"Здравствуйте, {currentUser.FirstName}";
            allPets = new AllPets(this);
            myPets = new MyPets(this);
            allPetsHelper = new AllPetsHelper(this);
            myPetsHelper = new MyPetsHelper(this);
            CurrentPage = allPets;
            CurrentPageHelper = allPetsHelper;
            FrameOpacity = 1;
            Pet.SelectedPetChanged += PetPropetryChanged;
            window.Closing += WindowClosing;

        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            new Log(LogType.logout, new string[] { currentUser.Login });
        }

        public void PetPropetryChanged()
        {
            OnPropertyChanged(nameof(SelectedPet));
            OnPropertyChanged(nameof(SayAboutFoodCost));
            OnPropertyChanged(nameof(Pets));


        }


        public RealiseCommand BackToLogin => (new RealiseCommand((obj) =>
        {
            Form.Create(window, new LoginView());
        }));


        private async void NextFrame(Page page, Page pageHelper)
        {
            Pet.SelectedPet = null;

            await Task.Factory.StartNew(() =>
            {
                for (double i = 1; i < 0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
                CurrentPage = page;
                CurrentPageHelper = pageHelper;
                for (double i = 0; i <= 1; i += 0.1)
                {
                    FrameOpacity = i;

                    Thread.Sleep(50);

                }
            });
        }

        public RealiseCommand PetsFrame
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    NextFrame(allPets, allPetsHelper);

                    SelectedPet = null;
                    OnPropertyChanged(nameof(Pets));

                });
            }
        }

        public RealiseCommand MyPetsFrame
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    NextFrame(myPets, myPetsHelper);
                    SelectedPet = null;
                    OnPropertyChanged(nameof(Pets));

                });
            }
        }

        public Pet SelectedPet
        {
            get => Pet.SelectedPet;
            set
            {
                Pet.SelectedPet = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SayAboutFoodCost));

            }
        }

        public ObservableCollection<PetExtender> Pets
        {
            get
            {
                var tmp = new ObservableCollection<PetExtender>();
                Pet.Load();
                for (int i = 0; i < Pet.pets.Count; i++)
                {
                    bool checkSelect = false; ;
                    if (SelectedPet != null)
                    {
                        checkSelect = Pet.pets[i].Name == SelectedPet.Name;
                    }
                    tmp.Add(new PetExtender(Pet.pets[i], checkSelect));
                }
                return tmp;
            }
        }


        public string SayAboutFoodCost
        {
            get
            {
                if (SelectedPet == null)
                {
                    return "";
                }
                else
                {
                    return $"Вы можете заказать еду для этого животного за {SelectedPet.Price * 0.1m} VKcoin'ов";
                }
            }
        }

        public RealiseCommand BuyFoodForPet
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {

                    if (SelectedPet.Price > currentUser.Money)
                    {
                        MessageBox.Show("У вас недостаточно денег на счету!!!");
                    }
                    else if (SelectedPet.PayedEating == true)
                    {
                        MessageBox.Show("Вас опередили!!! Этому питомцу уже заказали лакомство.");

                    }
                    else
                    {
                        currentUser.Money -= SelectedPet.Price * 0.1m;
                        for (int i = 0; i < User.Users.Count; i++)
                        {
                            if (User.Users[i].Status.StatusEnumValue == StatusEnum.adminisrator || User.Users[i].Status.StatusEnumValue == StatusEnum.superadmin)
                            {
                                User.Users[i].Money += SelectedPet.Price * 0.1m;
                                break;
                            }
                        }
                        Pet.pets[Pet.GetIndexOfPet(SelectedPet)].PayedEating = true;
                        new Log(LogType.buyDelicatesForPet, new string[] { currentUser.Login, SelectedPet.Name });
                        MessageBox.Show($"Вы успешно заказали лакомство для питомца по кличке {SelectedPet.Name}.");
                        SelectedPet = null;

                        Pet.Save();
                        OnPropertyChanged(nameof(Balance));
                        OnPropertyChanged(nameof(Pets));
                    }


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


        public RealiseCommand GetaJob
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    //currentUser.WantToBeWorker = true;
                    if (currentUser.WantToBeWorker != true)
                    {
                        if (MessageBox.Show($"Добро пожаловать, {currentUser.FirstName} {currentUser.LastName}!" +
                                " Мы всегда рады новым сотрудникам,\n" +
                                "Вы хотите подать заявку прямо сейчас?", "Хотите подать заявление на работу?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            User.Users[User.GetIndexOfUser(currentUser)].WantToBeWorker = true;
                            // currentUser.WantToBeWorker = true;
                            new Log(LogType.tryToGoToTheJob, new string[] { currentUser.Login });
                            User.Save();
                            MessageBox.Show("Ваша заявка будет рассмотрена в ближайшее время. Мы вам перезвоним");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы уже подали заявление");

                    }

                }));
            }
        }

        public RealiseCommand BuyPet
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    if (SelectedPet.Price > currentUser.Money)
                    {
                        MessageBox.Show("У вас недостаточно денег на счету!!!");
                    }
                    else
                    {
                        User.Users[User.GetIndexOfUser(currentUser)].Money -= SelectedPet.Price;
                        for (int i = 0; i < User.Users.Count; i++)
                        {
                            if (User.Users[i].Status.StatusEnumValue == StatusEnum.adminisrator || User.Users[i].Status.StatusEnumValue == StatusEnum.superadmin)
                            {
                                User.Users[i].Money += SelectedPet.Price;
                                break;
                            }
                        }
                        User.Users[User.GetIndexOfUser(currentUser)].AddPet(SelectedPet);
                        new Log(LogType.buyPet, new string[] { currentUser.Login, SelectedPet.Name });
                        Pet.pets.RemoveAt(Pet.GetIndexOfPet(SelectedPet));
                        SelectedPet = null;
                        Pet.Save();
                        OnPropertyChanged(nameof(Balance));
                        OnPropertyChanged(nameof(Pets));
                        OnPropertyChanged(nameof(MyPets));
                    }



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

        public RealiseCommand GiveAwayThePet
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    Pet.pets.Add(SelectedMyPet);
                    new Log(LogType.sendToNursery, new string[] { currentUser.Login, SelectedMyPet.Name });

                    User.Users[User.GetIndexOfUser(currentUser)].MyPets.Remove(SelectedMyPet);
                    User.Save();
                    Pet.Save();
                    OnPropertyChanged(nameof(Pets));
                    OnPropertyChanged(nameof(MyPets));

                }, (obj) =>
                {

                    if (SelectedMyPet == null)
                    {
                        return false;
                    }
                    return true;
                });
            }
        }

        //Пополнение счёта
        public string Balance
        {
            get
            {
                return $"На вашем счету : {currentUser.Money.ToString()} VKcoins";
            }
        }

        private static bool IsTexAllowedForDecimal(string text)
        {
            return !new Regex("[^0-9.,]+").IsMatch(text);
        }

        private string willBeAdded = "";
        public string WillBeAdded
        {
            get => willBeAdded;
            set
            {
                willBeAdded = (IsTexAllowedForDecimal(value) ? value : willBeAdded);
                OnPropertyChanged();
            }
        }


        public RealiseCommand AddMoney => new RealiseCommand(((obj) =>
        {
            if (BigInteger.Parse(currentUser.Money.ToString()) + BigInteger.Parse((WillBeAdded.Replace('.', ','))) > BigInteger.Parse(decimal.MaxValue.ToString()))
            {
                currentUser.Money = decimal.MaxValue;
            }
            else
            {
                currentUser.Money += decimal.Parse(WillBeAdded.Replace('.', ','));
            }
            new Log(LogType.userDonation, new string[] { currentUser.Login, WillBeAdded.Replace('.', ',') });

            OnPropertyChanged(nameof(Balance));
            WillBeAdded = "";
            OnPropertyChanged(nameof(WillBeAdded));
            User.Save();
        }), ((obj) =>
        {

            if (string.IsNullOrWhiteSpace(WillBeAdded))
            {
                return false;
            }
            return true;
        }));

        //Пополнение счёта end




        //Мои питомцы

        private Pet selectedMyPet;
        public Pet SelectedMyPet
        {
            get => selectedMyPet;
            set
            {
                selectedMyPet = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Pet> MyPets
        {
            get
            {
                var tmp = new ObservableCollection<Pet>();
                for (int i = 0; i < currentUser.MyPets.Count; i++)
                {
                    tmp.Add(currentUser.MyPets[i]);
                }
                return tmp;
            }
        }

        //profile redact




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


        //profile redact end


    }
}
