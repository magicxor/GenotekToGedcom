using GenotekToGedcom.Models.Genotek;
using GenotekToGedcom.Utils;
using Newtonsoft.Json;

Console.WriteLine($"Converting {args[0]} to {args[1]}...");

var fixDanglingRelations = args is [_, _, "--fix-dangling-relations"];

var genotekData = JsonConvert.DeserializeObject<GenotekData>(File.ReadAllText(args[0]));

genotekData.SaveAsGed(args[1], fixDanglingRelations);

Console.WriteLine($"OK");
