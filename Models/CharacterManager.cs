using static Crayon.Output;
using Assignment4.Interfaces;
using Assignment4.Services;
using IOutput = Assignment4.Interfaces.IOutput;

namespace Assignment4.Models;

//###########################################
// 'github.com/riezebosch/crayon'
//
// Methods for the extension
//Colors
//   Black()
//   Red()
//   Green()
//   Yellow()
//   Blue()
//   Magenta()
//   Cyan()
//   White()

//Rgb(r, g, b)

//Background.Blue()
//Bright.Blue()

//Decoration
//   Bold()
//   Dim()
//   Underline()
//   Reversed()
//####################################
public class CharacterManager
{
    private readonly IInput _input;
    private readonly IOutput _output;

    // Character management stuff
    private bool _updates = false; // Indicates that the list of characters has changed since the initial load
    private List<PlayerCharacter> _campaignList = new List<PlayerCharacter>();

    // File Stuff
    private string _filePath = null;
    private static IFileHandler _fileHandler;
    private string _fileFormat = "CSV";
    private Dictionary<string, string> _supportedFileFormats = new Dictionary<string, string>() { { "CSV", ".csv" } , { "JSON", ".json" } };
    public CharacterManager(IInput input, IOutput output)
    {
        _input = input;
        _output = output;
        _updates = false;
    }

