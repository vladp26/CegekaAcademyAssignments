namespace PetShelterDemo.DAL.Models
{
    public interface IRegistry<T> where T: IEntity
    {
        Task Register(T instance);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetByName(string name);
        Task<IReadOnlyList<T>> Find(Func<T, bool> filter);
    }
}
