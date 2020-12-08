using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
                recipe = new Recipe { title = "recipe", ingredientsBlobbed = "test@1@Units\n", instructionsBlobbed = "test\n", tagsBlobbed = "", yield = "1" };
                await App.Recipedb.InsertItemAsync(recipe); // creates a new row in the database
                
                await Navigation.PushAsync(new PageRecipeView(recipe, this));

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

        public void updateList()
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

            await Navigation.PushAsync(new PageRecipeView(recipe, this));
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
                    tempIng.Quantity = convert2oz(tempIng.Quantity, temp[2]);
                    await App.Database.InsertItemAsync(tempIng);
                }
            }

            await DisplayAlert("Added To List", "The Ingredients for this recipe have been added to the shopping list.", "OK");
        }

        private async void OnUpload(object sender, EventArgs e) // put here so all it has to do is upload recipe rather than pack in view then send
        {
            var mi = ((MenuItem)sender);
            var oi = (Recipe)mi.CommandParameter;

            var credentials = new Amazon.CognitoIdentity.CognitoAWSCredentials("us-east-1:9f0203b1-af69-4b1b-a7bd-8f2f3248e309", Amazon.RegionEndpoint.USEast1);
            var ddbClient = new Amazon.DynamoDBv2.AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(ddbClient);

            await context.SaveAsync(oi);
            
            //await ddbClient.UpdateItemAsync(new UpdateItemRequest
            //{
            //    TableName = "Recipes",
            //    AttributeUpdates = new Dictionary<string, AttributeValueUpdate> 
            //    {
            //        { "title", new AttributeValueUpdate { Value = new AttributeValue { S = "title" } } },
            //        { "yield", new AttributeValueUpdate { Value = new AttributeValue { S = "1" } } },
            //        { "ingredientsBlobbed", new AttributeValueUpdate { Value = new AttributeValue { S = "test" } } },
            //        { "instructionsBlobbed", new AttributeValueUpdate { Value = new AttributeValue { S = "test" } } },
            //        { "tagsBlobbed", new AttributeValueUpdate { Value = new AttributeValue { S = "tset" } } }
            //    }
            //});
        }

        public int convert2oz(int quantity, string units)
        {
            int ounces = 0;

            if (units == "oz")
            {
                ounces = quantity;
            }
            else if (units == "lbs")
            {
                ounces = quantity * 16;
            }
            else if (units == "cups")
            {
                ounces = quantity * 8;
            }
            else if (units == "tsp")
            {
                ounces = quantity / 6;
            }
            else
            {
                ounces = quantity;
            }

            return ounces;
        }
    }
}