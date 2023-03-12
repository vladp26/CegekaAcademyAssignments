using PetShelter.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.BusinessLayer.Builder
{
    public class PersonBuilder:IPersonBuilder
    {
        private Person _person=new Person();
        public PersonBuilder() {
            this.Reset();
        }
        public void Reset()
        {
            this._person = new Person();
        }

        public void AddPersonName(string name)
        {
            this._person.Name = name;
        }

        public void AddPersonIdNumber(string idNumber)
        {
           this._person.IdNumber = idNumber;
        }

        public void AddPersonDateOfBirth(DateTime dateOfBirth)
        {
            this._person.DateOfBirth = dateOfBirth;
        }
        public Person GetPerson()
        {
            Person result = this._person;

            this.Reset();

            return result;
        }

    }
}
