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
    public partial class PageShoppingList : ContentPage
    {
        private MVVM.ListViewModel items = new MVVM.ListViewModel(); // grabs list from database

        private ObservableCollection<TypeGroup> grouped = new ObservableCollection<TypeGroup>();
        private TypeGroup foodGet = new TypeGroup() { LongName = "Get", ShortName = "E" };
        private TypeGroup foodGot = new TypeGroup() { LongName = "Got", ShortName = "O" };

        public PageShoppingList()
        {
            InitializeComponent();

            updateList();
            MVVM.ObservableItem item;

            buttonAdd.Clicked += async (sender, e) =>
            {
                item = new MVVM.ObservableItem { Title = "Food", Description = "A test Food" , Quantity = 1, Got = false };
                await App.Database.InsertItemAsync(item); // creates a new row in the database

                await Navigation.PushAsync(new PageShoppingAdd(item, this));

                updateList(); // updates list
            };

            listShopping.ItemTapped += async (sender, e) => // TODO find way to move to button
            {
                item = (MVVM.ObservableItem)e.Item; // sets item to the tapped item
                item.Got = !item.Got; // toggles if the item is gotten

                await App.Database.UpdateItemAsync(item); // update table with changed item
                updateList(); // update list
            };
        }

        private async void listShopButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Found", "Item has been found.", "OK");
        }

        public void updateList()
        {
            items = new MVVM.ListViewModel(); // grab items from database
            foodGet.Clear();
            foodGot.Clear();
            grouped.Clear();

            foreach (var food in items.Items) // sift through items and add to list
            {
                if (food.Got)
                {
                    foodGot.Add(food);
                }
                else
                {
                    foodGet.Add(food);
                }
            }

            grouped.Add(foodGet);
            grouped.Add(foodGot);

            listShopping.ItemsSource = grouped; // sets items source to grabbed items
        }

        public async void OnDelete (object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender); // cast to menu item
            var oi = (MVVM.ObservableItem)mi.CommandParameter; // cast to list item

            await App.Database.DeleteItemAsync(oi.ID); // delete item
            await DisplayAlert("Delete", oi.ID + " Deleted", "OK");

            updateList(); // update list
        }

        public async void OnEdit (object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender); // cast to menu item
            var item = (MVVM.ObservableItem)mi.CommandParameter; // cast to list item

            await Navigation.PushAsync(new PageShoppingAdd(item, this));
        }
    }
}