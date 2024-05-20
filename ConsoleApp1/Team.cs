using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Team
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public int Points => Wins * 3 + Draws;
        public int GoalDifference => GoalsFor - GoalsAgainst;
    }
}
