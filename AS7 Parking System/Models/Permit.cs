using System;
using System.Collections.Generic;

namespace AS7_Parking_System.Models
{
    public partial class Permit
    {
        public int PermitId { get; set; }
        public int VehicleId { get; set; }
        public DateTime PermitStartDate { get; set; }
        public DateTime PermitEndDate { get; set; }
        public bool Valid { get; set; }
        public decimal Fee { get; set; }
        public decimal Premium { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
