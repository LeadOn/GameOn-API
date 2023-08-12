using Newtonsoft.Json;
using RestSharp;
using YuFoot.Entities;
using YuFoot.EntitiesContext;
using YuFoot.Parser.Models;

Console.WriteLine("YuFoot Parser 1.0");
Console.WriteLine("==================================================\n");

var currentPath = Directory.GetCurrentDirectory();

Console.WriteLine("\nCurrent execution path: " + currentPath);
Console.WriteLine("\nGetting json files in current folder...");

var files = Directory.GetFiles(currentPath);
var jsonFiles = files.Where(x => x.EndsWith(".json")).ToList();

Console.WriteLine(jsonFiles.Count + " JSON files found in current folder.");

var teams = new List<FifaTeam>();

foreach (var jsonFile in jsonFiles)
{
    Console.WriteLine("Parsing file " + jsonFile + "...");
    
    var fileContent = File.ReadAllText(jsonFile);
    try
    {
        var jsonContent = JsonConvert.DeserializeObject<FootballDataTeamList>(fileContent);
        if(jsonContent is null)
        {
            Console.WriteLine("⚠️ An error happened while deserializing JSON content. ⚠️");
        }
        else
        {
            if (jsonContent.Items is null)
            {
                Console.WriteLine("⚠️ Teams array seems to be invalid. ⚠️");
            }
            else
            {
                Console.WriteLine("✅ Parsing done! Found " + jsonContent.Items.Count + " teams in it.");

                foreach(var team in jsonContent.Items)
                {
                    var teamEntity = new FifaTeam
                    {
                        Id = team.Id,
                        Name = team.Name,
                    };


                    if (teams.FirstOrDefault(x => x.Id == team.Id || x.Name == team.Name) is null)
                    {
                        if(!File.Exists("S:\\Test App\\img\\" + team.Id + ".png"))
                        {
                            // Getting img
                            var client = new RestClient("https://futdb.app");
                            var request = new RestRequest("/api/clubs/" + team.Id + "/image");
                            request.AddHeader("X-AUTH-TOKEN", "TOKEN HERE");
                            var result = await client.DownloadDataAsync(request);
                            await File.WriteAllBytesAsync("S:\\Test App\\img\\" + team.Id + ".png", result);

                            Thread.Sleep(1000);
                        }
                        

                        teams.Add(teamEntity);
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Team " + team.Name + " (ID " + team.Id + ") already in list!");
                    }
                }
            }
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine("⚠️ Unable to parse file " + jsonFile + "! Exception: " + ex.Message);
    }
}

Console.WriteLine("ℹ️ Number of teams retrieved: " + teams.Count);
Console.WriteLine("Connecting to database...");

using(var context = new YuFootContext())
{
    try
    {
        Console.WriteLine("Adding teams to the database...");
        context.FifaTeams.AddRange(teams);
        Console.WriteLine("Saving changes...");
        //context.SaveChanges();
        Console.WriteLine("Done!");
    }
    catch(Exception ex)
    {
        Console.WriteLine("⚠️ Unable to import data in database! Exception: " + ex.Message);
    }
}