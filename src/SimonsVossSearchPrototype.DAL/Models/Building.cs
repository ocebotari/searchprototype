using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Building: Entity<Guid>
    {
        public Building()
        {
            Id = Guid.NewGuid();
        }

        public string ShortCut { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
