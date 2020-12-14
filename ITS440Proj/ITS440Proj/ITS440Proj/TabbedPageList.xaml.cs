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
    public partial class TabbedPageList : TabbedPage
    {
        public TabbedPageList()
        {
            InitializeComponent();

            CurrentPageChanged += CurrentPageHasChanged;
        }

        // gets the current page and updates it if special type
        private void CurrentPageHasChanged(object sender, EventArgs e) // super smart :)
        {
            var tabbedPage = (TabbedPage)sender; // gets the sender
            if (tabbedPage.CurrentPage.GetType().ToString() == "ITS440Proj.PageShoppingList") // if shopping list then update list
                ((PageShoppingList)tabbedPage.CurrentPage).updateList(); // update list
            else if(tabbedPage.CurrentPage.GetType().ToString() == "ITS440Proj.PageRecipe") // if recipe list then update list
                ((PageRecipe)tabbedPage.CurrentPage).updateList(); // update list
            else if (tabbedPage.CurrentPage.GetType().ToString() == "ITS440Proj.PageDiscover") // if discovery list then update list
                ((PageDiscover)tabbedPage.CurrentPage).fetchData(); // fetch data from database
        }
    }
}