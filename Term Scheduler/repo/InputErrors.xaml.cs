using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputErrors : ContentPage
    {
        public InputErrors()
        {
            InitializeComponent();
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}