using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Nursery.Model;
using Nursery.View;
using Nursery.View.AdminFrames;
using Nursery.View.AdminFrames.Helpers;
using System.ComponentModel;
using Nursery.Features;

namespace Nursery.ViewModel
{
    internal class AdminViewModel : BaseViewModel
    {
        private User currentUser;
        private Page users;
        private Page pets;
        private Page buys;
        private Page usersHelper;
        private Page clientsHelper;
        private Page logsHelper;

        private Page petsHelper;
        private Page buysHelper;
        private Page currentPage;
        private Page clientsTab;
        private Page usersTab;
        private Page logsTab;

        public Page ClientsTab { get => clientsTab; set { clientsTab = value; OnPropertyChanged(); } }
        public Page UsersTab { get => usersTab; set { usersTab = value; OnPropertyChanged(); } }
        public Page LogsTab { get => logsTab; set { logsTab = value; OnPropertyChanged(); } }

        public Page CurrentPage { get => currentPage; set { currentPage = value; OnPropertyChanged(); } }
        private Page currentPageHelper;
        public Page CurrentPageHelper { get => currentPageHelper; set { currentPageHelper = value; OnPropertyChanged(); } }
        public BitmapImage PetIcon { get; set; }
        private string PetIconPath;
        private int indexOfTabs = 0;

