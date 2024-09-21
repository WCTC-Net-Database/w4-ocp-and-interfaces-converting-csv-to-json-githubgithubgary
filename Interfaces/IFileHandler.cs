using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerCharacter;

namespace Assignment4.Interfaces
{
    public interface IFileHandler
    {
        List<PlayerCharacter> ReadFile(string filePath);
        void WriteFile(string filePath, List<PlayerCharacter> characters);
    }
}
