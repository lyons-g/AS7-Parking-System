using System;
using System.Collections.Generic;

namespace AS7_Parking_System.Models
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public string Registration { get; set; }
        public string Owner { get; set; }
        public int Apartment { get; set; }

        public virtual Permit Permit { get; set; }
    }
}
