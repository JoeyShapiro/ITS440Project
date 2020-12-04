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
        public string title { get; set; } // title of recipe
        public string yield { get; set; } // how many the recipe will make

        public string ingredientsBlobbed { get; set; } // blobbed list of ingredients
        public string instructionsBlobbed { get; set; } // blobbed list of instructions
        public string tagsBlobbed { get; set; } // blobbed list of tags for searching for the recipe

        public Recipe()
        {
            
        }
    }
}
