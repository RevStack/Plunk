using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevStack.Pattern;
using UnitTest.Models;

namespace UnitTest.Service
{
    public interface ICompanyService : IService<Company, int>
    {
        void DeleteAll();
    }

    public class CompanyService : Service<Company, int>, ICompanyService
    {
        public CompanyService(IRepository<Company, int> repository)
            : base(repository)
        {

        }

        public void DeleteAll()
        {
            var items = _repository.Get();
            foreach (var item in items)
                _repository.Delete(item);
        }
    }
}
