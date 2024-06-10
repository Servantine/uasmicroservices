using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServices.DAL.Interfaces
{
    public interface ICrud2<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }
}