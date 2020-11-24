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
    public partial class PageShoppingAdd : ContentPage
    {
        private bool Saved;
        private MVVM.ObservableItem item;
        /* TODO DEBUG
        public PageShoppingAdd()
        {
            InitializeComponent();
        }
        */

        public PageShoppingAdd(MVVM.ObservableItem itemPass)
        {
            InitializeComponent();

            item = itemPass; // see if best way

            entryFood.Text = item.Title;
            entryDescription.Text = item.Description;
            entryQuantity.Text = item.Quantity.ToString();
            pickerAmount.SelectedIndex = 1;

            buttunSave.Clicked += async (sender, e) =>
            {
                if (entryFood.Text == "" || entryDescription.Text == "" || entryQuantity.Text == "" || pickerAmount.SelectedIndex == -1)
                {
                    await DisplayAlert("Invalid Input", "Input is missing in a required field", "OK");
                    return;
                }

                item.Title = entryFood.Text;
                item.Description = entryDescription.Text;
                item.Quantity = convert2oz(int.Parse(entryQuantity.Text),(string)pickerAmount.ItemsSource[pickerAmount.SelectedIndex]); // TODO clean up

                await App.Database.UpdateItemAsync(item);

                Saved = true;
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
                    if (entryFood.Text == "" || entryDescription.Text == "" || entryQuantity.Text == "")
                    {
                        await DisplayAlert("Invalid Input", "Input is missing in a required field", "OK");
                        return;
                    }

                    item.Title = entryFood.Text;
                    item.Description = entryDescription.Text;
                    item.Quantity = int.Parse(entryQuantity.Text);

                    await App.Database.UpdateItemAsync(item);
                }
            }
        }

        public int convert2oz(int quantity, string units)
        {
            int ounces = 0;

            if(units == "oz")
            {
                ounces = quantity;
            } else if (units == "lbs")
            {
                ounces = quantity * 16;
            } else if (units == "cups")
            {
                ounces = quantity * 8;
            } else if (units == "tsp")
            {
                ounces = quantity / 6;
            } else
            {
                ounces = quantity;
            }

            return ounces;
        }
    }
}