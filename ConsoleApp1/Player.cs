using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Player: BaseEntity
    {
        
        public int JerseyNumber { get; set; }
        public string Name { get; set; }
        public int Goals { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
