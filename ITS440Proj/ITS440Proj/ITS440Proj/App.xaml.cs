using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITS440Proj
{
    public partial class App : Application
    {
        static ProjectDatabase database;
        static RecipeDatabase recipedb;

        public static ProjectDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ProjectDatabase();
                }
                return database;
            }
        }
        public static RecipeDatabase Recipedb
        {
            get
            {
                if (recipedb == null)
                {
                    recipedb = new RecipeDatabase();
                }
                return recipedb;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TabbedPageList()); // TODO MainPage = new TabbedPageList(); use this somehow but setup
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
