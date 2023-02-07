using HumanResourceAPI.Contexts;
using HumanResourceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceAPI.Repositories
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : AppDbContext
    {
        private readonly AppDbContext context;
        private readonly DbSet<Entity> entities;
        public GeneralRepository(AppDbContext context)
        {
            this.context = context;
            entities = context.Set<Entity>();
        }

        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
        }

        public Entity Get(Key key)
        {
            return entities.Find(key);
        }

        public int Insert(Entity entity)
        {
            entities.Add(entity);
            var insert = context.SaveChanges();
            return insert;
        }

        public int Update(Entity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public int Delete(Key key)
        {
            var findKey = entities.Find(key);
            if (findKey != null)
            {
                entities.Remove(findKey);
                return context.SaveChanges();
            }
            return 404;
        }
    }
}
