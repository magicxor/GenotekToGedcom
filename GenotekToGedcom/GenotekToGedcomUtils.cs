using GenotekToGedcom.Models.Genotek;
using LINQ2GEDCOM;
using LINQ2GEDCOM.Entities;

namespace GenotekToGedcom;

public static class GenotekToGedcomUtils
{
    public static void SaveAsGed(this GenotekData genotekData, string outputFilePath)
    {
        if (genotekData.Data?.Nodes == null)
        {
            throw new Exception("no genotek data found");
        }
        
        var people = genotekData
            .Data
            .Nodes
            .Select((node, index) =>
                new
                {
                    Key = node.Id ?? Guid.NewGuid().ToString(),
                    Individual = new Individual
                    {
                        ID = index,
                        Sex = node.Card?.Gender switch
                        {
                            "Female" => "F",
                            "Male" => "M",
                            _ => "U",
                        },
                        Names = new List<Name>
                        {
                            new()
                            {
                                GivenName = string.Join(' ', node.Card?.Name ?? Array.Empty<string>()),
                                Surname = string.Join(' ', node.Card?.Surname ?? Array.Empty<string>()),
                            }
                        }
                    },
                }
            )
            .ToDictionary(dictItem => dictItem.Key, dictItem => dictItem.Individual);

        var families = genotekData
            .Data
            .Nodes
            .Where(node => node.Card != null)
            .SelectMany(node => (node.Card?.Relatives ?? Array.Empty<Relative>())
                .Select(relative => new
                {
                    FamilyCompositeKey = string.Join(' ', new List<string>
                        {
                            node.Id ?? Guid.NewGuid().ToString(),
                            relative.Id ?? Guid.NewGuid().ToString(),
                        }
                        .OrderBy(x => x)),
                    PersonKey = node.Id,
                    RelativeKey = relative.Id,
                    relative.RelationType,
                }))
            .Where(relation => relation.RelationType == "spouse")
            .GroupBy(spouseRelation => spouseRelation.FamilyCompositeKey)
            .Select((relationGrouping, index) =>
                new
                {
                    FamilyId = index,
                    FamilyMembers = relationGrouping
                        .SelectMany(rg => new List<string>
                        {
                            rg.PersonKey ?? "",
                            rg.RelativeKey ?? "",
                        })
                        .Distinct()
                        .Select(personKey =>
                        {
                            people.TryGetValue(personKey, out var person);
                            return (personKey, person);
                        })
                        .Where(valueTuple => valueTuple.person != null),
                }
            )
            .Select(family => new
            {
                family.FamilyId,
                Husband = family.FamilyMembers.FirstOrDefault(individual => individual.person?.Sex == "M"),
                Wife = family.FamilyMembers.FirstOrDefault(individual => individual.person?.Sex == "F"),
            })
            .Select(family => new
            {
                family.FamilyId,
                family.Husband,
                family.Wife,
                Children = genotekData
                    .Data
                    .Nodes
                    .Where(node =>
                        (node.Card?.Relatives?.Any(
                            cr => cr.RelationType == "parent" && cr.Id == family.Husband.personKey) ?? false)
                        && (node.Card?.Relatives?.Any(
                            cr => cr.RelationType == "parent" && cr.Id == family.Wife.personKey) ?? false))
                    .Select(node => people!.GetValueOrDefault(node.Id))
                    .Where(individual => individual != null)
                    .ToList()
            })
            .Select((family, index) => new Family
            {
                ID = index,
                HusbandID = family.Husband.person?.ID,
                WifeID = family.Wife.person?.ID,
                Children = family.Children
                    .Select(c => new Child
                    {
                        ID = c!.ID,
                    })
                    .ToList(),
            })
            .ToList();
        
        var context = new GEDCOMContext("empty.ged");
        
        foreach (var individual in people.Values)
        {
            context.Individuals.Add(individual);
        }

        foreach (var family in families)
        {
            context.Families.Add(family);
        }
        
        context.SubmitChanges(outputFilePath);
    }
}
