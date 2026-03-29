using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class LeadDTO
    {
        public class CreateLeadDto
        {
            public Guid ClinicId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Source { get; set; }
            public string Status { get; set; }
        }

        public class AbandonLeadDto
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public int Step { get; set; }
        }
    }
}
