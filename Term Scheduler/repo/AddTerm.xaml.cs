using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_Term_Sch.Classes;

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTerm : ContentPage
    {
        public MainPage mainPage;
        private SQLiteAsyncConnection _connection;
        public AddTerm(MainPage main)
        {
            mainPage = main;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            InitializeComponent();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var newTerm = new Terms();
            newTerm.TermName = txtTermTitle.Text;
            newTerm.Start = dpStartDate.Date;
            newTerm.End = dpEndDate.Date;

            if (newTerm.Start < newTerm.End)
            {
                if (ValidField.NullCheck(newTerm.TermName))
                {
                    await _connection.InsertAsync(newTerm);

                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Alert!", "Please make sure the Term Name is filled out.", "OK");
                }
                
            } else
            {
                await DisplayAlert("Alert!", "Please make sure the Start Date is before the End Date.", "OK");
            }

        }
        /*private bool IsUserInputValid()
        {
            bool valid = true;

            if (txtTermTitle.Text == null ||
                dpStartDate.Date == null ||
                dpEndDate.Date == null ||
                dpEndDate.Date < dpStartDate.Date
                )

            {
                return false;
            }
            return valid;
        }*/

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}