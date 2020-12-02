using SQLite;
using SQLiteNetExtensions.Attributes;
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
        [TextBlob("ingredientsBlobbed")]
        public ObservableCollection<MVVM.ObservableItem> ingredients { get; set; } // TODO maybe make TypeGroup
        [TextBlob("instructionsBlobbed")]
        public ObservableCollection<string> instructions { get; set; } // TODO maybe make TypeGroup
        public string title { get; set; } // title of recipe
        public string yield { get; set; } // how many the recipe will make
        [TextBlob("tagsBlobbed")]
        public ObservableCollection<string> tags { get; set; } // list of tags for searching for the recipe

        public string ingredientsBlobbed { get; set; }
        public string instructionsBlobbed { get; set; }
        public string tagsBlobbed { get; set; }

        public Recipe()
        {
            
        }
    }
}
