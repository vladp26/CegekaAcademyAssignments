using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.BusinessLayer.Builder
{
    public class PersonDirector
    {
        private IPersonBuilder _personBuilder;

        public IPersonBuilder PersonBuilder
        {
            set { _personBuilder = value; }
        }
        public void BuildPersonWithNameAndIdNumberOnly(string name, string idNumber)
        {
            this._personBuilder.AddPersonName(name);
            this._personBuilder.AddPersonIdNumber(idNumber);
        }

        public void BuildPersonWithAllProperties(string name, string idNumber, DateTime dateOfBirth)
        {
            this._personBuilder.AddPersonName(name);
            this._personBuilder.AddPersonIdNumber(idNumber);
            this._personBuilder.AddPersonDateOfBirth(dateOfBirth);
        }
    }
}
