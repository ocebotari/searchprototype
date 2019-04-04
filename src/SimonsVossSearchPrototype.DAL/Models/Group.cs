using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Group: Entity<Guid>
    {
        public Group()
        {
            Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
