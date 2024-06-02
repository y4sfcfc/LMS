using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Match:BaseEntity
    {
        
        public int Week { get; set; }
        public int HostTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public Team HostTeam { get; set; }
        public Team AwayTeam { get; set; }
    }
}
