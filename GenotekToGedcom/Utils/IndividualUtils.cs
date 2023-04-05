using GenotekToGedcom.Models.Genotek;
using LINQ2GEDCOM.Entities;

namespace GenotekToGedcom.Utils;

public static class IndividualUtils
{
    public static Individual WithBirthdate(this Individual individual, Card? card, bool transliterate = false)
    {
        if (card == null)
        {
            return individual;
        }

        if (card.Birthplace != null || card.Birthdate?.Any() == true)
        {
            individual.Birth = new Event(Event.EventType.Birth)
            {
                Date = card.Birthdate?.ToGedcomDate(),
                Place = StringUtils.ToSingleString(card.Birthplace, transliterate),
            };
        }

        return individual;
    }
    
    public static Individual WithDeathdate(this Individual individual, Card? card, bool transliterate = false)
    {
        if (card == null)
        {
            return individual;
        }

        if (card.LiveOrDead == 0)
        {
            individual.Death = new Event(Event.EventType.Death)
            {
                Date = card.Deathdate?.ToGedcomDate(),
                Place = StringUtils.ToSingleString(card.Deathplace, transliterate),
            };
        }

        return individual;
    }
}