        public AdminViewModel(Window sender, User currentUser)
        {
            window = sender;
            this.currentUser = currentUser;
            window.Title = $"Здравствуйте, {currentUser.FirstName}";
            users = new UsersControlPanel(this);
            pets = new PetsControlPanel(this);
            buys = new BuysPanel(this);
            clientsHelper = new ClientsHelper(this);
            usersHelper = new UsersHelper(this);
            petsHelper = new PetsHelper(this);
            buysHelper = new BuysHelper(this);
            ClientsTab = new ClientsTab(this);
            UsersTab = new UsersTab(this);
            LogsTab = new LogsTab(this);
            logsHelper = new LogsHelper(this);
            CurrentPage = users;
            CurrentPageHelper = usersHelper;
            FrameOpacity = 1;
            PetIcon = new BitmapImage(new Uri("pack://application:,,,/Nursery;component/Resources/vk-dog-1.jpg"));
            window.Closing += WindowClosing;
            Pet.SelectedPetChanged += PetsChanged;
            logsList = new ObservableCollection<Log>();
            Logs = new MultiSelectCollectionView<Log>(logsList);
            Log.Load();
            for (int i = 0; i < Log.LogsList.Count; i++)
            {
                logsList.Add(Log.LogsList[i]);
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            new Log(LogType.logout, new string[] { currentUser.Login });
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

        private async void NextFrame(Page page, Page pageHelper, bool checkuser)
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
                if (checkuser)
                {
                    CheckTabIndex();
                }
                for (double i = 0; i <= 1; i += 0.1)
                {
                    FrameOpacity = i;

                    Thread.Sleep(50);

                }
            });
        }
        private async void NextFrame(Page pageHelper)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1; i < 0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
                CurrentPageHelper = pageHelper;
                for (double i = 0; i <= 1; i += 0.1)
                {
                    FrameOpacity = i;

                    Thread.Sleep(50);

                }
            });
        }

        private double frameOpacity;
        public double FrameOpacity { get => frameOpacity; set { frameOpacity = value; OnPropertyChanged(); } }

        public RealiseCommand UsersFrame
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    NextFrame(users, usersHelper, true);
                    OnPropertyChanged(nameof(Pets));

                });
            }
        }
        public RealiseCommand PetsFrame
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    NextFrame(pets, petsHelper);
                    OnPropertyChanged(nameof(Pets));

                });
            }
        }
        public RealiseCommand BuysFrame
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    NextFrame(buys, buysHelper);
                    OnPropertyChanged(nameof(Pets));

                });
            }
        }

        //Заявки на работу
        public ObservableCollection<User> ClientsToWorker
        {
            get
            {
                ObservableCollection<User> tmp = new ObservableCollection<User>();

                for (int i = 0; i < User.Users.Count; i++)
                {
                    if (User.Users[i].Status.StatusEnumValue == StatusEnum.client && User.Users[i].WantToBeWorker == true)
                    {
                        tmp.Add(User.Users[i]);
                    }

                }
                return tmp;
            }

        }



        private void CheckTabIndex()
        {
            switch (IndexOfTabs)
            {
                case 1:
                    NextFrame(clientsHelper);
                    break;
                case 0:
                    NextFrame(usersHelper);
                    break;
                case 2:
                    NextFrame(logsHelper);
                    break;
                default:
                    break;
            }
        }



        public int IndexOfTabs
        {
            get => indexOfTabs;
            set
            {
                indexOfTabs = value;
                CheckTabIndex();
                OnPropertyChanged();
            }
        }


        public RealiseCommand ConfirmStatement
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    int indexOfElement = User.GetIndexOfUser(SelectedClient);
                    User.Users[indexOfElement].Status.StatusEnumValue = StatusEnum.worker;
                    User.Users[indexOfElement].WantToBeWorker = false;
                    new Log(LogType.confirmStatement, new string[] { currentUser.Login, User.Users[indexOfElement].Login });
                    OnPropertyChanged(nameof(ClientsToWorker));
                    UsersChanged();

                    User.Save();

                }), ((obj) =>
                {
                    if (SelectedClient == null)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand RejectStatement
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    int indexOfElement = User.GetIndexOfUser(SelectedClient);
                    User.Users[indexOfElement].WantToBeWorker = false;
                    new Log(LogType.rejectStatement, new string[] { currentUser.Login, User.Users[indexOfElement].Login });

                    OnPropertyChanged(nameof(ClientsToWorker));
                    User.Save();

                }), ((obj) =>
                {
                    return SelectedClient != null;
                }));
            }
        }


        //Заявки на работу конец


        //журнал логов
        private Log selectedLog;

        public Log SelectedLog
        {
            get
            {
                return selectedLog;
            }
            set
            {
                selectedLog = value;
                OnPropertyChanged();
            }
        }
        public MultiSelectCollectionView<Log> Logs { get; private set; }
        private ObservableCollection<Log> logsList;
       

        public RealiseCommand DeleteSelectedLog
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    for (int i = 0; i < Logs.SelectedItems.Count; i++)
                    {
                        Log.LogsList.Remove(Logs.SelectedItems[i]);

                    }
                    logsList.Clear();
                    for (int i = 0; i < Log.LogsList.Count; i++)
                    {
                        logsList.Add(Log.LogsList[i]);
                    }
                    OnPropertyChanged(nameof(Logs));
                    Log.Save();
                }), ((obj) =>
                {
                    if (Logs.SelectedItems == null || Logs.SelectedItems.Count==0)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand ClearLogs
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    Log.LogsList.Clear();
                    logsList.Clear();
                    OnPropertyChanged(nameof(Logs));
                    Log.Save();
                }));
            }
        }

        //журнал логов конец


        public ObservableCollection<User> Users
        {
            get
            {
                ObservableCollection<User> tmp = new ObservableCollection<User>();

                for (int i = 0; i < User.Users.Count; i++)
                {
                    if (User.Users[i].Login != currentUser.Login && User.Users[i].Status.StatusEnumValue != StatusEnum.superadmin)
                    {
                        tmp.Add(User.Users[i]);
                    }

                }
                return tmp;
            }

        }

        public ObservableCollection<User> Workers
        {
            get
            {
                ObservableCollection<User> tmp = new ObservableCollection<User>();

                for (int i = 0; i < User.Users.Count; i++)
                {
                    if (User.Users[i].Login != currentUser.Login && (User.Users[i].Status.StatusEnumValue == StatusEnum.worker || User.Users[i].Status.StatusEnumValue == StatusEnum.adminisrator))
                    {
                        tmp.Add(User.Users[i]);
                    }

                }
                return tmp;
            }

        }
        public ObservableCollection<User> Clients
        {
            get
            {
                ObservableCollection<User> tmp = new ObservableCollection<User>();

                for (int i = 0; i < User.Users.Count; i++)
                {
                    if (User.Users[i].Login != currentUser.Login && User.Users[i].Status.StatusEnumValue == StatusEnum.client)
                    {
                        tmp.Add(User.Users[i]);
                    }

                }
                return tmp;
            }

        }

        public RealiseCommand DeleteUser
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    new Log(LogType.deleteUser, new string[] { currentUser.Login, SelectedItem.Login });

                    User.Users.Remove(SelectedItem);
                    SelectedItem = null;
                    UsersChanged();

                    User.Save();

                }), ((obj) =>
                {
                    if (SelectedItem == null)
                    {
                        return false;

                    }
                    if (SelectedItem.Status.StatusEnumValue != StatusEnum.superadmin && currentUser.Status.StatusEnumValue == StatusEnum.superadmin)
                    {
                        return true;
                    }
                    if (SelectedItem.Status.StatusEnumValue == StatusEnum.adminisrator || SelectedItem.Status.StatusEnumValue == StatusEnum.superadmin)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }
        public RealiseCommand UpThisUser
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    int indexOfElement = User.GetIndexOfUser(SelectedItem);
                    User.Users[indexOfElement].Status.StatusEnumValue += 1;
                    new Log(LogType.userStatusChanged, new string[] { currentUser.Login, User.Users[indexOfElement].Login, User.Users[indexOfElement].Status.Status });
                    SelectedItem = null;

                    UsersChanged();
                    User.Save();
                }), ((obj) =>
                {
                    if (SelectedItem == null || (SelectedItem.Status.StatusEnumValue == StatusEnum.adminisrator )  || SelectedItem.Status.StatusEnumValue == StatusEnum.superadmin || (SelectedItem.Status.StatusEnumValue == StatusEnum.client && currentUser.Status.StatusEnumValue != StatusEnum.superadmin))
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand DownThisUser
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    int indexOfElement = User.GetIndexOfUser(SelectedItem);
                    User.Users[indexOfElement].Status.StatusEnumValue -= 1;
                    new Log(LogType.userStatusChanged, new string[] { currentUser.Login, User.Users[indexOfElement].Login, User.Users[indexOfElement].Status.Status });
                    SelectedItem = null;

                    UsersChanged();

                    User.Save();

                }), ((obj) =>
                {
                    if (SelectedItem == null || SelectedItem.Status.StatusEnumValue == StatusEnum.client || SelectedItem.Status.StatusEnumValue == StatusEnum.adminisrator && currentUser.Status.StatusEnumValue != StatusEnum.superadmin || SelectedItem.Status.StatusEnumValue == StatusEnum.superadmin)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        private User selectedClient;
        public User SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                OnPropertyChanged();
            }
        }





        private User selectedItem;
        public User SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }
        //Конец панели пользователей


        //Панель питомцев
        public Pet SelectedPet
        {
            get => Pet.SelectedPet;
            set
            {
                Pet.SelectedPet = value;
                OnPropertyChanged();
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
                    if (SelectedPet!=null)
                    {
                        checkSelect = Pet.pets[i].Name == SelectedPet.Name;
                    }
                    tmp.Add(new PetExtender(Pet.pets[i], checkSelect));
                }
                return tmp;
            }
        }


        public RealiseCommand DeletePet
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {

                    new Log(LogType.deletePet, new string[] { currentUser.Login, SelectedPet.Name });

                    if (File.Exists(SelectedPet.ImagePath))
                    {
                        File.Delete(SelectedPet.ImagePath);
                    }
                  
                    Pet.pets.RemoveAt(Pet.GetIndexOfPet(SelectedPet));
                    SelectedPet = null;
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

        private void PetsChanged()
        {
            OnPropertyChanged(nameof(Pets));
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

        //Панель питомцев конец

        //Регистрация питомца

        private bool vactcina = false;
        public bool Vactcina
        {
            get
            {
                return vactcina;
            }
            set
            {
                vactcina = value;
                OnPropertyChanged();
            }
        }

        private static bool IsTexAllowedOnlyNums(string text)
        {
            return !new Regex("[^0-9]+").IsMatch(text);
        }

        private static bool IsTexAllowedForDecimal(string text)
        {
            return !new Regex("[^0-9.,]+").IsMatch(text);
        }
        private string age = "";
        public string Age
        {
            get => age;
            set
            {
                age = (IsTexAllowedOnlyNums(value) ? value : age);
                OnPropertyChanged();
            }
        }
        private string price = "";
        public string Price
        {
            get => price;
            set
            {
                price = (IsTexAllowedForDecimal(value) ? value : price);
                price = price.Replace('.', ',');


                OnPropertyChanged();
            }
        }

        private ObservableCollection<TypeOfPet> typesofPets = new ObservableCollection<TypeOfPet>() { new TypeOfPet(TypesOfPet.cat), new TypeOfPet(TypesOfPet.dog), new TypeOfPet(TypesOfPet.hamster) };
        public ObservableCollection<TypeOfPet> TypesofPets
        {
            get
            {
                return typesofPets;
            }
        }

        private TypeOfPet selectedTypeOfPet;
        public TypeOfPet SelectedTypeOfPet
        {
            get
            {
                return selectedTypeOfPet;

            }
            set
            {
                selectedTypeOfPet = value;
                OnPropertyChanged();
            }
        }

        public RealiseCommand AddPet
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    Window addPetter = new PetRegistration(this);
                    addPetter.Owner = window;
                    addPetter.Show();
                });
            }
        }

        private Males male;
        private ObservableCollection<Males> males = new ObservableCollection<Males>() { new Males("Мужской"), new Males("Женский"), new Males("Не определился") };
        public ObservableCollection<Males> Males { get => males; }

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

        private string TypeOfFile(string path)
        {
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '.')
                {
                    return path.Substring(i);
                }
            }
            return ".png";
        }


        public RealiseCommand RegistratePet
        {
            get
            {
                return new RealiseCommand((obj) =>
                {

                    object[] objects = obj as object[];

                    if (string.IsNullOrWhiteSpace(objects[0].ToString())  || SelectedTypeOfPet == null || SettedMale == null || string.IsNullOrWhiteSpace(Price) || string.IsNullOrWhiteSpace(Age))
                    {
                        MessageBox.Show("Не все поля заполнены!!!");
                        return;
                    }

                    string Name = objects[0].ToString();
                    string Description = objects[1].ToString();

                    Pet.Load();
                    if (Pet.CheckLoginDublicate(Name))
                    {
                        MessageBox.Show("Питомец с таким именем уже есть!!!");

                        return;
                    }
                    Directory.CreateDirectory("Data/Pets/");
                    string src;
                    if (!string.IsNullOrWhiteSpace(PetIconPath))
                    {
                        File.Copy(PetIconPath, $"Data/Pets/{Name}{TypeOfFile(PetIconPath)}");
                        src = TypeOfFile(PetIconPath);
                    }
                    else
                    {
                        var exePath = Environment.CurrentDirectory;
                        int startindex = exePath.Length - 1;

                        for (int i = 0; i < 2; i++)
                        {
                            startindex = exePath.LastIndexOf("\\", startindex);
                            startindex--;
                        }
                        File.Copy(exePath.Substring(0, startindex + 1) + "\\Resources\\vk-dog-1.jpg", $"Data/Pets/{Name}{".jpg"}");
                        src = ".jpg";
                    }

                    new Log(LogType.createNewPet, new string[] { currentUser.Login, Name });

                    new Pet(Name, Description, int.Parse(Age), decimal.Parse(Price), Vactcina, SelectedTypeOfPet, SettedMale, $"Data/Pets/{Name}{src}");
                    Name = "";
                    Description = "";
                    Age = "";
                    Price = "";
                    Vactcina = false;
                    SelectedTypeOfPet = null;
                    SettedMale = null;
                    PetIcon = new BitmapImage(new Uri("pack://application:,,,/Nursery;component/Resources/vk-dog-1.jpg"));
                    OnPropertyChanged(nameof(PetIcon));
                    PetIconPath = "";
                    OnPropertyChanged(nameof(Pets));
                    MessageBox.Show("Питомец успешно добавлен!!!");
                    for (int i = 0; i < window.OwnedWindows.Count; i++)
                    {
                        window.OwnedWindows[i].Close();
                    }
                }
                );
            }
        }

        public RealiseCommand OpenImage
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    OpenFileDialog open_dialog = new OpenFileDialog();
                    BitmapImage bitmap = new BitmapImage();

                    open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
                    if (open_dialog.ShowDialog() == true) //если в окне была нажата кнопка "ОК"
                    {
                        try
                        {
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(open_dialog.FileName, UriKind.RelativeOrAbsolute);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"Невозможно открыть выбранный файл {e.Message}",
                            "О боже!!!");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(open_dialog.FileName))
                    {
                        PetIcon = bitmap;
                        OnPropertyChanged(nameof(PetIcon));
                        PetIconPath = open_dialog.FileName;

                    }

                });
            }
        }

        

        //Регистрация питомца конец



        //Панель покупок
        public string OnBalanceOrganization
        {
            get
            {
                return $"На счету фирмы {currentUser.Money} VKcoins";
            }
        }

        private string whantToAdd = "";
        public string WhantToAdd
        {
            get => whantToAdd;
            set
            {
                whantToAdd = (IsTexAllowedForDecimal(value) ? value : whantToAdd);
                OnPropertyChanged();
            }
        }


        public RealiseCommand UpBalance
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    if (BigInteger.Parse(currentUser.Money.ToString()) + BigInteger.Parse((WhantToAdd.Replace('.', ','))) > BigInteger.Parse(decimal.MaxValue.ToString()))
                    {
                        currentUser.Money = decimal.MaxValue;
                    }
                    else
                    {
                        currentUser.Money += decimal.Parse(WhantToAdd.Replace('.', ','));
                    }

                    OnPropertyChanged(nameof(OnBalanceOrganization));
                    WhantToAdd = "";
                    OnPropertyChanged(nameof(WhantToAdd));

                    User.Save();
                }), (obj) =>
                {
                    if (string.IsNullOrWhiteSpace(WhantToAdd))
                    {
                        return false;
                    }
                    return true;
                });
            }
        }

        //основные кнопки покупок

        private string wantToPay = "";
        public string WantToPay
        {
            get => wantToPay;
            set
            {
                wantToPay = (IsTexAllowedForDecimal(value) ? value : wantToPay);

                OnPropertyChanged();
            }
        }

        public RealiseCommand UniversalPay
        {
            get
            {
                return new RealiseCommand(((obj) =>
                {
                    int currentIndex = User.GetIndexOfUser(currentUser);
                    User.Users[currentIndex].Money -= decimal.Parse(WantToPay.Replace('.', ','));


                    User.Save();
                    switch (int.Parse(obj.ToString()))
                    {
                        case 1:
                            MessageBox.Show("Теперь зверушки будут сыты!!!");
                            for (int i = 0; i < Pet.pets.Count; i++)
                            {
                                Pet.pets[i].LastTimeOfEating = DateTime.Now;
                                Pet.Save();
                                OnPropertyChanged(nameof(Pets));
                            }
                            new Log(LogType.feedAllPets, new string[] { currentUser.Login, (WantToPay.Replace('.', ',')) });
                            break;
                        case 2:
                            MessageBox.Show("Теперь зверушки будут веселее!!!");
                            new Log(LogType.buyToysForPets, new string[] { currentUser.Login, (WantToPay.Replace('.', ',')) });

                            break;
                        case 3:
                            MessageBox.Show("Работники вам благодарны!!!");
                            decimal forEveryone = decimal.Parse(WantToPay.Replace('.', ',')) / User.Users.Where(i => i.Status.StatusEnumValue == StatusEnum.worker).ToList().Count;
                            for (int i = 0; i < User.Users.Count; i++)
                            {
                                if (User.Users[i].Status.StatusEnumValue == StatusEnum.worker)
                                {
                                    User.Users[i].Money += forEveryone;
                                }
                            }
                            new Log(LogType.giveAdditiveMoney, new string[] { currentUser.Login, (WantToPay.Replace('.', ',')) });

                            break;
                        default:
                            MessageBox.Show("Вы сделали хоть что-то!!!");

                            break;
                    }
                    WantToPay = "";
                    OnPropertyChanged(nameof(WantToPay));
                    OnPropertyChanged(nameof(OnBalanceOrganization));


                }), ((obj) =>
                {
                    decimal tmp;
                    if (decimal.TryParse(WantToPay.Replace('.', ','), out tmp) == false || decimal.Parse(WantToPay.Replace('.', ',')) > currentUser.Money)
                    {
                        return false;
                    }
                    return true;
                }));
            }
        }

        public RealiseCommand GoToEditter//Редактировать профиль
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
        private void UsersChanged()
        {
            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(Workers));

        }
        public RealiseCommand RedactThis//Редактировать профиль
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                  
                    var redactor = new ProfileRedactor();
                    var redactorDC = new ProfileEdittorViewModel(redactor, selectedItem, true);
                    redactorDC.UsersChanged += UsersChanged;
                    redactor.DataContext = redactorDC;
                    redactor.Owner = window;
                    redactor.Show();

                },(obj)=> {
                    if (selectedItem==null)
                    {
                        return false;
                    }
                    return true;
                });
            }
        }






    }
}
