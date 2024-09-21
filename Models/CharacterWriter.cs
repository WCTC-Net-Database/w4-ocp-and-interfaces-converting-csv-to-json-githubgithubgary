using Assignment4.Interfaces;
using CharacterConsole;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using static PlayerCharacter;

public class CharacterWriter
{
    private List<PlayerCharacter> _characterlist;

    public CharacterWriter()
    {
        _characterlist = new List<PlayerCharacter>();
    }
    public List<PlayerCharacter> Characterlist
    {
        set
        {
            string? path = Directory.GetCurrentDirectory();
            if (path != null)
            {
                using (var writer = new StreamWriter(path + "\\Files\\input.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CharacterPlayerMap>();
                    csv.WriteRecords(value);
                }
            }
        }
    }
}