using System;
using System.Collections.Generic;
using System.Text;

namespace VitaTest.Domain.Models
{
    public class BaseDbEntity
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
