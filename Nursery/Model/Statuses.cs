using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursery.Model
{
    
    public class StatusOfPeople
    {
        private StatusEnum _status;
        public StatusEnum _Status { get => _status; set => _status = value; }

        public string Status
        {
            get
            {
                switch (_status)
                {
                    case StatusEnum.client:
                        return "Клиент";
                    case StatusEnum.worker:
                        return "Работник";
                    case StatusEnum.adminisrator:
                        return "Администратор";
                    case StatusEnum.superadmin:
                        return "Господь Бог";
                    default:
                        return "Чтоооооооооооооо?";
                }
            }
            set
            {
                switch (value)
                {
                    case "Клиент":
                        _status = StatusEnum.client;
                        break;
                    case "Работник":
                        _status = StatusEnum.worker;

                        break;
                    case "Администратор":
                        _status = StatusEnum.adminisrator;

                        break;
                    case "Господь Бог":
                        _status = StatusEnum.superadmin;
                        break;

                    default:
                        break;
                }
            }
        }


        public StatusOfPeople()
        {

        }
        public StatusOfPeople(StatusEnum status)
        {
            this._status = status;
        }
    }

}
