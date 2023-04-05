using GenotekToGedcom.Models.Genotek;
using GenotekToGedcom.Utils;
using Newtonsoft.Json;

Console.WriteLine($"Converting {args[0]} to {args[1]}...");

var fixDanglingRelations = args.Any(a => a == "--fix-dangling-relations");
var transliterate = args.Any(a => a == "--transliterate");

var genotekData = JsonConvert.DeserializeObject<GenotekData>(File.ReadAllText(args[0]));

if (genotekData != null)
{
    genotekData.SaveAsGed(args[1], fixDanglingRelations, transliterate);
    Console.WriteLine("OK");
}
else
{
    Console.WriteLine("ERROR: no genotek data found");
}
