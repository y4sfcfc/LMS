using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {
        static List<Team> teams = new List<Team>();
        static List<Player> players = new List<Player>();
        static List<Match> matches = new List<Match>();
        static void Main(string[] args)
        {
            Console.WriteLine("Futbol Lig Yönetim Sistemine Hoş Geldiniz!");

            InitializeData(); 

            while (true)
            {
                Console.WriteLine("Menyu:");
                Console.WriteLine("1. Bitmiş oyuna dair neticelerin daxil edilmesi");
                Console.WriteLine("2. Bir komandanın cari veziyyetinin və futbolcularının siyahilanmasi");
                Console.WriteLine("3. Puan cedvelinin siralanmasi");
                Console.WriteLine("4. En cox qol vuran və ən cox qol yiyen komandalarin siralanmasi");
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

                if (teamName.ToLower() == "cixis")
                    break;

                Console.Write("Komanda kodu: ");
                int teamCode = int.Parse(Console.ReadLine());

                teams.Add(new Team { Code = teamCode, Name = teamName });

                
                while (true)
                {
                    Console.WriteLine("Oyuncu melumatlarini daxil edin  (sonlandirmaq ucun 'cixis' yazın):");

                    Console.Write("Oyuncu adi: ");
                    string playerName = Console.ReadLine();

                    if (playerName.ToLower() == "cixis")
                        break;

                    Console.Write("Forma nomresi: ");
                    int jerseyNumber = int.Parse(Console.ReadLine());

               

                    players.Add(new Player { JerseyNumber = jerseyNumber, Name = playerName, TeamCode = teamCode });
                }
            }
        }

        static void EnterMatchResult()
        {
            Console.Write("Tur nomresi: ");
            int week = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.Write("Ev sahibi komanda kodu: ");
                int homeTeamCode = int.Parse(Console.ReadLine());

                Console.Write("Qonaq komanda kodu: ");
                int awayTeamCode = int.Parse(Console.ReadLine());

                Console.Write("Ev sahibi komandanin qol sayi: ");
                int homeTeamGoals = int.Parse(Console.ReadLine());

                Console.Write("Qonaq komandanin qol sayi: ");
                int awayTeamGoals = int.Parse(Console.ReadLine());

                matches.Add(new Match
                {
                    Week = week,
                    HomeTeamCode = homeTeamCode,
                    AwayTeamCode = awayTeamCode,
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
            var homeTeam = teams.First(t => t.Code == homeTeamCode);
            var awayTeam = teams.First(t => t.Code == awayTeamCode);

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

                var player = players.First(p => p.JerseyNumber == jerseyNumber);
                player.Goals += goals;

                Console.Write("Basqa qol vuran oyuncu varmı? (b/x): ");
                if (Console.ReadLine().ToLower() != "b")
                    break;
            }
        }

        static void DisplayTeamInfo()
        {
            Console.Write("Komandanin adini daxil edin: ");
            string teamName = Console.ReadLine();

            var team = teams.FirstOrDefault(t => t.Name == teamName);
            if (team == null)
            {
                Console.WriteLine("KOmanda tapilmadi.");
                return;
            }

            Console.WriteLine($"Komanda: {team.Name}");
            Console.WriteLine($"Kod: {team.Code}, Qalibiyyet: {team.Wins}, Beraberlik: {team.Draws}, Meglubiyyet: {team.Losses}");
            Console.WriteLine($"Vurulan qol: {team.GoalsFor}, BUraxilan qol: {team.GoalsAgainst}");

            var teamPlayers = players.Where(p => p.TeamCode == team.Code).OrderBy(p => p.JerseyNumber);
            foreach (var player in teamPlayers)
            {
                Console.WriteLine($"Forma No: {player.JerseyNumber}, Ad Soyad: {player.Name}, Vurdugu qol: {player.Goals}");
            }
        }

        static void DisplayLeagueTable()
        {
            var sortedTeams = teams.OrderByDescending(t => t.Points).ThenByDescending(t => t.GoalDifference).ThenByDescending(t => t.GoalsFor);
            Console.WriteLine("\nTurnir cedveli:");
            foreach (var team in sortedTeams)
            {
                Console.WriteLine($"{team.Name} - Xal: {team.Points}, Top ferqi: {team.GoalDifference}, Vurulan Gol: {team.GoalsFor}, Buraxilan Gol: {team.GoalsAgainst}");
            }
        }

        static void DisplayTopScoringAndConcedingTeams()
        {
            var topScoringTeams = teams.OrderByDescending(t => t.GoalsFor).Take(1);
            var topConcedingTeams = teams.OrderByDescending(t => t.GoalsAgainst).Take(1);

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
            
            var topScoringPlayers = players.OrderByDescending(p => p.Goals).Take(1);
            Console.WriteLine("\nEn cox qol vuran oyuncu:");
            foreach (var player in topScoringPlayers)
            {
                Console.WriteLine("   Ad Soyad       Forma No        Qol sayi");
                Console.WriteLine("-----------     -----------     ------------");
                Console.WriteLine($"     {player.Name}             {player.JerseyNumber}           {player.Goals}");
            }
        }

    }
}
