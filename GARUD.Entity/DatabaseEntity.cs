using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GARUD.Entity
{
    public class DatabaseEntity 
    {
        public List<string> DatabaseNames { get; set; }


        public ObservableCollection<TestCase> TestCaseList { get; set; }
    }
}
