using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.General.Scripts
{
    public class ScriptValidationException : Exception
    {
        public class ScriptValidationError
        {
            public string FunctionName { get; set; }
            public int LineNumber { get; set; }
            public string Description { get; set; }
        }
        public List<ScriptValidationError> ScriptValidationErrors { get; set; } = new List<ScriptValidationError>();
        public string FileName { get; set; }
    }
}
