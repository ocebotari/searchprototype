using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Lock: Entity<Guid>
    {
        public Lock()
        {
            Id = Guid.NewGuid();
        }

        public Guid? BuildingId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public string Floor { get; set; }

        public string RoomNumber { get; set; }

        public Building Building { get; set; }
    }
}
