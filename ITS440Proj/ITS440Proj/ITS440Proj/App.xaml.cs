using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITS440Proj
{
    public partial class App : Application
    {
        static ProjectDatabase database;

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
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage()); // TODO MainPage = new TabbedPageList(); use this somehow but setup
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
