using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_Term_Sch.Classes;

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsScreen : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Courses _currentCourse;
        public Assessments _assessments;
        private ObservableCollection<Assessments> _assessmentList;
        public AssessmentsScreen(Courses currentCourse)
        {
            InitializeComponent();
            Title = currentCourse.CourseName;
            _currentCourse = currentCourse;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            AssessmentsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Assessment_Tapped);
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Assessments>();
            var assessmentList = await _connection.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE Course = '{_currentCourse.Id}'");
            _assessmentList = new ObservableCollection<Assessments>(assessmentList);
            AssessmentsListView.ItemsSource = _assessmentList;

            base.OnAppearing();
        }

        async void Assessment_Tapped(object sender, ItemTappedEventArgs e)
        {
            Assessments assessment = (Assessments)e.Item;
            await DisplayAlert("Alert!", "You cannot edit the Assessment's Type. If you need to change the type, delete the assessment and choose the correct Assessment Type.", "Continue");
            await Navigation.PushModalAsync(new EditAssessments(assessment));
        }

        async private void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async private void btnNewAssessment_Clicked(object sender, EventArgs e)
        {
            int assessmentsCount = 0;
            //bool assessmentCheck = true;
            
            foreach (Assessments assessment in _assessmentList)
            {
                if (assessment.AssessType == "Objective" || assessment.AssessType == "Performance")
                    assessmentsCount++;
            }

            if (assessmentsCount == 2)
            {
                await DisplayAlert("Alert!", "You already have two assessments.", "OK");
                //assessmentCheck = false;
            } else
            {
                await Navigation.PushModalAsync(new AddAssessment(_currentCourse));
            }

            
            // I only need two assessments per course; One Objective and One Performance
            /*if (getAssessmentCount() < 2)
            {
                await Navigation.PushModalAsync(new AddAssessment(_assessment));
            } else
            {
                await DisplayAlert("Alert!", "You cannot have more than 1 Objective and 1 Performance Assessment per each Course!", "OK");
            }*/
        }

        /*int getAssessmentCount()
        {
            int count = 0;
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                var assessmentCount = conn.Query<Assessments>($"SELECT * FROM Assessments WHERE Course = '{_course.Id}'");
                count = assessmentCount.Count;
            }
            return count;
        }*/
    }
}