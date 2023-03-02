using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using PetShelter.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PetShelter.BusinessLayer.Tests.IntegrationTests
{
    public class PersonIntegrationTests:IDisposable
    {
        private readonly PetShelterContext _petShelterContext;
        private readonly IPersonRepository _personRepositorySut;
        private Person _newPerson;
        private string IdNumber = "1234567890123";
        public PersonIntegrationTests()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PetShelterContext>();
            dbContextOptionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PetShelter4v3;Trusted_Connection=True;TrustServerCertificate=True;");

            _petShelterContext = new PetShelterContext(dbContextOptionsBuilder.Options);
            _personRepositorySut = new PersonRepository(_petShelterContext);
            _newPerson = new Person();
            _newPerson.Name = "Nume test";
            _newPerson.IdNumber= IdNumber;
            _newPerson.DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge);
            _personRepositorySut.Add(_newPerson);
        }
        [Fact]
        public void GivenOnePersonWithIdNumber1234567890123_WhenGetByIdNumber_ReturnsPersonWithIdNumber1234567890123()
        {
            var person = _personRepositorySut.GetPersonByIdNumber(IdNumber);
            person.Should().NotBeNull();

            person.Result.IdNumber.Should().Be(IdNumber);
        }
        public void Dispose()
        {
            _petShelterContext.Persons.Remove(_newPerson);
            _petShelterContext.SaveChangesAsync();
        }
    }
}
