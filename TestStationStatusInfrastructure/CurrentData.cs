using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusInfrastructure
{
    public class CurrentData
    {
        [Key]
        public int Id { get; set; }

        public String Title { get; set; }
    }
}
