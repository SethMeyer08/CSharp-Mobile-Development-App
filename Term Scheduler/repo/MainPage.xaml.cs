using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using C971_Term_Sch.Classes;
using Plugin.LocalNotifications;
using System.Collections.ObjectModel;

namespace C971_Term_Sch
{
    public partial class MainPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        public ObservableCollection<Terms> _termList;
        private bool _firstAppearance = true;
        public List<Terms> terms = new List<Terms>();
        public List<Courses> courses = new List<Courses>();
        public List<Assessments> assessments = new List<Assessments>();

        public MainPage main;
        //bool isInitRound = true;
        public MainPage()
        {
            InitializeComponent();
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            termsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(ItemTapped);
            main = this;
        }

        protected override async void OnAppearing()
        {
            /*using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Terms>();
                conn.CreateTable<Courses>();
                conn.CreateTable<Assessments>();

                terms = conn.Table<Terms>().ToList();

                

            }*/

            await _connection.CreateTableAsync<Terms>();
            await _connection.CreateTableAsync<Courses>();
            await _connection.CreateTableAsync<Assessments>();

            var termList = await _connection.Table<Terms>().ToListAsync();

            if (!termList.Any())
            {
                // Seed the Term information!
                Terms newTerm = new Terms();
                newTerm.TermName = "Term 1";
                newTerm.Start = new DateTime(2021, 04, 15);
                newTerm.End = new DateTime(2021, 09, 30);
                await _connection.InsertAsync(newTerm);
                termList.Add(newTerm);

                // Seed that Course information!
                Courses newCourse = new Courses();
                newCourse.Term = newTerm.Id;
                newCourse.CourseName = "C971 - Mobile Development";
                newCourse.CourseStatus = "Completed";
                newCourse.Start = new DateTime(2021, 04, 16);
                newCourse.End = new DateTime(2021, 04, 24);
                newCourse.InstructorName = "Seth Meyer";
                newCourse.InstructorEmail = "smeye58@my.wgu.edu";
                newCourse.InstructorPhone = "765-716-4014";
                newCourse.Notes = "Be careful about Path Length. If it's too long, that issue can cause you to waste hours and feel that you're going crazy.";
                newCourse.GetNotified = 1;
                await _connection.InsertAsync(newCourse);

                // Seed the Object Assessment, yo!
                Assessments newObjectiveAssessment = new Assessments();
                newObjectiveAssessment.AssessmentName = "L337";
                newObjectiveAssessment.Start = new DateTime(2021, 05, 25);
                newObjectiveAssessment.End = new DateTime(2021, 05, 25);
                newObjectiveAssessment.AssessType = "Objective";
                newObjectiveAssessment.Course = newCourse.Id;
                newObjectiveAssessment.GetNotified = 1;
                await _connection.InsertAsync(newObjectiveAssessment);

                // Finally, let's seed the Performance Assessment, d00d!
                Assessments newPerformanceAssessment = new Assessments();
                newPerformanceAssessment.AssessmentName = "P0WN";
                newPerformanceAssessment.Start = new DateTime(2021, 05, 30);
                newPerformanceAssessment.End = new DateTime(2021, 05, 30);
                newPerformanceAssessment.AssessType = "Performance";
                newPerformanceAssessment.Course = newCourse.Id;
                newPerformanceAssessment.GetNotified = 1;
                await _connection.InsertAsync(newPerformanceAssessment);
            }

            var courseList = await _connection.Table<Courses>().ToListAsync();
            var assessmentList = await _connection.Table<Assessments>().ToListAsync();


            if (_firstAppearance)
            {
                _firstAppearance = false;

                int courseId = 0;

                foreach (Courses course in courseList)
                {
                    courseId++;
                    
                    if(course.GetNotified == 1)
                    {
                        if ((course.Start - DateTime.Now).TotalDays < 3)
                            CrossLocalNotifications.Current.Show("Reminder ", $"{course.CourseName} is starting on {course.Start}!");
                        if ((course.End - DateTime.Now).TotalDays < 3)
                            CrossLocalNotifications.Current.Show("Reminder ", $"{ course.CourseName} is ending on {course.End}!");

                    }
                }

                int assessmentId = courseId;
                
                foreach (Assessments assessment in assessmentList)
                {
                    assessmentId++;
                    if(assessment.GetNotified == 1)
                    {
                        /*if ((assessment.Start - DateTime.Now).TotalDays < 3)
                            CrossLocalNotifications.Current.Show("Reminder ", $"{assessment.AssessmentName} starts on {assessment.Start}!");*/
                        if ((assessment.End - DateTime.Now).TotalDays < 3)
                            CrossLocalNotifications.Current.Show("Reminder ", $"{assessment.AssessmentName} ends on {assessment.End}!");
                    }
                }
            }

            _termList = new ObservableCollection<Terms>(termList);
            termsListView.ItemsSource = _termList;

            base.OnAppearing();
        }

        async private void btnNewTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddTerm(this));
        }

        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Terms term = (Terms)e.Item;
            await Navigation.PushAsync(new TermsScreen(term));
        }
    }
}
