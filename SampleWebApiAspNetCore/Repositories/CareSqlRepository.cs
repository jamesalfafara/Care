using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CareSqlRepository : ICareRepository
    {
        private readonly CareDbContext _careDbContext;

        public CareSqlRepository(CareDbContext careDbContext)
        {
            _careDbContext = careDbContext;
        }

        public CareEntity GetSingle(int id)
        {
            return _careDbContext.CareItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(CareEntity item)
        {
            _careDbContext.CareItems.Add(item);
        }

        public void Delete(int id)
        {
            CareEntity careItem = GetSingle(id);
            _careDbContext.CareItems.Remove(careItem);
        }

        public CareEntity Update(int id, CareEntity item)
        {
            _careDbContext.CareItems.Update(item);
            return item;
        }

        public IQueryable<CareEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<CareEntity> _allItems = _careDbContext.CareItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Effectiveness.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _careDbContext.CareItems.Count();
        }

        public bool Save()
        {
            return (_careDbContext.SaveChanges() >= 0);
        }

        public ICollection<CareEntity> GetRandomCare()
        {
            List<CareEntity> toReturn = new List<CareEntity>();

            return toReturn;
        }

        private CareEntity GetRandomItem(string type)
        {
            return _careDbContext.CareItems
                .Where(x => x.Product == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
