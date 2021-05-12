using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_Term_Sch.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Net.Mail;
using Xamarin.Essentials;
using Plugin.LocalNotifications;

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesScreen : ContentPage
    {
        private Courses _currentCourse;
        private SQLiteAsyncConnection _connection;
        public Courses _course;
        public Terms _term;
        public MainPage _main;
        public List<string> pickerStates = new List<string> { "In Progress", "Completed", "Dropped", "Plan To Take" };
        public List<string> pickerNotificationsStates = new List<string> { "Yes", "No" };
        public bool _firstAppearance;
        public CoursesScreen(Courses course)
        {
            _currentCourse = course;
            InitializeComponent();
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            _firstAppearance = true;

        }
        protected override void OnAppearing()
        {
            txtCourseTitle.Text = _currentCourse.CourseName;
            //pickerCourseStatus.SelectedItem = _currentCourse.CourseStatus;
            pickerCourseStatus.SelectedIndex = pickerStates.FindIndex(status => status == _currentCourse.CourseStatus);
            dpStartDate.Date = _currentCourse.Start.Date;
            dpEndDate.Date = _currentCourse.End.Date;
            txtInstructorName.Text = _currentCourse.InstructorName;
            txtInstructorPhone.Text = _currentCourse.InstructorPhone;
            txtInstructorEmail.Text = _currentCourse.InstructorEmail;
            txtNotes.Text = _currentCourse.Notes;
            
            if (_currentCourse.GetNotified == 0)
            {
                pickerNotifications.SelectedIndex = 0;
            } else
            {
                pickerNotifications.SelectedIndex = 1;
            }

            


            /*pickerCourseStatus.ItemsSource = pickerStates;
            pickerCourseStatus.SelectedIndex = pickerStates.FindIndex(status => status == _course.CourseStatus);
            txtCourseTitle.Text = _course.CourseName;
            pickerCourseStatus.SelectedItem = _course.CourseStatus;
            dpStartDate.Date = _course.Start.Date;
            dpEndDate.Date = _course.End.Date;
            txtInstructorName.Text = _course.InstructorName;
            txtInstructorPhone.Text = _course.InstructorPhone;
            txtInstructorEmail.Text = _course.InstructorEmail;
            txtNotes.Text = _course.Notes;
            if (_course.GetNotified == 0)
            {
                pickerNotifications.SelectedIndex = 0;
            }
            else
            {
                pickerNotifications.SelectedIndex = 1;
            }*/
            base.OnAppearing();
        }

        private async void btnEditCourse_Clicked(object sender, EventArgs e)
        {
            _currentCourse.CourseName = txtCourseTitle.Text;
            _currentCourse.Start = dpStartDate.Date;
            _currentCourse.End = dpEndDate.Date;
            _currentCourse.CourseStatus = (string)pickerCourseStatus.SelectedItem;
            _currentCourse.InstructorName = txtInstructorName.Text;
            _currentCourse.InstructorPhone = txtInstructorPhone.Text;
            _currentCourse.InstructorEmail = txtInstructorEmail.Text;
            _currentCourse.Notes = txtNotes.Text;
            _currentCourse.GetNotified = pickerNotifications.SelectedIndex;

            if(ValidField.NullCheck(txtInstructorName.Text) && ValidField.NullCheck(txtInstructorPhone.Text) && ValidField.NullCheck(txtCourseTitle.Text))
            {
                if (ValidField.EmailCheck(txtInstructorEmail.Text))
                {
                    if (_currentCourse.Start < _currentCourse.End)
                    {
                        await _connection.UpdateAsync(_currentCourse);

                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Alert!", "Please make sure the Start Date is before the End Date.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Alert!", "Please make sure the email address is valid before submitting.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alert!", "Please make sure all Fields are filled out before submitting.", "OK");
            }

        }

        public async Task ShareCourseNotes()
        {
           
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = txtNotes.Text,
                Title = "Share Course Notes"
            });

            
        }

        private async void btnShareNotes_Clicked(object sender, EventArgs e)
        {
            await ShareCourseNotes();
        }

        private void btnDiscardChanges_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnViewAssessments_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AssessmentsScreen(_currentCourse));
        }

        /*private bool IsUserInputValid()
        {
            bool valid = true;

            if (String.IsNullOrEmpty(txtCourseTitle.Text) ||
                pickerCourseStatus.SelectedItem == null ||
                dpStartDate.Date == null ||
                dpEndDate.Date == null ||
                dpEndDate.Date < dpStartDate.Date ||
                String.IsNullOrEmpty(txtInstructorName.Text) ||
                String.IsNullOrEmpty(txtInstructorEmail.Text) ||
                String.IsNullOrEmpty(txtInstructorPhone.Text) ||
                pickerNotifications.SelectedItem == null
                )

            {
                return false;
            }

            if (txtInstructorEmail.Text != null)
            {
                valid = IsEmailValid(txtInstructorEmail.Text);
            }


            return valid;
        }*/

        /*private bool IsEmailValid(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }*/

        private async void btnDeleteCourse_Clicked(object sender, EventArgs e)
        {
            // Fix this to work with new Database resolution
            var result = await DisplayAlert("Alert!", "Are you sure you want to delete this course?", "Yes", "No");

            if (result)
            {
                await _connection.DeleteAsync(_currentCourse);
                await Navigation.PopAsync();
            }
        }



    }
}