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
    public partial class PageRecipe : ContentPage
    {
        public PageRecipe()
        {
            InitializeComponent();

            buttonAdd.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new PageRecipeView());
            };
        }
    }
}