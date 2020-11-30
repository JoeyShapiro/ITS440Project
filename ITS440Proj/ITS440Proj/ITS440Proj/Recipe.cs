using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ITS440Proj
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } // primary key
        public ObservableCollection<MVVM.ObservableItem> ingredients = new ObservableCollection<MVVM.ObservableItem>(); // TODO maybe make TypeGroup
        public ObservableCollection<string> instructions = new ObservableCollection<string>(); // TODO maybe make TypeGroup
        public string title { get; set; } // title of recipe
        public string yield { get; set; } // how many the recipe will make
        public ObservableCollection<string> tags = new ObservableCollection<string>(); // list of tags for searching for the recipe

        public Recipe()
        {
            
        }
    }
}
