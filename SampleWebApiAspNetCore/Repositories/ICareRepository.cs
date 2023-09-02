using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface ICareRepository
    {
        CareEntity GetSingle(int id);
        void Add(CareEntity item);
        void Delete(int id);
        CareEntity Update(int id, CareEntity item);
        IQueryable<CareEntity> GetAll(QueryParameters queryParameters);
        ICollection<CareEntity> GetRandomCare();
        int Count();
        bool Save();
    }
}
