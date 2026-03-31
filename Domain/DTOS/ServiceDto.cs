using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class ServiceDto
    {
        public Guid? Id { get; set; }   // null = new service
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
    }
}