    public void Run()
    {
        bool EndProg = false;
        _output.WriteLine(Bright.Blue().Text("Welcome to Character Management\r\n"));
        //
        //AnsiConsole.MarkupLine("[bold green]Hello[/] [italic blue]World[/]!");

        //CharacterReader cr = new CharacterReader();
        //_campaignList = cr.CharacterList;
        string filePath = "\\Files\\input";
        switch (_fileFormat)
        {
            case "CSV":
                _filePath = filePath + _supportedFileFormats["CSV"];
                _fileHandler = new CSVFileHandler(_input, _output);
                _campaignList = _fileHandler.ReadFile(_filePath);
                break;
            case "JSON":
                _filePath = filePath + _supportedFileFormats["JSON"];
                _fileHandler = new JSONFileHandler(_input, _output);
                _campaignList = _fileHandler.ReadFile(_filePath);
                break;
            default:
                _filePath = filePath + _supportedFileFormats["CSV"];
                _fileHandler = new CSVFileHandler(_input, _output);
                _campaignList = _fileHandler.ReadFile(_filePath);
                break;
        }

        while ((EndProg == false) && (_campaignList != null))
        {
            _output.WriteLine("     " + Bright.Blue().Underline().Text("Main Menu:\r\n"));
            _output.WriteLine($"1. Display Characters");
            _output.WriteLine($"2. Add Character");
            _output.WriteLine($"3. Level Up Character");
            _output.WriteLine($"4. Find Character(s)");
            _output.WriteLine($"5. Change File Formats");
            _output.WriteLine($"6. Save Character List");
            _output.WriteLine($"0. Exit");
            _output.Write($"\r\nEnter your choice: ");

            var choice = _input.ReadLine();

            switch (choice)
            {
                case "1":
                    ListCampaign();
                    break;
                case "2":
                    AddCharacter();
                    break;
                case "3":
                    LevelUpCharacters();
                    break;
                case "4":
                    FindCharacters();
                    break;
                case "5":
                    ChangeFileFormat();
                    break;
                case "6":
                    SaveCharacters();
                    break;
                case "0":
                    // Leave the program
                    EndProg = ExitProgram();
                    break;
                default:
                    _output.WriteLine(Bright.Red("Invalid choice. Please try again."));
                    break;
            }
            _output.Clear();
        }
    }
    public bool ConfirmYN()
    {
        bool ans = false;

        while (ans == false)
        {
            _output.Write($"{Green("Yes (Y)")} or {Red("No (N)")}? ");
            var response = char.ToUpper(Convert.ToChar(Console.Read()));
            switch (response)
            {
                case 'Y':
                    ans = true;
                    break;

                case 'N':
                    ans = true;
                    break;

                default:
                    _output.WriteLine($"You can only answer with {Green("Yes (Y)")} or {Red("No (N)")}.");
                    _output.Clear();
                    continue;
            }
        }
        return ans;
    }
    public void ListCharacters(List<PlayerCharacter> pcList)
    {
        string? i = null;
        string? n = null;
        string? l = null;
        string? cn = null;
        string? e = null;

        foreach (var character in pcList)
        {
            i = character.Id.ToString();
            n = character.Name;
            l = character.Level;
            cn = character.ClassName;
            e = character.PrintEquipment(",");
            _output.WriteLine($"Player {Red("#")}{Bold().Red().Text(i)}: {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is at level {Bold().Rgb(255, 165, 0).Text(l)} with the following equipment: {Bold().Green().Text(e)}.");
        }
    }

    public void ListCampaign()
    {
        _output.Clear();
        _output.WriteLine($"The campaign includes the following player character(s):\r\n");

        if (_campaignList.Count == 0)
        {
            _output.WriteLine($"There are {Bright.Red().Text("no")} player character(s) in the current campaign.");
        }
        else
        {
            ListCharacters(_campaignList);
        }
        _output.Write($"\r\n\r\nPress {Yellow("<Enter>")} key when you are ready to continue...");
        var input = _input.ReadLine();
    }

    public void ListCampaign(List<PlayerCharacter> pc_list)
    {
        _output.Clear();
        _output.WriteLine($"The results include the following player character(s):\r\n");

        if (pc_list.Count == 0)
        {
            _output.WriteLine($"There are {Bright.Red().Text("no")} player character(s) in the current campaign.");
        }
        else
        {
            ListCharacters(pc_list);
        }
        _output.Write($"\r\n\r\nPress {Yellow("<Enter>")} key when you are ready to continue...");
        var input = _input.ReadLine();
    }

    public void AddCharacter()
    {
        bool endInput = false;
        bool endEquipInput = false;
        bool continueInput = false;

        string? answer = null;
        string? equipname = null;
        PlayerCharacter character = null;
        List<string> questions = new List<string>() { "name", "class", "level" };
        List<string> answers = null;
        int count = 0;
        int max_count = questions.Count();
        bool first_time = true;

        //This means we have questions
        if (max_count > 0)
        {
            _output.Clear();

            while (endInput == false)
            {
                _output.Write($"\r\nYou must enter a character's {questions[count]} (Type {Yellow("<End>")} to exit): ");
                answer = _input.ReadLine();
                if (answer.Length == 0)
                {
                    _output.Write($"\r\nYou must enter a character's {questions[count]} (Type {Yellow("<End>")} to exit): ");
                    endInput = false;
                }
                else
                {
                    if (answer.ToLower() == "end")
                    {
                        endInput = true;
                    }
                    else
                    {
                        if (first_time == true)
                        {
                            answers = new List<string>();
                            first_time = false;
                        }
                        answers.Add(answer);
                        count += 1;
                        if (count >= max_count)
                        {
                            endInput = true;
                            continueInput = true;
                        }
                    }
                }
            }

            if (continueInput == true)
            {
                // Ask for more equipment until the user indicates they are done
                character = new PlayerCharacter(answers[0], answers[1], answers[2]);
                character.Updated = true;  // indicates that this character was changed after the inital champaign was loaded
                List<string> equipment_List = new List<string>();

                while (endEquipInput == false)
                {
                    _output.Write($"\r\nEnter your character's equipment (Type {Yellow("<Done>")} when finished or {Yellow("<End>")} to exit): ");
                    equipname = _input.ReadLine();
                    if (equipname == null)
                    {
                        _output.Write($"Type {Yellow("<Done>")} when finished entering equipment or {Yellow("<End>")} to exit: ");
                        endEquipInput = false;
                    }
                    else
                    {
                        if (equipname.ToLower() == "done")
                        {
                            var x = equipment_List.ToString();
                            character.Equipment = string.Join("|", equipment_List);
                            endEquipInput = true;
                            endInput = false;
                        }
                        else
                        {
                            if (equipname.ToLower() == "end")
                            {
                                endEquipInput = true;
                                endInput = true;
                            }
                            else
                            {
                                equipment_List.Add(equipname);
                            }
                        }
                    }
                }
            }

            if (endInput != true)
            {
                string i = character.Id.ToString();
                string n = character.Name;
                string l = character.Level;
                string cn = character.ClassName;
                string e = character.PrintEquipment(",");
                string? str = null;

                _output.Clear();
                _output.WriteLine($"Add, {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} at level {Bold().Rgb(255, 165, 0).Text(l)} with the following equipment: {Bold().Green().Text(e)}.");
                bool AddCharacter = ConfirmYN();
                if (AddCharacter)
                {
                    _output.Clear();
                    str = string.Format($"Welcome, {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)}! Congradulations on achieving level {Bold().Rgb(255, 165, 0).Text(l)}. Your supplies include: {Bold().Green().Text(e)}.");
                    _campaignList.Add(new PlayerCharacter(character.Name, character.ClassName, character.Level));
                    _campaignList[_campaignList.Count() - 1].Id = _campaignList.Count();
                    _campaignList[_campaignList.Count() - 1].Equipment = character.Equipment;
                    _updates = true;
                }
                else
                {
                    str = string.Format($"Sorry {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} will not be joining of the campaign!");
                }
                _output.WriteLine(str);
            }
        }
    }

    public void LevelUpCharacter(PlayerCharacter tmpPlayerCharacter)
    {
        int nextLevel = 0;
        int levels = 0;
        bool success = false;
        string? i = tmpPlayerCharacter.Id.ToString();
        string? n = tmpPlayerCharacter.Name;
        string? l = tmpPlayerCharacter.Level;
        string? cn = tmpPlayerCharacter.ClassName;
        string? e = tmpPlayerCharacter.PrintEquipment(",");

        _output.Clear();
        _output.WriteLine($"Player #{i}: {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is at level {Bold().Rgb(255, 165, 0).Text(l)} with the following equipment: {Bold().Green().Text(e)}.");
        while (true)
        {
            nextLevel = 0;
            _output.Write($"\r\nHow many levels has {Bold().Magenta().Text(n)} earned? ");
            var input = _input.ReadLine();
            if (input == null)
            {
                _output.WriteLine($"You must enter how many levels has {Bold().Magenta().Text(n)} earned? ");
                continue;
            }
            else
            {
                success = int.TryParse(input, out levels);
                if (success == false)
                {
                    _output.WriteLine(Bright.Red("Invalid choice. Please try again."));
                }
            }

            if ((success) && (levels >= 1))
            {
                int level = 0;
                success = int.TryParse(l, out level);
                nextLevel = level + levels;

                _output.WriteLine($"\r\nIs {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} ready to level up from level {Bold().Rgb(255, 165, 0).Text(l)} to level {Bold().Rgb(255, 165, 0).Text(nextLevel.ToString())}?");
                bool UpdCharacter = ConfirmYN();
                Console.Clear();
                if (UpdCharacter)
                {
                    _output.WriteLine($"The Dungeon Master has confirmed that {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is ready for the responsibility that comes with level {nextLevel} power.");
                    tmpPlayerCharacter.Level = nextLevel.ToString();
                    tmpPlayerCharacter.Updated = true; // indicates that this character was changed after the inital champaign was loaded
                    _updates = true;
                }
                else
                {
                    _output.WriteLine($"The Dungeon Master has confirmed that {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is not ready for such great power.");
                    _updates = false;
                }
                break;
            }
            else
            {
                _output.WriteLine($"We were unable to move {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} to the next level, please try again.");
            }
        }
    }

    public void LevelUpCharacters()
    {
        string? input = null;
        bool endInput = false;

        while (endInput != true)
        {
            _output.Clear();
            ListCampaign();
            if (_campaignList.Count() > 0)
            {
                _output.WriteLine("\r\n\r\nEnter a player number to level up.");
                input = _input.ReadLine();
                if (input == null)
                {
                    _output.WriteLine($"You must select a player number or enter {Yellow("0 (zero)")} to exit.");
                    endInput = false;
                }
                else
                {
                    int choice = -1;
                    bool success = Int32.TryParse(input, out choice);
                    if (success == true)
                    {
                        if (string.Compare((choice - 1).ToString(), (_campaignList.Count() - 1).ToString()) > 0)
                        {
                            _output.WriteLine($"Player number {Bright.Red().Text(input)} is not valid, please select another player number.");
                            endInput = false;
                        }
                        else
                        {
                            switch (input)
                            {
                                case "0":
                                    endInput = true;
                                    break;
                                default:
                                    int idx = -1;
                                    if (int.TryParse(input, out idx))
                                    {
                                        LevelUpCharacter(_campaignList[idx - 1]);
                                        endInput = true;
                                        break;
                                    }
                                    else
                                    {
                                        continue; // This has to be here otherwise the complier complains with an error.
                                    }
                            }
                        }
                    }
                    else
                    {
                        _output.WriteLine($"You must select a player number or enter {Yellow("0 (zero)")} to exit.");
                        endInput = false;
                    }
                }
            }
            else
            {
                // Message is displayed by the ListCharacters method so no need for one here
                endInput = true;
                break;
            }
        }
    }
    public void FindCharacters()
    {
        //###################################################################
        // stackoverflow.com/questions/45239889/linq-case-insensitive
        // var foundCharacter = _campaignList.Where(c => c.Name == "Bob").ToList();
        //###################################################################

        List<PlayerCharacter> foundCharacters = new List<PlayerCharacter>();

        while (true)
        {
            _output.Clear();
            _output.Write($"Which infamous hero or heroine are you in search of my leige (Enter {Yellow("<Done>")} to exit)? ");
            string input = _input.ReadLine();
            if (input.Length == 0)
            {
                _output.WriteLine("I don't believe I heard you correctly...");
                Thread.Sleep(2000);
            }
            else
            {
                if (input.ToLower() == "done")
                {
                    _output.Clear();
                    _output.WriteLine("Safe journey my leige, may the Great One watch over you.");
                    break;
                }
                else
                {
                    foundCharacters = _campaignList.Where(c => c.Name.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
                    //CharacterReader cr = new CharacterReader();
                    //foundCharacters = cr.CharacterSearch(input, _campaignList);
                    if (foundCharacters.Count() > 0)
                    {
                        ListCampaign(foundCharacters);
                    }
                    else
                    {
                        _output.Clear();
                        _output.WriteLine($"I do regret my leige that I did not find a reference in the archives that match your inquiry. Perhaps \r\nif you might grace the coffers of the temple with a small donation ({Green("Cha Ching")}), the Great One might \r\n show favor for your next request!");
                        _output.Write($"\r\nShall I search again for you, my liege (Enter {Yellow("<Done>")} to exit)? ");
                        input = _input.ReadLine();
                        if (input.ToLower() == "done")
                        {
                            _output.Clear();
                            _output.Write("Safe journey my leige. Please do remember to stop by our gift shop for bit of ale and something to remember your visit!");
                            Thread.Sleep(5000);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
    public void SaveCharacters()
    {
        if (_campaignList.Count == 0)
        {
            _output.WriteLine($"There are {Bright.Red().Text("no")} player character(s) in the current campaign.");
        }
        else
        {
            _output.Clear();
            if (_updates == true)
            {
                _output.WriteLine("Please wait while the scribes carve the tale of our courageous and mighty \r\nheroes on to the walls of the Temple of Champions!");

                //CharacterWriter cw = new CharacterWriter();
                //cw.characterlist = _campaignList;
                //fileHandler = new CSVFileHandler();

                //fileHandler.WriteCharacters(_filePath, _campaignList);
                string filePath = "\\Files\\input";
                IFileHandler _fileHandler = null;
                switch (_fileFormat)
                {
                    case "CSV":
                        _filePath = filePath + _supportedFileFormats["CSV"];
                        _fileHandler = new CSVFileHandler(_input, _output);
                        break;
                    case "JSON":
                        _filePath = filePath + _supportedFileFormats["JSON"];
                        _fileHandler = new JSONFileHandler(_input, _output);
                        break;
                    default:
                        _filePath = filePath + _supportedFileFormats["CSV"];
                        _fileHandler = new CSVFileHandler(_input, _output);
                        break;
                }
                _fileHandler.WriteFile(_filePath, _campaignList);
                _updates = false;
            }
            else
            {
                _output.WriteLine("Your campaign is all ready enshrined in the scrolls of history.");
            } 
            Thread.Sleep(4000);
        }
    }
    public void ChangeFileFormat()
    {
        string? input = null;
        bool endInput = false;
        bool success = false;
        int choice = 0;
        bool format_changed = false;
        string original_format = _fileFormat;
        while (endInput == false) 
        {
            _output.Clear();
            _output.WriteLine($"Current format is {Bright.Red().Text(_fileFormat)}, valid options are\r\n");
            int cnt = 1;
            foreach (KeyValuePair<string, string> fformat in _supportedFileFormats)
            {
                _output.WriteLine($"{cnt.ToString()}. {fformat.Key.ToString()}");
                cnt = cnt + 1;
            }
            _output.WriteLine($"\r\nPlease select an option, enter {Yellow("<Done>")} to keep current file format.");
            input = _input.ReadLine();
            success = Int32.TryParse(input, out choice );
            if (success == true && cnt > 1)
            {
                if (choice > _campaignList.Count() || choice < 1)
                {
                    _output.Write(Bright.Red($"Invalid choice. Please try again, press {Yellow("<Enter>")} key when you are ready to continue..."));
                    input = _input.ReadLine();
                }
                else
                {
                    _output.Clear();
                    _fileFormat = _supportedFileFormats.Keys.ElementAt(choice-1);
                    format_changed = true;
                }
            }
            else
            {
                if (input.ToLower() == "done")
                {
                    endInput = true;

                    if (format_changed == true && original_format == _fileFormat)
                    {
                        _output.Clear();
                        _output.WriteLine($"Out with the old and in with the new.  \r\n\r\nPlease wait while the 'Fates' search far and wide for a more worthy group of heros and heronies...");
                        Thread.Sleep(1000);
                        _campaignList.Clear();
                        string filePath = "\\Files\\input";
                        switch (_fileFormat)
                        {
                            case "CSV":
                                _filePath = filePath + _supportedFileFormats["CSV"];
                                _fileHandler = new CSVFileHandler(_input, _output);
                                _campaignList = _fileHandler.ReadFile(_filePath);
                                break;
                            case "JSON":
                                _filePath = filePath + _supportedFileFormats["JSON"];
                                _fileHandler = new JSONFileHandler(_input, _output);
                                _campaignList = _fileHandler.ReadFile(_filePath);
                                break;
                            default:
                                _filePath = filePath + _supportedFileFormats["CSV"];
                                _fileHandler = new CSVFileHandler(_input, _output);
                                _campaignList = _fileHandler.ReadFile(_filePath);
                                break;
                        }
                    }
                }
                else
                {
                    _output.Write($"Please select from one of the options, press {Yellow("<Enter>")} key when you are ready to continue...");
                    _input.ReadLine();
                }
            }
        }
    }
    public bool ExitProgram()
    {
        //TODO - Check if changes, ask, exit or not
        _output.Clear();
        if (_updates == true)
        {
            _output.WriteLine($"Do you want to save the tales of your champaign?");
            bool confirm = ConfirmYN();
            if (confirm)
            {
                SaveCharacters();
            }
        }
        else
        {
            _output.WriteLine($"Exiting...");
            Thread.Sleep(1000);
        }
        _output.Write($"\r\n\r\n{Yellow("Goodbye!")}");
        Thread.Sleep(1000);
        return true;
    }
}