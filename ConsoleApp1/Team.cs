using ConsoleApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Team: BaseEntity
    {
        
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public int Points { get; set; }

        public List<Player> Players { get; set; }
        [InverseProperty("HostTeam")]
        public List<Match> HostMatches { get; set; }
        [InverseProperty("AwayTeam")]
        public List<Match> AwayMatches { get; set; }

    }
}
