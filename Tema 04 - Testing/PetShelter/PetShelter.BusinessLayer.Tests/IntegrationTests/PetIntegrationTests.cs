using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using PetShelter.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetShelter.BusinessLayer.ExternalServices;
using PetShelter.BusinessLayer.Validators;
using PetShelter.BusinessLayer.Models;
using FluentAssertions;
using System.Globalization;

namespace PetShelter.BusinessLayer.Tests.IntegrationTests
{
    public class PetIntegrationTests : IDisposable
    {
        private readonly PetShelterContext _petShelterContext;
        private readonly IPetRepository _petRepositorySut;
        private Pet _newPet;
        private string name = "Max";
        public PetIntegrationTests()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PetShelterContext>();
            dbContextOptionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PetShelter4v3;Trusted_Connection=True;TrustServerCertificate=True;");

            _petShelterContext = new PetShelterContext(dbContextOptionsBuilder.Options);
            _petRepositorySut = new PetRepository(_petShelterContext);
            _newPet = new Pet();
            _newPet.Name = name;
            _newPet.Type = Constants.PetType.Dog.ToString();
            _newPet.Description = "Nice dog";
            _newPet.IsHealthy = true;
            _newPet.ImageUrl = "test";
            _newPet.WeightInKg = 10;
            _petRepositorySut.Add(_newPet);
        }
        [Fact]
        public void GivenAtLeastOnePetWithNameMax_WhenGetByName_ReturnsPetWithNameMax()
        {
            var pet = _petRepositorySut.GetPetByName(name);
            pet.Should().NotBeNull();
            
            pet.Result.Name.Should().Be(name);
        }
        public void Dispose()
        {
            _petShelterContext.Pets.Remove(_newPet);
            _petShelterContext.SaveChangesAsync();
        }
    }
}
