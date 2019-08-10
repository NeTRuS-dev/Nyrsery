using Microsoft.Win32;
using Nursery.Model;
using Nursery.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Nursery.ViewModel
{
    class PetRedactorViewModel:BaseViewModel
    {
        private int indexOfUser;

        private Males male;
        private ObservableCollection<Males> males = new ObservableCollection<Males>() { new Males("Мужской"), new Males("Женский"), new Males("Не определился") };
        public ObservableCollection<Males> Males { get => males; }
        private string oldName;
        private Pet currentPet;
        private int indexOfPet;
        private User currentUser;
        public PetRedactorViewModel(Window window, Pet currentPet, User currentUser)
        {
            this.window = window;
            this.currentPet = currentPet;
            this.currentUser = currentUser;
            indexOfPet = Pet.GetIndexOfPet(currentPet);
            SettedMale = Males[indexOfMale(currentPet.Male)];
            SelectedTypeOfPet = TypesofPets[indexOfType(currentPet.Type)];
            indexOfUser = User.GetIndexOfUser(currentUser);
            ImagePath = currentPet.ImagePath;
            window.Closing += WindowClosing;
            oldName = currentPet.Name;
        }


        private void WindowClosing(object sender, CancelEventArgs e)
        {
            new Log(LogType.logout, new string[] { currentUser.Login });
        }
        //private void LoadPets(object sender, CancelEventArgs e)
        //{
        //    Pet.Load();
        //    PetsChanged?.Invoke();
        //}
        public Pet CurrentPet
        {
            get
            {
                return currentPet;
            }
        }

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

        private int indexOfType(TypeOfPet type)
        {
            for (int i = 0; i < TypesofPets.Count; i++)
            {
                if (TypesofPets[i]._type == type._type)
                {
                    return i;
                }
            }
            return -1;
        }
        public string ImagePath { get; set; }

        public BitmapImage PetIcon
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

                        ImagePath = open_dialog.FileName;
                        OnPropertyChanged(nameof(PetIcon));
                        photoChanged = true;
                    }

                });
            }
        }
        bool photoChanged = false;
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
        private string NameOfPetOnImage(string path)
        {
            int end = 0;
            int start = 0;
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '.')
                {
                    end = i;
                }
                else if (path[i] == '/')
                {
                    start = i;
                }
            }
            return path.Substring(start, end - start);
        }


        public RealiseCommand EditProfil
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    for (int i = 0; i < Pet.pets.Count; i++)
                    {
                        if (currentPet.Name == Pet.pets[i].Name && i != indexOfPet)
                        {
                            MessageBox.Show("Питомец с таким именем уже есть");
                            return;
                        }
                    }
                    string src;

                    currentPet.Male = SettedMale;
                    currentPet.Type = SelectedTypeOfPet;
                    src = TypeOfFile(ImagePath);
                    if (photoChanged == true)
                    {
                        if (File.Exists(currentPet.ImagePath))
                        {
                            File.Delete(currentPet.ImagePath);
                        }
                        File.Copy(ImagePath, $"Data/Pets/{currentPet.Name}{src}", true);
                    }
                    else if (oldName.ToLower() != currentPet.Name.ToLower())
                    {
                        src = TypeOfFile(currentPet.ImagePath);

                        File.Copy(currentPet.ImagePath, $"Data/Pets/{currentPet.Name}{src}", true);
                        if (File.Exists(currentPet.ImagePath))
                        {
                            File.Delete(currentPet.ImagePath);
                        }
                    }

                    currentPet.ImagePath = $"Data/Pets/{currentPet.Name}{src}";
                    Pet.pets[indexOfPet] = currentPet;
                    Pet.Save();
                    MessageBox.Show("Изменения применены!!!");
                    new Log(LogType.edittedPetProfile, new string[] { currentUser.Login, currentPet.Name });

                    nextWindow();



                });
            }
        }

        private void nextWindow()
        {
            Window nextWindow;

            switch (User.users[indexOfUser].Status._Status)
            {
                case StatusEnum.client:
                    nextWindow = new ClientView(User.users[indexOfUser]);
                    break;
                case StatusEnum.worker:
                    nextWindow = new WorkerView(User.users[indexOfUser]);

                    break;
                case StatusEnum.adminisrator:
                    nextWindow = new AdminView(User.users[indexOfUser]);

                    break;
                case StatusEnum.superadmin:
                    nextWindow = new AdminView(User.users[indexOfUser]);

                    break;
                default:
                    nextWindow = new ClientView(User.users[indexOfUser]);
                    break;

            }
            window.Closing -= WindowClosing;
            Form.Create(window, nextWindow);
        }

        public RealiseCommand GoBack
        {
            get
            {
                return new RealiseCommand((obj) =>
                {
                    User.Load();

                    nextWindow();

                });
            }
        }

    }
}
