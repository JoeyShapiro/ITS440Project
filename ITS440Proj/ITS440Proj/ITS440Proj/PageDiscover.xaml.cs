using Amazon.DynamoDBv2.Model;
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
    public partial class PageDiscover : ContentPage
    {
        public List<Recipe> Data = new List<Recipe>();

        public PageDiscover()
        {
            InitializeComponent();

            fetchData();
        }

        public void fetchData()
        {
            var credentials = new Amazon.CognitoIdentity.CognitoAWSCredentials("us-east-1:9f0203b1-af69-4b1b-a7bd-8f2f3248e309", Amazon.RegionEndpoint.USEast1);
            var ddbClient = new Amazon.DynamoDBv2.AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);

            Task.Run(async () => // grab items from database
            {
                var results = await ddbClient.ScanAsync(new ScanRequest
                {
                    TableName = "Recipe",
                    AttributesToGet = new List<string> { "ID", "title", "yield", "ingredientsBlobbed", "instructionsBlobbed", "tagsBlobbed" }
                });

                foreach (var item in results.Items)
                {
                    var tempRecipe = new Recipe { title = item["title"].S, yield = item["yield"].S, ingredientsBlobbed = item["ingredientsBlobbed"].S, instructionsBlobbed = item["instructionsBlobbed"].S, tagsBlobbed = item["tagsBlobbed"].S };
                    Data.Add(tempRecipe);
                }
            }).Wait();

            listData.ItemsSource = Data;
        }

        private void OnDownload(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var oi = (Recipe)mi.CommandParameter;

            App.Recipedb.InsertItemAsync(oi);

            DisplayAlert("Recipe Downloaded", "The recipe for \""+ oi.title + "\" has been downloaded and added to your recipe list.", "OK");
        }
    }
}