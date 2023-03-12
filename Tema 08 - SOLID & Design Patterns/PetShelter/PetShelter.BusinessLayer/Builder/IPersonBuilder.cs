using PetShelter.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.BusinessLayer.Builder
{
    public interface IPersonBuilder
    {
        void AddPersonName(string name);

        void AddPersonIdNumber(string idNumber);

        void AddPersonDateOfBirth(DateTime dateOfBirth);
        public Person GetPerson();
    }
}
