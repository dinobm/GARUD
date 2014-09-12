using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GARUD.Entity
{
    public class DatabaseObject
    {
        public List<string> DatabaseNamesList { get; set; }

        public ObservableCollection<TestCase> TestCaseList { get; set; }
    }
}
