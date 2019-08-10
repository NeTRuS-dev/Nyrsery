using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursery.Model
{
    
    public class Males
    {
        public MaleEnum male;
        public string Male
        {
            get
            {
                switch (male)
                {
                    case MaleEnum.female:
                        return "Женский";

                    case MaleEnum.male:
                        return "Мужской";
                    case MaleEnum.trans:
                        return "Не определился";
                    default:
                        return "Чтоооооооооооооо?";
                }
            }
            set
            {
                switch (value)
                {
                    case "Женский":
                        male = MaleEnum.female;
                        break;
                    case "Мужской":
                        male = MaleEnum.male;
                        break;
                    case "Не определился":
                        male = MaleEnum.trans;
                        break;
                    default:
                        break;
                }
            }
        }
        public Males()
        {

        }
        public Males(string male)
        {
            Male = male;

        }
    }
}

