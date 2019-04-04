using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Media: Entity<Guid>
    {
        public Media()
        {
            Id = Guid.NewGuid();
        }

        public Guid? GroupId { get; set; }

        public string Type { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public Group Group { get; set; }
    }
}
