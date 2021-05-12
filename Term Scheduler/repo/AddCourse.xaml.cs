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

namespace C971_Term_Sch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCourse : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private Terms _currentTerm;
        /*Dictionary<string, bool> notificationsDict = new Dictionary<string, bool>
        {
            {"Yes", true },
            {"No", false }
        };*/

        public AddCourse(Terms currentTerm)
        {
            _currentTerm = currentTerm;
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            //terms = term;
            //mainPage = main;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Courses>();

            base.OnAppearing();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
                var newCourse = new Courses();

                newCourse.CourseName = txtCourseTitle.Text;
                newCourse.CourseStatus = (string)pickerCourseStatus.SelectedItem;
                newCourse.Start = dpStartDate.Date;
                newCourse.End = dpEndDate.Date;
                newCourse.InstructorName = txtCourseInstructorName.Text;
                newCourse.InstructorEmail = txtInstructorEmail.Text;
                newCourse.InstructorPhone = txtInstructorPhone.Text;
                newCourse.Notes = txtNotes.Text;
                newCourse.GetNotified = pickerNotifications.SelectedIndex;
                newCourse.Term = _currentTerm.Id;

            if (ValidField.NullCheck(txtCourseInstructorName.Text) && ValidField.NullCheck(txtInstructorPhone.Text) && ValidField.NullCheck(txtCourseTitle.Text))
            {
                if (ValidField.EmailCheck(txtInstructorEmail.Text))
                {
                    if (newCourse.Start < newCourse.End)
                    {
                        await _connection.InsertAsync(newCourse);

                        await Navigation.PopModalAsync();
                    }
                    else
                        await DisplayAlert("Alert!", "Please make sure the start date is before the end date.", "OK");
                }
                else
                    await DisplayAlert("Alert!", "Please make sure your email is valid before submit.", "OK");
            }
            else
                await DisplayAlert("Alert!", "Please make sure all fields are filled out before submit.", "OK");
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

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
        }

        private bool IsUserInputValid()
        {
            bool valid = true;

            if (txtCourseTitle == null ||
                pickerCourseStatus.SelectedItem == null ||
                dpStartDate.Date == null ||
                dpEndDate.Date == null ||
                dpEndDate.Date < dpStartDate.Date ||
                txtCourseInstructorName == null ||
                txtInstructorEmail.Text == null ||
                txtInstructorPhone == null ||
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

    }
}