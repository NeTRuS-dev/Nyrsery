using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Nursery.Model
{


    public class Log
    {
        private static ObservableCollection<Log> _logs;
        private static readonly string path = "Data/Logs.xml";
        public string Info { get; set; }

        public static ObservableCollection<Log> LogsList
        {
            get => _logs;
            set => _logs = value;
        }

        public Log() { }
        public Log(LogType type, string[] info)
        {
            Log.Load();
            switch (type)
            {
                case LogType.login:
                    Info = $"{DateTime.Now}: В аккаунт вошёл пользователь {info[0]}";
                    break;
                case LogType.logout:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} вышел из аккаунта";
                    break;
                case LogType.registrate:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} создал аккаунт";
                    break;
                case LogType.createNewPet:
                    Info = $"{DateTime.Now}: Администратор {info[0]} добавил питомца {info[1]}";
                    break;
                case LogType.deleteUser:
                    Info = $"{DateTime.Now}: Администратор {info[0]} удалил профиль пользователя {info[1]}";
                    break;
                case LogType.deletePet:
                    Info = $"{DateTime.Now}: Администратор {info[0]} удалил профиль питомца {info[1]}";
                    break;
                case LogType.userStatusChanged:
                    Info = $"{DateTime.Now}: Администратор {info[0]} изменил статус пользователя {info[1]} на {info[2]}";
                    break;
                case LogType.userDonation:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} пополнил баланс аккаунта на {info[1]}";
                    break;
                case LogType.buyDelicatesForPet:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} оплатил лакомство для питомца по кличке {info[1]}";
                    break;
                case LogType.buyPet:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} приобрёл питомца по кличке {info[1]}";
                    break;
                case LogType.sendToNursery:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} отдал в питомник питомца по кличке {info[1]}";
                    break;
                case LogType.tryToGoToTheJob:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} подал заявку о приёме на работу";
                    break;
                case LogType.giveDelicatesToPet:
                    Info = $"{DateTime.Now}: Служащий {info[0]} выполнил заказ на кормление сладостями питомца по кличке {info[1]}";
                    break;
                case LogType.feedPet:
                    Info = $"{DateTime.Now}: Служащий {info[0]} покормил питомца по кличке {info[1]}";
                    break;
                case LogType.getVactcination:
                    Info = $"{DateTime.Now}: Служащий {info[0]} сделал прививки питомцу по кличке {info[1]}";
                    break;
                case LogType.confirmStatement:
                    Info = $"{DateTime.Now}: Администратор {info[0]} удовлетворил просьбу о принятии на работу пользователя {info[1]}";
                    break;
                case LogType.rejectStatement:
                    Info = $"{DateTime.Now}: Администратор {info[0]} отклонил просьбу о принятии на работу пользователя {info[1]}";
                    break;
                case LogType.feedAllPets:
                    Info = $"{DateTime.Now}: Администратор {info[0]} заказал корм для животных на сумму {info[1]}";
                    break;
                case LogType.buyToysForPets:
                    Info = $"{DateTime.Now}: Администратор {info[0]} заказал игрушки для животных на сумму {info[1]}";
                    break;
                case LogType.giveAdditiveMoney:
                    Info = $"{DateTime.Now}: Администратор {info[0]} выплатил работникам премии на сумму {info[1]}";
                    break;
                case LogType.edittedProfil:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} отредактировал свой профиль";
                    break;
                case LogType.edittedPetProfile:
                    Info = $"{DateTime.Now}: Пользователь {info[0]} отредактировал профиль питомца по кличке {info[1]}";
                    break;
                default:
                    break;
            }
            _logs ??= new ObservableCollection<Log>();
            _logs.Add(this);
            Save();
        }

        public static void Save()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(Log[]));
            using (FileStream fileStram = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStram, _logs.ToArray());
                fileStram.Close();
            }
        }
        public static void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Log[]));
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (serializer.Deserialize(fileStream) is Log[] _logs)
                    {
                        Log._logs = new ObservableCollection<Log>(_logs);
                    }
                    fileStream.Close();
                }

            }

        }
    }
}
