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

            entryTitle.Text = recipe.title;
            entryYield.Text = recipe.yield;
            foreach (var tag in recipe.tags) // clean
            {
                entryTags.Text += tag + ",";
            }
            entryIng.Text = recipe.ingredients.ToString();
            entryIns.Text = recipe.instructions.ToString();

            buttonSave.Clicked += async (sender, e) =>
            {
                if(entryTitle.Text != "" && entryTags.Text != "" && entryYield.Text != "")
                {
                    recipe.title = entryTitle.Text;
                    recipe.yield = entryYield.Text;
                    var tags = entryTags.Text.Split(','); // splits tags and puts in array
                    recipe.tags.Clear(); // clears to readd tags
                    foreach (var tag in tags) // add tags to list
                    {
                        recipe.tags.Add(tag);
                    }
                    // do ing
                    // do ins

                    await App.Recipedb.UpdateItemAsync(recipe);

                    Saved = true;
                    await DisplayAlert("Recipe Saved", "The recipe changes have been saved.", "OK");
                } else
                {
                    await DisplayAlert("Recipe Not Saved", "Please fill in all the fields to save.", "OK");
                }
            };

            buttonAddIng.Clicked += async (sender, e) =>
            {
                if(entryIng.Text != null && entryAmount.Text != null && pickerAmount.SelectedIndex != -1)
                {
                    var ing = new MVVM.ObservableItem { Title = entryIng.Text, Description = "", Quantity = int.Parse(entryAmount.Text) }; // TODO maybe Food : ObservableItem
                    recipe.ingredients.Add(ing);
                }
            };

            buttonAddIns.Clicked += async (sender, e) =>
            {
                if(entryIns.Text != null || entryIns.Text != "")
                {
                    recipe.instructions.Add(entryIns.Text);
                }
            };
        }

        protected override async void OnDisappearing() // TODO clean up repeat
        {
            base.OnDisappearing();

            if (!Saved)
            {
                bool Should = await DisplayAlert("Save?", "You have not saved changed, would you like to.", "OK", "NO");
                if (Should)
                {
                    if (entryTitle.Text != "" || entryTags.Text != "" || entryYield.Text != "")
                    {
                        recipe.title = entryTitle.Text;
                        recipe.yield = entryYield.Text;
                        // do tags
                        // do ing
                        // do ins

                        await App.Recipedb.UpdateItemAsync(recipe);

                        Saved = true;
                        await DisplayAlert("Recipe Saved", "The recipe changes have been saved.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Recipe Not Saved", "Please fill in all the fields to save.", "OK");
                    }
                }
            }
        }
    }
}