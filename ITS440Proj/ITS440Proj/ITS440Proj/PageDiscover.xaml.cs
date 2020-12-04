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
        public IEnumerable<object> Data { get; private set; }

        public PageDiscover()
        {
            InitializeComponent();

            fetchData();

            entrySearch.Text = Data.ToString();
        }

        private async Task fetchData()
        {
            var credentials = new Amazon.CognitoIdentity.CognitoAWSCredentials("arn:aws:iam::963244653868:role/Cognito_ITS440ProjUnauth_Role", Amazon.RegionEndpoint.USEast1);
            var ddbClient = new Amazon.DynamoDBv2.AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);

            var results = await ddbClient.ScanAsync(new ScanRequest
            {
                TableName = "Recipes",
                AttributesToGet = new List<string> { "id", "title", "yield", "ingredientsBlobbed", "instructionsBlobbed", "tagsBlobbed"}
            });

            Data = results.Items.Select(i => new
            {
                id = i["id"].S,
                title = i["title"].S,
                yield = i["yield"].S,
                ingredientsBlobbed = i["ingredientsBlobbed"].S,
                instructionsBlobbed = i["instructionsBlobbed"].S,
                tagsBlobbed = i["tagsBlobbed"]
            }).OrderBy(i => i.id);
        }
    }
}