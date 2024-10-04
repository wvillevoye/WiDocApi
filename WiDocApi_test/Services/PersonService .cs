using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection;
using WiDocApi_test.Models;

namespace WiDocApi_test.Services
{
    public class PersonService : IPersonService
    {
        private readonly SamplePersonsContext _dbContext;  // Your DbContext class

        public PersonService(SamplePersonsContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: Add a new person
        public async Task<Person> AddPersonAsync(Person newPerson)
        {
            _dbContext.Persons.Add(newPerson);
            await _dbContext.SaveChangesAsync();
            return newPerson;
        }

        // PUT: Update an existing person by ID
        public async Task<Person> UpdatePersonAsync(int id, Person updatedPerson)
        {
            var existingPerson = await _dbContext.Persons.FindAsync(id);
            if (existingPerson != null)
            {
                if (!string.IsNullOrEmpty(updatedPerson.FirstName))
                    existingPerson.FirstName = updatedPerson.FirstName;

                if (!string.IsNullOrEmpty(updatedPerson.LastName))
                    existingPerson.LastName = updatedPerson.LastName;

                if (updatedPerson.Age.HasValue)
                    existingPerson.Age = updatedPerson.Age;

                if (!string.IsNullOrEmpty(updatedPerson.Gender))
                    existingPerson.Gender = updatedPerson.Gender;

                if (!string.IsNullOrEmpty(updatedPerson.Email))
                    existingPerson.Email = updatedPerson.Email;

                if (!string.IsNullOrEmpty(updatedPerson.State))
                    existingPerson.State = updatedPerson.State;

                if (!string.IsNullOrEmpty(updatedPerson.ZipCode))
                    existingPerson.ZipCode = updatedPerson.ZipCode;

                await _dbContext.SaveChangesAsync();
                return existingPerson;
            }
            return new Person(); // or throw an exception
        }

        // DELETE: Delete a person by ID
        public async Task<bool> DeletePersonAsync(int id)
        {
            var person = await _dbContext.Persons.FindAsync(id);
            if (person != null)
            {
                _dbContext.Persons.Remove(person);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Optional: Get a person by ID
        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return await _dbContext.Persons.FindAsync(id);
        }

        // Optional: Get persons by last name starting with a given string
        public async Task<List<Person>> GetPersonsByLastNameAsync(string lastName, string state)
        {
           
            var _person = await _dbContext.Persons
            .Where(x => x.LastName.ToLower().StartsWith(lastName.ToLower().Trim())
                && x.State.ToLower().Trim().StartsWith(state.ToLower().Trim()))
            .ToListAsync();

            return _person;
        }

        public async Task<List<string>> GetStateAsync()
        {
            var result = await _dbContext.Persons
                         .AsNoTracking()
                         .Select(x => x.State)
                         .Distinct()
                         .ToListAsync();

           
            return result;

        }
        public async Task<List<string>> GetZipAsync()
        {
            var result = await _dbContext.Persons
                         .AsNoTracking()
                         .Select(x => x.ZipCode)
                         .Distinct()
                         .ToListAsync();


            return result;

        }



    }
    
}