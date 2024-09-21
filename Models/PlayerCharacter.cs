using CsvHelper.Configuration;

public class PlayerCharacter
{
    private string? _name;
    private string? _classname;
    private string? _level;
    private string? _hitpoints;
    private List<string>? _equipment;
    private int _id;
    private bool _updated;

    public string? Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string? ClassName
    {
        get { return _classname; }
        set { _classname = value; }
    }
    public string? Level
    {
        get { return _level; }
        set { _level = value; }
    }
    public string? HP
    {
        get { return _hitpoints; }
        set { _hitpoints = value; }
    }
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public bool Updated
    {
        get { return _updated; }
        set { _updated = value; }
    }
    public string Equipment
    {
        get
        {
            // This allows me to output the list of equipment with a unique delimiter based on need of where it is being called.
            string? str = null;
            for (int i = 0; i < _equipment.Count(); i++)
            {
                if (str == null)
                {
                    str = _equipment[i];
                }
                else
                {
                    str += "|" + _equipment[i];
                }
            };
            return str;
        }
        set
        {
            List<string>? equip = new List<string>();
            equip = value.Split("|").ToList();
            this._equipment = equip;
        }
    }
    public PlayerCharacter()
    {
        // This is a do nothing constructor.  It is needed because we have a constructor all ready and without
        // it the CSV Helper would not work.  It expects to use the default get/set and cannot because I created
        // the constructor with parameters. 
    }
    public PlayerCharacter(string Name, string ClassName, string Level)
    {
        _name = Name;
        _level = Level;
        _classname = ClassName;
        _hitpoints = "0";
        _equipment = new List<string>();
        _updated = false;
    }
    public PlayerCharacter(string Name, string ClassName, string Level, string HP, string Equip, int Id)
    {
        _name = Name;
        _level = Level;
        _classname = ClassName;
        _hitpoints = HP;
        _equipment = new List<string>(Equip.Split('|'));
        _id = Id;
        _updated = false;
    }
    public string PrintEquipment(string delimiter = "|")
    {
        if (delimiter == "|")
        {
            return Equipment;
        }
        else
        {
            // This allows me to output the list of equipment with a unique delimiter based on need of where it is being called.
            string? str = null;
            for (int i = 0; i < _equipment.Count(); i++)
            {
                if (str == null)
                {
                    str = _equipment[i];
                }
                else
                {
                    str += delimiter + _equipment[i];
                }
            };
            return str;
        }
    }
    public sealed class CharacterPlayerMap : ClassMap<PlayerCharacter>
    {
        //  This map allows you to skip additonal fields added to the main class when you read/write to a file.
        public CharacterPlayerMap()
        {
            Map(m => m.Name);
            Map(m => m.ClassName);
            Map(m => m.Level);
            Map(m => m.HP);
            Map(m => m.Equipment);
        }
    }
}