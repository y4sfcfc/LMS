using ConsoleApp.Repository;
using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        public static IRepository<Player> PlayerRepository = new Repository<Player>();
        public static IRepository<Team> TeamRepository = new Repository<Team>();
        public static IRepository<Match> MatchRepository = new Repository<Match>();
        static void Main(string[] args)
        {
            Console.WriteLine("SERIA A");

            

            while (true)
            {
                Console.WriteLine("Menyu:");
                Console.WriteLine("1. Bitmiş oyuna dair neticelerin daxil edilmesi");
                Console.WriteLine("2. Bir komandanın cari veziyyetinin ve futbolcularının siyahilanmasi");
                Console.WriteLine("3. Puan cedvelinin siralanmasi");
                Console.WriteLine("4. En cox qol vuran ve en cox qol yiyen komandalarin siralanmasi");
                Console.WriteLine("5. Liqde en cox qol atan futbolcunun siralanmasi");
                Console.WriteLine("6. Cixis");
                Console.Write("Seçiminiz: ");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        EnterMatchResult();
                        break;
                    case 2:
                        DisplayTeamInfo();
                        break;
                    case 3:
                        DisplayLeagueTable();
                        break;
                    case 4:
                        DisplayTopScoringAndConcedingTeams();
                        break;
                    case 5:
                        DisplayTopScoringPlayers();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Yanlis secim!");
                        break;
                }
            }
        }

        static void InitializeData()
        {
            Console.WriteLine("Komanda adladlarini, kodlarini ve oyuncularini daxil edin (sonlandirmaq ucun 'cixis' yazın):");

            while (true)
            {
                Console.Write("Komanda adi: ");
                string teamName = Console.ReadLine();

                Console.Write("Komanda Id: ");
                int teamId = int.Parse(Console.ReadLine());


                if (teamName.ToLower() == "cixis")
                    break;
                               
                
               Team Team =  TeamRepository.Add(new Team { Name = teamName });


                while (true)
                {
                    Console.WriteLine("Oyuncu melumatlarini daxil edin  (sonlandirmaq ucun 'cixis' yazın):");

                    Console.Write("Oyuncu adi: ");
                    string playerName = Console.ReadLine();

                    if (playerName.ToLower() == "cixis")
                        break;

                    Console.Write("Forma nomresi: ");
                    int jerseyNumber = int.Parse(Console.ReadLine());



                    PlayerRepository.Add(new Player { JerseyNumber = jerseyNumber, Name = playerName, TeamId = Team.Id });
                }
            }
        }

        static void EnterMatchResult()
        {
            Console.Write("Tur nomresi: ");
            int week = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.Write("Ev sahibi Id: ");
                int homeTeamCode = int.Parse(Console.ReadLine());

                Console.Write("Qonaq komanda Id: ");
                int awayTeamCode =int.Parse(Console.ReadLine());

                Console.Write("Ev sahibi komandanin qol sayi: ");
                int homeTeamGoals = int.Parse(Console.ReadLine());

                Console.Write("Qonaq komandanin qol sayi: ");
                int awayTeamGoals = int.Parse(Console.ReadLine());

                MatchRepository.Add(new Match
                {
                    Week = week,
                    HostTeamId = homeTeamCode,
                    AwayTeamId = awayTeamCode,
                    HomeTeamGoals = homeTeamGoals,
                    AwayTeamGoals = awayTeamGoals
                });

                UpdateTeamStats(homeTeamCode, awayTeamCode, homeTeamGoals, awayTeamGoals);

                Console.Write("Bu tur basqa oyun kecirildimi (b/x): ");
                if (Console.ReadLine().ToLower() != "b")
                    break;
            }
        }

        static void UpdateTeamStats(int homeTeamCode, int awayTeamCode, int homeTeamGoals, int awayTeamGoals)
        {
            var homeTeam = TeamRepository.GetById(homeTeamCode);
            var awayTeam = TeamRepository.GetById(awayTeamCode);

            homeTeam.GoalsFor += homeTeamGoals;
            homeTeam.GoalsAgainst += awayTeamGoals;
            awayTeam.GoalsFor += awayTeamGoals;
            awayTeam.GoalsAgainst += homeTeamGoals;

            if (homeTeamGoals > awayTeamGoals)
            {
                homeTeam.Wins++;
                awayTeam.Losses++;
            }
            else if (homeTeamGoals < awayTeamGoals)
            {
                homeTeam.Losses++;
                awayTeam.Wins++;
            }
            else
            {
                homeTeam.Draws++;
                awayTeam.Draws++;
            }

            while (true)
            {
                Console.Write("Qol vuran oyuncunun forma nomresi: ");
                int jerseyNumber = int.Parse(Console.ReadLine());

                Console.Write("Vurulan qol sayi: ");
                int goals = int.Parse(Console.ReadLine());

                var player = PlayerRepository.GetById(jerseyNumber);
                player.Goals += goals;

                Console.Write("Basqa qol vuran oyuncu varmı? (b/x): ");
                if (Console.ReadLine().ToLower() != "b")
                    break;
            }
            TeamRepository.Update(homeTeam);
            TeamRepository.Update(awayTeam);
        }

        static void DisplayTeamInfo()
        {
            Console.Write("Komandanin Id-i daxil edin: ");
            int teamId = int.Parse(Console.ReadLine());

            var team = TeamRepository.GetById(teamId);
            if (team == null)
            {
                Console.WriteLine("Komanda tapilmadi.");
                return;
            }

            Console.WriteLine($"Komanda: {team.Name}");
            Console.WriteLine($"Kod: {team.Id}, Qalibiyyet: {team.Wins}, Beraberlik: {team.Draws}, Meglubiyyet: {team.Losses}");
            Console.WriteLine($"Vurulan qol: {team.GoalsFor}, BUraxilan qol: {team.GoalsAgainst}");

            var teamPlayers = PlayerRepository.GetAll().Where(p=>p.TeamId==teamId).OrderBy(p=>p.JerseyNumber);
            foreach (var player in teamPlayers)
            {
                Console.WriteLine($"Forma No: {player.JerseyNumber}, Ad Soyad: {player.Name}, Vurdugu qol: {player.Goals}");
            }
        }

        static void DisplayLeagueTable()
        {
            var sortedTeams = TeamRepository.GetAll().OrderByDescending(t => t.Points).ThenByDescending(t => t.GoalsFor - t.GoalsAgainst).ThenByDescending(t => t.GoalsFor);
            Console.WriteLine("\nTurnir cedveli:");

            Console.WriteLine("Komanda    Xal   TF   VQ   BQ  ");
            Console.WriteLine("-----------------------------------");

            foreach (var team in sortedTeams)
            {
                
               
                Console.WriteLine($"{team.Name}   {team.Points}   {team.GoalsFor - team.GoalsAgainst}   {team.GoalsFor}   {team.GoalsAgainst}");
            }
        }

        static void DisplayTopScoringAndConcedingTeams()
        {
            var topScoringTeams = TeamRepository.GetAll().OrderByDescending(t => t.GoalsFor).Take(1);
            var topConcedingTeams = TeamRepository.GetAll().OrderByDescending(t => t.GoalsAgainst).Take(1);

            Console.WriteLine("\nEn cox qol vuran komanda:");
            foreach (var team in topScoringTeams)
            {
                Console.WriteLine($"{team.Name} - Vurulan qol: {team.GoalsFor}");
            }

            Console.WriteLine("\nEn cox qol buraxan komanda:");
            foreach (var team in topConcedingTeams)
            {
                Console.WriteLine($"{team.Name} - Buraxilan Gol: {team.GoalsAgainst}");
            }
        }

        static void DisplayTopScoringPlayers()
        {

            var topScoringPlayers = PlayerRepository.GetAll().OrderByDescending(p => p.Goals).Take(1);
            Console.WriteLine("\nEn cox qol vuran oyuncu:");
            foreach (var player in topScoringPlayers)
            {
                Console.WriteLine("   Komanda      Ad Soyad       Forma No        Qol sayi");
                Console.WriteLine("-----------     -----------     ------------    __________");
                Console.WriteLine($"   {player.Team.Name}  {player.Name}             {player.JerseyNumber}           {player.Goals}");
            }
        }

    }
}
