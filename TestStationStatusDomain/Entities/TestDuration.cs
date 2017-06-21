using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusDomain.Entities
{
    public class TestDuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual string TestStationName { get; set; }
        public virtual string TestCaseName { get; set; }
        public virtual double DurationSeconds { get; set; }

    }
}
