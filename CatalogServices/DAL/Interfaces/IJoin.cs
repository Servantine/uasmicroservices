using CatalogServices.Models;

namespace CatalogServices.DAL.Interfaces
{
    public interface IJoin : ICrud2<Join>
    {
        IEnumerable<Join> GetByName(string name);
    }
}