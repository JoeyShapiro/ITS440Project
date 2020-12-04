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
    public partial class PageRecipe : ContentPage
    {
        private ObservableCollection<Recipe> recipes; // grabs list from database

        public PageRecipe()
        {
            InitializeComponent();

            updateList();
            Recipe recipe;

            buttonAdd.Clicked += async (sender, e) =>
            {
                recipe = new Recipe { title = "recipe", ingredientsBlobbed = "test@1\n", instructionsBlobbed = "test\n", tagsBlobbed = "", yield = "1" };
                await App.Recipedb.InsertItemAsync(recipe); // creates a new row in the database
                
                await Navigation.PushAsync(new PageRecipeView(recipe));

                updateList();
            };

            buttonSearch.Clicked += async (sender, e) =>
            {
                var search = entrySearch.Text;
                var results = new ObservableCollection<Recipe>();

                foreach (var r in recipes.Where(r => r.title == search)) // checks for all occurances of the search querry
                    results.Add(r);

                listRecipes.ItemsSource = results;

                if (search == "" || search == null) // go back to list but find better way also incorporate null into searches
                    listRecipes.ItemsSource = recipes;
            };
        }

        private void updateList()
        {
            Task.Run(async () => // grab items from database
            {
                var table = await App.Recipedb.GetItemsAsync();
                recipes = new ObservableCollection<Recipe>(table);
            }).Wait();

            listRecipes.ItemsSource = recipes; // sets items source to grabbed items
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender); // cast to menu item
            var oi = (Recipe)mi.CommandParameter; // cast to list item

            await App.Recipedb.DeleteItemAsync(oi.ID); // delete item
            await DisplayAlert("Delete", oi.ID + " Deleted", "OK");

            updateList(); // update list
        }

        public async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender); // cast to menu item
            var recipe = (Recipe)mi.CommandParameter; // cast to list item

            await Navigation.PushAsync(new PageRecipeView(recipe));
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var recipe = (Recipe)mi.CommandParameter;

            // add ing to list
            var ingdeblob = recipe.ingredientsBlobbed.Split('\n'); // array of foods ( FOOD\n )
            foreach (var item in ingdeblob)
            {
                if (item != "")
                {
                    var temp = item.Split('@'); // array of item vars ( Title@Quantity )
                    var tempIng = new MVVM.ObservableItem { Title = temp[0], Description = "", Quantity = int.Parse(temp[1]), Got = false};
                    await App.Database.InsertItemAsync(tempIng);
                }
            }

            await DisplayAlert("Added To List", "The Ingredients for this recipe have been added to the shopping list.", "OK");
        }
    }
}