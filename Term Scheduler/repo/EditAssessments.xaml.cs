using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_Term_Sch.Classes;
using System.Collections.ObjectModel;

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessments : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Assessments _assessment;
        public Courses _currentCourse;
        //private ObservableCollection<Assessments> _assessmentList;
        public List<string> pickerStates = new List<string> { "Objective", "Performance" };
        public List<string> pickerNotificationsStates = new List<string> { "Yes", "No" };

        public EditAssessments(Assessments assessment)
        {
            InitializeComponent();
            _assessment = assessment;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
        }

        protected override void OnAppearing()
        {
            /*pickerAssessmentType.ItemsSource = pickerStates;
            pickerAssessmentType.SelectedIndex = pickerStates.FindIndex(status => status == _assessment.AssessType);
            txtAssessmentName.Text = _assessment.AssessmentName;
            dpDueDate.Date = _assessment.End.Date;

            if (_assessment.GetNotified == 0)
            {
                pickerNotifications.SelectedIndex = 0;
            } else
            {
                pickerNotifications.SelectedIndex = 1;
            }*/

            txtAssessmentName.Text = _assessment.AssessmentName;
            pickerAssessmentType.SelectedItem = _assessment.AssessType;
            dpDueDate.Date = _assessment.End;

            if (_assessment.GetNotified == 0)
            {
                pickerNotifications.SelectedIndex = 0;
            } else
            {
                pickerNotifications.SelectedIndex = 1;
            }

            base.OnAppearing();
        }

        private void btnDiscardChanges_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void btnDeleteAssess_Clicked(object sender, EventArgs e)
        {
            var result = await this.DisplayAlert("CAUTION!", "Do you really wish to delete this assessment?", "Yes", "No");

            if (result)
            {
                await _connection.DeleteAsync(_assessment);
                await Navigation.PopModalAsync();
            }
        }

        

        private async void btnEditCourse_Clicked(object sender, EventArgs e)
        {

            await _connection.UpdateAsync(_assessment);
            await Navigation.PopModalAsync();
            
        }
    }


}
        /*var objCount = _connection.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE AssessType = 'Objective'");
        var perfCount = _connection.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE AssessType = 'Performance'");

        var isAssessmentObjective = _assessment.AssessType.ToString() == "Objective";
        var isAssessmentPerformance = _assessment.AssessType.ToString() == "Performance";*/