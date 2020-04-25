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
        public StatusEnum StatusEnumValue { get; set; }

        public StatusOfPeople() { }
        public StatusOfPeople(StatusEnum statusEnumValue)
        {
            this.StatusEnumValue = statusEnumValue;
        }
        public string Status
        {
            get
            {
                switch (StatusEnumValue)
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
                        StatusEnumValue = StatusEnum.client;
                        break;
                    case "Работник":
                        StatusEnumValue = StatusEnum.worker;

                        break;
                    case "Администратор":
                        StatusEnumValue = StatusEnum.adminisrator;

                        break;
                    case "Господь Бог":
                        StatusEnumValue = StatusEnum.superadmin;
                        break;

                    default:
                        break;
                }
            }
        }
        
    }

}
