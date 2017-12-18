using Entities.GorillaEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDSA2017.Assignment08.UWP.Model.GorillaRestInterfaces
{
    public interface IRestCategoryRepository
    {
        Task<bool> UpdateAsync(CategoryObject categoryObject);
    }
}
