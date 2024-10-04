using WiDocApi_test.Models;

namespace WiDocApi_test.Services
{
    public interface IPersonService
    {
        Task<Person> AddPersonAsync(Person newPerson);
        Task<bool> DeletePersonAsync(int id);
        Task<Person?> GetPersonByIdAsync(int id);
        Task<List<Person>> GetPersonsByLastNameAsync(string lastName, string state);
        Task<Person> UpdatePersonAsync(int id, Person updatedPerson);
        Task<List<string>> GetStateAsync();
        Task<List<string>> GetZipAsync();
    }
}