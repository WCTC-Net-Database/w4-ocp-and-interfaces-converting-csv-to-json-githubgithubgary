using Assignment4.Interfaces;
using static Crayon.Output;
using CsvHelper;
using System.Globalization;
using static PlayerCharacter;
using IOutput = Assignment4.Interfaces.IOutput;
using IInput = Assignment4.Interfaces.IInput;

namespace Assignment4.Services
{
    public class CSVFileHandler : IFileHandler
    {
        private readonly IInput _input;
        private readonly IOutput _output;
        public CSVFileHandler(IInput input, IOutput output)
        {
            _input = input;
            _output = output;
        }

        public List<PlayerCharacter> ReadFile(string filePath)
        {
            string? path = Directory.GetCurrentDirectory();
            if (path != null)
            {
                if (File.Exists(path + filePath))
                {
                    List<PlayerCharacter> pc_list = new List<PlayerCharacter>();

                    using (var reader = new StreamReader(path + filePath))
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
                    //TODO
                    _output.WriteLine(Bright.Red($"Error, unable to locate the character file {filePath}."));
                    return null;
                }
            }
            else
            {
                //TODO
                _output.WriteLine(Bright.Red($"Error, unable to locate default directory."));
                return null;
            }
        }
        public void WriteFile(string filePath, List<PlayerCharacter> characters)
        {
            string? path = Directory.GetCurrentDirectory();
            if (path != null)
            {
                using (var writer = new StreamWriter(path + filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CharacterPlayerMap>();
                    csv.WriteRecords(characters);
                }
            }
        }
    }
}
