using GenotekToGedcom;
using GenotekToGedcom.Models.Genotek;
using Newtonsoft.Json;

Console.WriteLine($"Converting {args[0]} to {args[1]}...");

var genotekData = JsonConvert.DeserializeObject<GenotekData>(File.ReadAllText(args[0]));

genotekData.SaveAsGed(args[1]);

Console.WriteLine($"OK");
