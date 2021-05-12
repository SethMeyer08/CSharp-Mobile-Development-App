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
    public partial class EditTerms : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Terms _currentTerm;

        public EditTerms(Terms currentTerm)
        {
            InitializeComponent();
            _currentTerm = currentTerm;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Terms>();
            txtTermTitle.Text = _currentTerm.TermName;
            dpStartDate.Date = _currentTerm.Start;
            dpEndDate.Date = _currentTerm.End;

            base.OnAppearing();
        }

        private async void btnSaveChanges_Clicked(object sender, EventArgs e)
        {
                _currentTerm.TermName = txtTermTitle.Text;
                _currentTerm.Start = dpStartDate.Date;
                _currentTerm.End = dpEndDate.Date;

            if (_currentTerm.Start < _currentTerm.End)
            {
                if (ValidField.NullCheck(_currentTerm.TermName))
                {
                    await _connection.UpdateAsync(_currentTerm);

                    await Navigation.PopAsync();
                } else
                {
                    await DisplayAlert("Alert!", "Please make sure the Term Title is not blank.", "OK");
                }
            } else
            {
                await DisplayAlert("Alert!", "Please make sure the Start Date is before the End Date!", "OK");
            }

}

        /*private bool isInputValid()
        {
            bool valid = true;

            if (txtTermTitle.Text == null || dpStartDate.Date == null || dpEndDate.Date == null || dpEndDate.Date < dpStartDate.Date)
            {
                return false;
            }

            return valid;
        }*/

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}