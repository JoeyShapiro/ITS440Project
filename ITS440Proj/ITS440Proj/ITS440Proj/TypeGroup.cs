using System;
using System.Collections.Generic;
using System.Text;

namespace ITS440Proj
{
    public class TypeGroup : List<MVVM.ObservableItem>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public TypeGroup()
        {
        }
    }
}
