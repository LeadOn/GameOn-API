using GameOn.Domain;
using GameOn.FutDbParser.Entities;
using GameOn.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("GameOn! - FutDB Parser 2.0");
        Console.WriteLine("==================================================\n");

        Console.WriteLine("Please select what you want today:\n");
        Console.WriteLine("1. Parse all teams from FutDB API");
        Console.WriteLine("2. Download club images from FutDB API");
        Console.WriteLine("3. Insert/Update clubs in database");
        Console.WriteLine("4. Remove duplicates in database");
        Console.WriteLine("5. Parse all nations from FutDB API");
        Console.WriteLine("6. Download nations images from FutDB API");
        Console.WriteLine("7. Convert clubs to nations");
        Console.WriteLine("8. Insert nations in database");
        Console.WriteLine("9. Rename nations pictures to ID");

        Console.WriteLine("Your selection (1 by default): ");

        var selection = Console.ReadLine();

        try
        {
            var selectionInt = int.Parse(selection ?? "1");

            switch (selectionInt)
            {
                case 1:
                    await ParseAllTeams();
                    break;

                case 2:
                    await DownloadTeamImages();
                    break;

                case 3:
                    await InsertClubsInDb();
                    break;

                case 4:
                    await RemoveDuplicatesInDb();
                    break;

                case 5:
                    await ParseAllNations();
                    break;

                case 6:
                    await DownloadNationImages();
                    break;

                case 7:
                    await ConvertClubsToNations();
                    break;

                case 8:
                    await InsertNationsInDb();
                    break;

                case 9:
                    await RenameNationImgToId();
                    break;

                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }
        catch
        {
            Console.WriteLine("\n\nInvalid selection.");
        }

        async Task<ClubPaginationResult> GetClubs(int page, string apiKey)
        {
            var client = new RestClient();
            var request = new RestRequest("https://futdb.app/api/clubs?page=" + page, Method.Get);
            request.AddHeader("X-AUTH-TOKEN", apiKey);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ClubPaginationResult>(response.Content ?? throw new NotImplementedException()) ?? throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        async Task<ClubPaginationResult> GetNations(int page, string apiKey)
        {
            var client = new RestClient();
            var request = new RestRequest("https://futdb.app/api/nations?page=" + page, Method.Get);
            request.AddHeader("X-AUTH-TOKEN", apiKey);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ClubPaginationResult>(response.Content ?? throw new NotImplementedException()) ?? throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        async Task ParseAllTeams()
        {
            var clubs = new List<FifaTeam>();

            Console.WriteLine("Please type your FutDB API key: ");
            var apiKey = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("Please type filename (without extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all teams from API...");

            var lastPage = false;
            var currentPage = 1;

            while(!lastPage)
            {
                var clubsApiResult = await GetClubs(currentPage, apiKey);

                if (currentPage == clubsApiResult.Pagination.PageTotal)
                {
                    lastPage = true;
                }
                else
                {
                    currentPage++;
                }

                clubs.AddRange(clubsApiResult.Items);
            }

            // Saving to file
            Console.WriteLine($"{clubs.Count} clubs retrieved from FutDB. Saving to {fileName}.json...");
            await File.WriteAllTextAsync($"{fileName}.json", JsonConvert.SerializeObject(clubs, Formatting.Indented));
        }

        async Task DownloadTeamImages()
        {
            var clubs = new List<FifaTeam>();

            Console.WriteLine("Please type your FutDB API key: ");
            var apiKey = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("Please type filename (with extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all teams from file...");

            var fileContent = await File.ReadAllTextAsync(fileName);

            if(fileContent is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                clubs = JsonConvert.DeserializeObject<List<FifaTeam>>(fileContent);

                Console.WriteLine($"{clubs.Count} clubs parsed from file.");

                var clubsImgPath = Directory.GetCurrentDirectory() + "\\clubs";

                foreach (var club in clubs)
                {
                    if(!Directory.Exists(clubsImgPath))
                    {
                        Console.WriteLine("Creating clubs image directory...");
                        Directory.CreateDirectory(clubsImgPath);
                    }

                    if (!File.Exists(clubsImgPath + "\\" + club.Id + ".png"))
                    {
                        Console.WriteLine($"Downloading image for {club.Name} ({club.Id})...");

                        var client = new RestClient("https://futdb.app");
                        var request = new RestRequest("/api/clubs/" + club.Id + "/image");
                        request.AddHeader("X-AUTH-TOKEN", apiKey);
                        try
                        {
                            var result = await client.DownloadDataAsync(request);
                            await File.WriteAllBytesAsync(clubsImgPath + "\\" + club.Id + ".png", result);
                            Console.WriteLine($"Image for {club.Name} downloaded.");
                        }
                        catch
                        {
                            Console.WriteLine($"Error downloading image for {club.Name} (ID: {club.Id})");
                        }
                    }
                }
            }
        }

        async Task InsertClubsInDb()
        {
            var clubs = new List<FifaTeam>();

            Console.WriteLine("Please type filename (with extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all teams from file...");

            var fileContent = await File.ReadAllTextAsync(fileName);

            if (fileContent is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                clubs = JsonConvert.DeserializeObject<List<FifaTeam>>(fileContent);

                Console.WriteLine($"{clubs.Count} clubs parsed from file.");
                Console.WriteLine("Instantiating DbContext...");

                var gameOnContext = new GameOnContext();

                Console.WriteLine("Trying to read data from table FifaTeam...");

                var fifaTeams = await gameOnContext.FifaTeams.ToListAsync();

                Console.WriteLine($"{fifaTeams.Count} teams already present in database. Updating everything...");

                var insertCount = 0;
                var updateCount = 0;

                foreach (var club in clubs)
                {
                    var clubInDb = await gameOnContext.FifaTeams.FirstOrDefaultAsync(x => x.Id == club.Id);

                    if (clubInDb is null)
                    {
                        Console.WriteLine($"Inserting {club.Name} (ID: {club.Id} in database...");
                        clubInDb = new FifaTeam
                        {
                            Id = club.Id,
                            Name = club.Name,
                        };
                        gameOnContext.FifaTeams.Add(clubInDb);
                        insertCount++;
                    }

                    else
                    {
                        if (club.Name != clubInDb.Name)
                        {
                            Console.WriteLine($"Updating name from {clubInDb.Name} to {club.Name}...");
                            clubInDb.Name = club.Name;
                            gameOnContext.FifaTeams.Update(clubInDb);
                            updateCount++;
                        }
                    }
                }

                Console.WriteLine($"Total: {insertCount} inserts, {updateCount} updates.");
                Console.WriteLine("Saving changes...");
                await gameOnContext.SaveChangesAsync();
            }
        }

        async Task RemoveDuplicatesInDb()
        {
            Console.WriteLine("Checking for duplicates...");
            var gameOnContext = new GameOnContext();

            var fifaTeams = await gameOnContext.FifaTeams.ToListAsync();

            Console.WriteLine($"Searching in {fifaTeams.Count} teams...");

            var duplicates = fifaTeams.GroupBy(x => x.Name).Where(g => g.Count() > 1).ToList();

            Console.WriteLine($"Found {duplicates.Count} duplicates in database.");

            int duplicatesDeletedCount = 0;

            foreach (var duplicate in duplicates)
            {
                Console.WriteLine($"Checking duplicates for {duplicate.Key}...");

                int? idToKeep = null;

                foreach (var row in duplicate)
                {
                    Console.WriteLine($"Checking if games where played with {row.Name} (ID: {row.Id})...");

                    var gamesPlayed = await gameOnContext.FifaGamesPlayed.AnyAsync(x => x.Team1Id == row.Id || x.Team2Id == row.Id);

                    if (idToKeep is null)
                    {
                        Console.WriteLine($"{row.Name} (ID: {row.Id} is the first match, keeping it...");
                        idToKeep = row.Id;
                    }
                    else
                    {
                        if (!gamesPlayed)
                        {
                            Console.WriteLine($"No games played with {row.Name} (ID: {row.Id}), deleting it...");
                            gameOnContext.FifaTeams.Remove(row);
                            duplicatesDeletedCount++;
                        }
                        else
                        {
                            Console.WriteLine($"Games played with {row.Name} (ID: {row.Id}), keeping it...");
                        }
                    }
                }
            }

            Console.WriteLine("Saving changes...");
            await gameOnContext.SaveChangesAsync();
            Console.WriteLine($"Total: {duplicatesDeletedCount} duplicates deleted.");
        }

        async Task ParseAllNations()
        {
            var nations = new List<FifaTeam>();

            Console.WriteLine("Please type your FutDB API key: ");
            var apiKey = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("Please type filename (without extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all nations from API...");

            var lastPage = false;
            var currentPage = 1;

            while (!lastPage)
            {
                var nationsApiResult = await GetNations(currentPage, apiKey);

                if (currentPage == nationsApiResult.Pagination.PageTotal)
                {
                    lastPage = true;
                }
                else
                {
                    currentPage++;
                }

                nations.AddRange(nationsApiResult.Items);
            }

            // Saving to file
            Console.WriteLine($"{nations.Count} nations retrieved from FutDB. Saving to {fileName}.json...");
            await File.WriteAllTextAsync($"{fileName}.json", JsonConvert.SerializeObject(nations, Formatting.Indented));
        }

        async Task DownloadNationImages()
        {
            var nations = new List<FifaTeam>();

            Console.WriteLine("Please type your FutDB API key: ");
            var apiKey = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("Please type filename (with extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all nations from file...");

            var fileContent = await File.ReadAllTextAsync(fileName);

            if (fileContent is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                nations = JsonConvert.DeserializeObject<List<FifaTeam>>(fileContent);

                Console.WriteLine($"{nations.Count} clubs parsed from file.");

                var nationsImgPath = Directory.GetCurrentDirectory() + "\\nations";

                foreach (var nation in nations)
                {
                    if (!Directory.Exists(nationsImgPath))
                    {
                        Console.WriteLine("Creating nations image directory...");
                        Directory.CreateDirectory(nationsImgPath);
                    }

                    if (!File.Exists(nationsImgPath + "\\" + nation.Name + ".png"))
                    {
                        Console.WriteLine($"Downloading image for {nation.Name} ({nation.Id})...");

                        var client = new RestClient("https://futdb.app");
                        var request = new RestRequest("/api/nations/" + nation.Id + "/image");
                        request.AddHeader("X-AUTH-TOKEN", apiKey);
                        try
                        {
                            var result = await client.DownloadDataAsync(request);
                            await File.WriteAllBytesAsync(nationsImgPath + "\\" + nation.Name + ".png", result);
                            Console.WriteLine($"Image for {nation.Name} downloaded.");
                        }
                        catch
                        {
                            Console.WriteLine($"Error downloading image for {nation.Name} (ID: {nation.Id})");
                        }
                    }
                }
            }
        }

        async Task ConvertClubsToNations()
        {
            var nations = new List<FifaTeam>();

            Console.WriteLine("Please type filename (with extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all teams from file...");

            var fileContent = await File.ReadAllTextAsync(fileName);

            if (fileContent is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                nations = JsonConvert.DeserializeObject<List<FifaTeam>>(fileContent);

                Console.WriteLine($"{nations.Count} nations parsed from file.");
                Console.WriteLine("Instantiating DbContext...");

                var gameOnContext = new GameOnContext();

                Console.WriteLine("Trying to read clubs from table FifaTeam...");

                var fifaTeams = await gameOnContext.FifaTeams.Where(x => x.Type == 0).ToListAsync();

                Console.WriteLine($"{fifaTeams.Count} teams already present in database. Updating to nations...");

                var updateCount = 0;

                foreach (var nation in nations)
                {
                    var clubInDb = await gameOnContext.FifaTeams.FirstOrDefaultAsync(x => x.Name == nation.Name && x.Type == 0);

                    if (clubInDb is not null)
                    {
                        Console.WriteLine($"Updating {clubInDb.Name} to nation...");
                        clubInDb.Type = 1;
                        gameOnContext.FifaTeams.Update(clubInDb);
                        updateCount++;
                    }
                }

                Console.WriteLine($"Total: {updateCount} updates.");
                Console.WriteLine("Saving changes...");
                await gameOnContext.SaveChangesAsync();
            }
        }

        async Task InsertNationsInDb()
        {
            var nations = new List<FifaTeam>();

            Console.WriteLine("Please type filename (with extension): ");
            var fileName = Console.ReadLine() ?? throw new NotImplementedException();

            Console.WriteLine("\n\nParsing all nations from file...");

            var fileContent = await File.ReadAllTextAsync(fileName);

            if (fileContent is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                nations = JsonConvert.DeserializeObject<List<FifaTeam>>(fileContent);

                Console.WriteLine($"{nations.Count} nations parsed from file.");
                Console.WriteLine("Instantiating DbContext...");

                var gameOnContext = new GameOnContext();

                Console.WriteLine("Trying to read nations from table FifaTeam...");

                var fifaTeams = await gameOnContext.FifaTeams.Where(x => x.Type == 1).ToListAsync();

                Console.WriteLine($"{fifaTeams.Count} nations already present in database. Inserting/Updating everything...");

                var insertCount = 0;
                var updateCount = 0;

                foreach (var nation in nations)
                {
                    var nationInDb = await gameOnContext.FifaTeams.FirstOrDefaultAsync(x => x.Name == nation.Name && x.Type == 1);

                    if (nationInDb is null)
                    {
                        Console.WriteLine($"Inserting {nation.Name} in database...");
                        nationInDb = new FifaTeam
                        {
                            Name = nation.Name,
                            Type = 1
                        };
                        gameOnContext.FifaTeams.Add(nationInDb);
                        insertCount++;
                    }
                }

                Console.WriteLine($"Total: {insertCount} inserts, {updateCount} updates.");
                Console.WriteLine("Saving changes...");
                await gameOnContext.SaveChangesAsync();
            }
        }

        async Task RenameNationImgToId()
        {
            var nations = new List<FifaTeam>();

            Console.WriteLine("\n\nChecking if directory exists...");

            var nationsImgPath = Directory.GetCurrentDirectory() + "\\nations";

            if(!Directory.Exists(nationsImgPath))
            {
                Console.WriteLine("Directory not found. Exiting...");
                return;
            }
            else
            {
                Console.WriteLine("Getting all files...");

                var files = Directory.GetFiles(nationsImgPath);

                Console.WriteLine($"{files.Length} files found. Getting nations from database...");

                var context = new GameOnContext();

                var nationsInDb = await context.FifaTeams.Where(x => x.Type == 1).ToListAsync();

                Console.WriteLine($"{nationsInDb.Count} nations found in database. Renaming files...");

                int renamedFiles = 0;

                foreach (var file in files)
                {
                    // Getting filename
                    var fileName = Path.GetFileName(file);

                    var nationName = fileName.Replace(".png", "");

                    var nation = nationsInDb.FirstOrDefault(x => x.Name == nationName);

                    if(nation != null)
                    {
                        Console.WriteLine($"Renaming {fileName} to {nation.Id}.png...");
                        File.Move(file, nationsImgPath + "\\" + nation.Id + ".png");
                        renamedFiles++;
                    }
                }

                Console.WriteLine($"Total: {renamedFiles}/{files.Length} files renamed.");
            }
        }
    }
}