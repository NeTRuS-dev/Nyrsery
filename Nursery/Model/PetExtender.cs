using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursery.Model
{
    class PetExtender:Pet
    {
        public bool Selected { get; set; }
      
        public PetExtender(Pet pet,bool selected)
        {
            this.Name = pet.Name;
            this.Decription = pet.Decription;
            this.Age = pet.Age;
            this.Price = pet.Price;
            this.Vactinade = pet.Vactinade;
            this.Type = pet.Type;
            this.Male = pet.Male;
            this.LastTimeOfEating = pet.LastTimeOfEating;
            this.PayedEating = pet.PayedEating;
            this.ImagePath = pet.ImagePath;

            this.Selected = selected;

        }
    }
}
