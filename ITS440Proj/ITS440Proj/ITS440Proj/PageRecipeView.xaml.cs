using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Food> ingredients = new ObservableCollection<Food>();
        private ObservableCollection<string> instructions = new ObservableCollection<string>();
        private string tags;
        private PageRecipe pageRecipe;
        /* TODO DEBUG
        public PageRecipeView()
        {
            InitializeComponent();
        }
        */
        public PageRecipeView(Recipe recipePass, PageRecipe parentPage)
        {
            InitializeComponent();

            recipe = recipePass;
            pageRecipe = parentPage;

            // deserialize data
            var ingdeblob = recipe.ingredientsBlobbed.Split('\n'); // array of foods ( FOOD\n )
            foreach (var item in ingdeblob)
            {
                if (item != "")
                {
                    var temp = item.Split('@'); // array of item vars ( ID@Title@Quantity )
                    var tempIng = new Food { Title = temp[0], Description = "", Quantity = int.Parse(temp[1]), Units = temp[2] };
                    ingredients.Add(tempIng);
                }
            }
            var insdeblob = recipe.instructionsBlobbed.Split('\n'); // array of instructions ( INSTRUCTION\n )
            foreach (var item in insdeblob)
            {
                if (item != "") // if item exists change to remove maybe
                    instructions.Add(item);
            }
            tags = recipe.tagsBlobbed; // can be a string with commas

            entryTitle.Text = recipe.title;
            entryYield.Text = recipe.yield;
            entryTags.Text = tags;
            listIng.ItemsSource = ingredients;
            listIns.ItemsSource = instructions;
            
            buttonSave.Clicked += async (sender, e) =>
            {
                if(entryTitle.Text != "" && entryTags.Text != "" && entryYield.Text != "")
                {
                    recipe.title = entryTitle.Text;
                    recipe.yield = entryYield.Text;
                    var tags = entryTags.Text.Split(','); // splits tags and puts in array
                    // do ing
                    // do ins

                    // serialize data \n shows new entry and @ shows new section
                    recipe.ingredientsBlobbed = "";
                    foreach (var food in ingredients)
                        recipe.ingredientsBlobbed += food.Title + '@' + food.Quantity + '@' + food.Units + '\n';
                    recipe.instructionsBlobbed = "";
                    foreach (var step in instructions)
                    {
                        recipe.instructionsBlobbed += step + '\n';
                    }
                    recipe.tagsBlobbed = entryTags.Text; // user can split using comma

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
                    if (!entryIng.Text.Contains('@') && !entryAmount.Text.Contains('@'))
                    {
                        var ing = new Food { Title = entryIng.Text, Description = "", Quantity = int.Parse(entryAmount.Text), Units = pickerAmount.SelectedItem.ToString() }; // TODO maybe Food : ObservableItem
                        ingredients.Add(ing);

                        entryIng.Text = "";
                        entryAmount.Text = "";
                        pickerAmount.SelectedIndex = -1;
                    }
                }
            };

            buttonAddIns.Clicked += async (sender, e) =>
            {
                if(entryIns.Text != null || entryIns.Text != "")
                {
                    instructions.Add(entryIns.Text);

                    entryIns.Text = "";
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
                        var tags = entryTags.Text.Split(','); // splits tags and puts in array
                                                              // do ing
                                                              // do ins

                        // serialize data \n shows new entry and @ shows new section
                        recipe.ingredientsBlobbed = "";
                        foreach (var food in ingredients)
                            recipe.ingredientsBlobbed += food.Title + '@' + food.Quantity + '@' + food.Units + '\n';
                        recipe.instructionsBlobbed = "";
                        foreach (var step in instructions)
                        {
                            recipe.instructionsBlobbed += step + '\n';
                        }
                        recipe.tagsBlobbed = entryTags.Text; // user can split using comma

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

            pageRecipe.updateList();
        }

        public void IngOnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var oi = (Food)mi.CommandParameter;

            ingredients.Remove(oi);

            listIng.ItemsSource = ingredients;
        }

        public void InsOnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var oi = (string)mi.CommandParameter;

            instructions.Remove(oi);

            listIns.ItemsSource = instructions;
        }
    }
}