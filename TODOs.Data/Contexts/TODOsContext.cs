using System.Data.Entity;

namespace TODOs.Data.Entities
{
    public class TODOsContext : DbContext
    {
        public TODOsContext() : base("name=TODOsContext") { }

        public virtual DbSet<TODO> TODOs { get; set; }

    }
}
