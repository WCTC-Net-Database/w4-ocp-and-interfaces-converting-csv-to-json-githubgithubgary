using Assignment4.Interfaces;
using CharacterConsole;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

public class CharacterReader
{
    private List<PlayerCharacter> _characterlist;

    private CharacterReader()
    {
         _characterlist = new List<PlayerCharacter>();
    }

    public List<PlayerCharacter> CharacterList
    {
        get
        {
            List<PlayerCharacter> pc_list = new List<PlayerCharacter>();

            string? path = Directory.GetCurrentDirectory();

            if (path != null)
            {
                using (var reader = new StreamReader(path + "\\Files\\input.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new List<PlayerCharacter>();
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        PlayerCharacter record = new PlayerCharacter()
                        {
                            Name = csv.GetField("Name"),
                            ClassName = csv.GetField("Class"),
                            Level = csv.GetField("Level"),
                            HP = csv.GetField("HP"),
                            Equipment = csv.GetField("Equipment"),
                            Id = csv.Context.Parser.RawRow - 1
                        };
                        records.Add(record);
                    }
                    pc_list = records;
                }
                return pc_list;
            }
            else
            {
                return null;
            }
        }
    }
}