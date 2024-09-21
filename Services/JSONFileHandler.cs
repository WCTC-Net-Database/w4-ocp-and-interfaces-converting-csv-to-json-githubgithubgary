using Assignment4.Interfaces;
using Crayon;
using static Crayon.Output;
using Newtonsoft.Json;
using static PlayerCharacter;
using IOutput = Assignment4.Interfaces.IOutput;
using IInput = Assignment4.Interfaces.IInput;
using System.Xml.Linq;

//www.youtube.com/watch?v=Y14gG9IJ230
//www.youtube.com/watch?v=XvsOnKvwhfQ
//stackoverflow.com/questions/18538428/loading-a-json-file-into-c-sharp-program

// read file into a string and deserialize JSON to a type
//PlayerCharacter pc_list = JsonConvert.DeserializeObject<PlayerCharacterJSONMap>(File.ReadAllText(@"path + filePath".json));

namespace Assignment4.Services
{
    public class PlayerCharacterJSONMap
    {
        public string? Name { get; set; }
        public string? @Class { get; set; }
        public string? Level { get; set; }
        public string? HP { get; set; }
        public string? Equipment { get; set; }

    }
    public class JSONFileHandler : IFileHandler
    {
        private readonly IInput _input;
        private readonly IOutput _output;


        public JSONFileHandler(IInput input, IOutput output)
        {
            _input = input;
            _output = output;
        }

        public List<PlayerCharacter> ReadFile(string filePath)
        {
            string? path = Directory.GetCurrentDirectory();
            List <PlayerCharacter> characters = new List<PlayerCharacter>();
            if (path != null)
            {
                if (File.Exists(path + filePath))
                {
                    // read file into a string and deserialize JSON to a type
                    string fullPath = path + filePath;
                    string json = File.ReadAllText(fullPath);

                    //TODO Check to make sure the file was there and we got results
                    List<PlayerCharacterJSONMap> inboundList = JsonConvert.DeserializeObject<List<PlayerCharacterJSONMap>>(json);

                    for (int i = 1; i <= inboundList.Count(); i++)
                    {
                        string n = inboundList[i - 1].Name;
                        string c = inboundList[i - 1].Class;
                        string l = inboundList[i - 1].Level;
                        string h = inboundList[i - 1].HP;
                        string e = inboundList[i - 1].Equipment;
                        PlayerCharacter character = new PlayerCharacter(n, c, l, h, e, i);
                        characters.Add(character);
                    }
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
            return characters; 
        }
        public void WriteFile(string filePath, List<PlayerCharacter> characters)
        {
            //TODO Implement this code
            List<PlayerCharacterJSONMap> outboundList = new List<PlayerCharacterJSONMap>();

            foreach (PlayerCharacter character in characters)
            {
                PlayerCharacterJSONMap playerCharacterJSONMap = new PlayerCharacterJSONMap();
                playerCharacterJSONMap.Name = character.Name;
                playerCharacterJSONMap.Class = character.ClassName;
                playerCharacterJSONMap.Level = character.Level;
                playerCharacterJSONMap.HP = character.HP;
                playerCharacterJSONMap.Equipment = character.Equipment;

                outboundList.Add(playerCharacterJSONMap);
            }
            string json = JsonConvert.SerializeObject(outboundList, Formatting.Indented);

            if (json != null)
            {
                string? path = Directory.GetCurrentDirectory();
                string fullPath = path + filePath;
                File.WriteAllText(fullPath, json);
            }

            //string json = JsonSerializer.Serialize(outboundList);
            //File.WriteAllText(@"D:\path.json", json);

            //string json = JsonConvert.SerializeObject(outboundList);
        }
    }
}
