using Bogus;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

 
namespace TestBogus.Models
{
    public static class HelpingExtensions
    {
        public static string Dump<T>(this T obj, bool indent = true)
        {
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }

    public enum Gender
    {
        Male,
        Female,
    }

    public enum SkillLevel
    {
        Newbie,
        Beginner,
        Amateur,
        Pro,
        Expert
    }

    // Классы для тестирования Bogus
    public class GameUser
    {
        public Guid Id { get; set; }
        public DateTime BirthDate { get; set; }

        [JsonIgnore]
        public Gender Gender { get; set; }
        public string StringGender { get => Gender.ToString();  }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public PlayedGame[] PlayedGames { get; set; }
    }

    public class PlayedGame
    {
        public static List<string> Missions = new List<string>() { "Ded End", "Fortress attack", "Enemy in bound", "Thunderstorm", "Surrounded by enemies" };
        public string Mission { get; set; }
        public Guid PlayerID { get; set; }
        public int Score { get; set; }
        public int? Bonus { get; set; } 
        public TimeSpan CompletionTime { get; set; }

        [JsonIgnore]
        public SkillLevel SkillLevel { get; set; }

        public string StringSkillLevel { get => SkillLevel.ToString(); }
    }

    public static class UserGenerator
    {
        public static PlayedGame GetPlayedGameProcedural(Guid playerId)
        {
            var faker = new Faker();
            //var maxCompletionTime = new TimeSpan(0, 10, 0);
            return new PlayedGame()
            {
                PlayerID = playerId,
                Score = faker.Random.Number(1, 1000),
                CompletionTime = faker.Date.Timespan(),
                SkillLevel = faker.PickRandom<SkillLevel>(),
                Mission = faker.PickRandom(PlayedGame.Missions)
            };
        }

        public static Faker<PlayedGame> GetPlayedGameFaker(Guid playerId, bool isNeedLogging = false)
        {
            var maxCompletionTime = new TimeSpan(0, 10, 0);
            return new Faker<PlayedGame>().
                   StrictMode(true).
                   RuleFor(x => x.PlayerID, _ => playerId).
                   RuleFor(x => x.SkillLevel, f => f.PickRandom<SkillLevel>()).
                   RuleFor(x => x.Score, (f, game) => f.Random.Number(1, 1000) * (int)game.SkillLevel).
                   RuleFor(x => x.Bonus, x => x.Random.Number(1, 5).OrNull(x, 0.8f)).
                   RuleFor(x => x.CompletionTime, x => x.Date.Timespan(maxCompletionTime)).
                   RuleFor(x => x.Mission, f => f.PickRandom(PlayedGame.Missions)).
                   FinishWith((f, game) =>
                   {
                       if (isNeedLogging)
                       {
                           Console.WriteLine(game.Dump());
                       }
                   }
                   );
        }

        public static Faker<GameUser> GetGameUserFaker(bool isNeedLogging = false)
        {
            int minGames = 1;
            int maxGames = 10;
            Randomizer.Seed = new Random(8675309);

            return new Faker<GameUser>("ru").
                   StrictMode(true).
                   RuleFor(x => x.Id, x => Guid.NewGuid()).
                   RuleFor(x => x.Address, f => f.Address.FullAddress()).
                   RuleFor(x => x.Name, f => f.Person.FirstName).
                   RuleFor(x => x.Surname, f => f.Person.LastName).
                   RuleFor(x => x.Gender, f => f.PickRandom<Gender>()).
                   RuleFor(x => x.Email, f => f.Person.Email).
                   RuleFor(x => x.Password, f => f.Hacker.Phrase()).
                   RuleFor(x => x.FullName, (f, user) => user.Name + " " + user.Surname).
                   RuleFor(x => x.PlayedGames, (f, user) => GetPlayedGameFaker(user.Id).GenerateBetween(minGames, maxGames).ToArray()).
                   RuleFor(x => x.BirthDate, f => f.Date.Past(100)).
                   RuleFor(x => x.UserName, f => f.Internet.UserName()).
                   FinishWith((f, user) =>
                   {
                       if (isNeedLogging)
                       {
                           Console.WriteLine(user.Dump());
                       }
                   }
                   );
        }
    }
}
