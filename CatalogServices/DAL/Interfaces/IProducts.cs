using CatalogServices.DTO;
using CatalogServices.Models;

namespace CatalogServices.DAL.Interfaces
{
    public interface IProducts : ICrud<Product>
    {
        IEnumerable<Product> GetByName(string name);

        void UpdateStockAfterOrder(ProductsUpdateStockDto productsUpdateStockDto);
    }
}