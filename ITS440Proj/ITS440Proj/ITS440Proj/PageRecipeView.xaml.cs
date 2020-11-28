using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITS440Proj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRecipeView : ContentPage
    {
        private bool Saved;
        private Recipe recipe;
        /* TODO DEBUG
        public PageRecipeView()
        {
            InitializeComponent();
        }
        */
        public PageRecipeView(Recipe recipePass)
        {
            InitializeComponent();

            recipe = recipePass;

            entryYield.Text = recipe.yield;
            entryTags.Text = recipe.tags.ToString(); // fix

            buttonSave.Clicked += async (sender, e) =>
            {
                await DisplayAlert("Recipe Saved", "The recipe changes have been saved.", "OK");
            };
        }
    }
}