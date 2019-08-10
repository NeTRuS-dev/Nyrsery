using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursery.Model
{
   

    public class TypeOfPet
    {
        public TypesOfPet _type;
        public TypeOfPet()
        {

        }
        public string PetType
        {
            get
            {
                switch (_type)
                {
                    case TypesOfPet.cat:
                        return "Кот";
                    case TypesOfPet.dog:
                        return "Собака";
                    case TypesOfPet.hamster:
                        return "Хомячёчек";
                    default:
                        return "Чтоооооооооооооо?";
                }
            }
            set
            {
                switch (value)
                {
                    case "Кот":
                        _type = TypesOfPet.cat;
                        break;
                    case "Собака":
                        _type = TypesOfPet.dog;

                        break;
                    case "Хомячёчек":
                        _type = TypesOfPet.hamster;

                        break;
                    default:
                        break;
                }
            }
        }

        public TypeOfPet(TypesOfPet type)
        {
            _type = type;
        }
    }
}
