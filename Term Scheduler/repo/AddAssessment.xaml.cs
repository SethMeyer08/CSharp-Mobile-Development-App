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
    public partial class AddAssessment : ContentPage
    {
        private Courses _course;
        private SQLiteAsyncConnection _connection;

        public AddAssessment(Courses course)
        {
            
            InitializeComponent();
            _course = course;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Assessments>();
            var assessmentList = await _connection.QueryAsync<Assessments>($"SELECT AssessType FROM Assessments WHERE Course = '{_course.Id}'");

            foreach (Assessments assessment in assessmentList)
            {
                if (String.IsNullOrEmpty(assessment.AssessType))
                {
                    pickerAssessmentType.Items.Add("Objective");
                    pickerAssessmentType.Items.Add("Performance");
                }
                else
                {
                    return;
                }
            }

            base.OnAppearing();
        }

        private void btnDiscardChanges_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        async private void btnAddAssessment_Clicked(object sender, EventArgs e)
        {
            if (ValidField.NullCheck(txtAssessmentName.Text))
            {
                Assessments newAssessment = new Assessments();
                newAssessment.AssessmentName = txtAssessmentName.Text;
                newAssessment.End = dpDueDate.Date;
                newAssessment.Course = _course.Id;
                newAssessment.GetNotified = pickerNotifications.SelectedIndex;
                newAssessment.AssessType = (string)pickerAssessmentType.SelectedItem;

                var objCount = 0;
                var objectiveCount = await _connection.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE Course = '{_course.Id}' AND AssessType = 'Objective'");
                objCount = objectiveCount.Count;

                var perfCount = 0;
                var performanceCount = await _connection.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE Course = '{_course.Id}' AND AssessType = 'Performance'");
                perfCount = performanceCount.Count;

                if (newAssessment.AssessType.ToString() == "Objective" && objCount == 0)
                {
                    await _connection.InsertAsync(newAssessment);
                    await Navigation.PopModalAsync();
                } else if (newAssessment.AssessType.ToString() == "Performance" && perfCount == 0)
                {
                    await _connection.InsertAsync(newAssessment);
                    await Navigation.PopModalAsync();
                } else
                {
                    await DisplayAlert("Alert!", "You cannot have more than one of the same assessment type.", "OK");
                }

                }
                else
                {
                    await DisplayAlert("Alert!", "You must fill out all the information.", "OK");
                }
        }
    }
}