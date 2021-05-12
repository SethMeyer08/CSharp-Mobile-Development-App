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
    public partial class TermsScreen : ContentPage
    {
        //private Terms _term;
        private SQLiteAsyncConnection _connection;
        public ObservableCollection<Courses> _courseList;
        private Terms _currentTerm;

        public TermsScreen(Terms term)
        {
            _currentTerm = term;
            InitializeComponent();
            _connection = DependencyService.Get<SQLiteDb>().GetConnection();
            coursesListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(ItemTapped);
        }

        protected override async void OnAppearing()
        {
            Title = _currentTerm.TermName;
            termStart.Text = _currentTerm.Start.ToString("MM/dd/yyyy");
            termEnd.Text = _currentTerm.End.ToString("MM/dd/yyyy");

            await _connection.CreateTableAsync<Courses>();
            var courseList = await _connection.QueryAsync<Courses>($"SELECT * FROM Courses WHERE Term = '{_currentTerm.Id}'");
            _courseList = new ObservableCollection<Courses>(courseList);
            coursesListView.ItemsSource = _courseList;

            base.OnAppearing();
        }



        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Courses course = (Courses)e.Item;
            await Navigation.PushAsync(new CoursesScreen(course));
        }

        /*public Terms _term;
        public MainPage _main;
        private SQLiteAsyncConnection _connection;
        private Assessments _assessments;

        public TermsScreen(Assessments assessments)
        {
            InitializeComponent();
            _assessments = assessments;
        }

        public TermsScreen(Terms term, MainPage main)
        {
            _term = term;
            _main = main;
            InitializeComponent();
            coursesListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(ItemTapped);
            Title = term.TermName;


        }
        protected override async void OnAppearing()
        {
            termStart.Text = _term.Start.ToString("MM/dd/yyyy");
            termEnd.Text = _term.End.ToString("MM/dd/yyyy");
            /*using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Courses>();
                var coursesForTerm = conn.Query<Courses>($"SELECT * FROM Courses WHERE Term = '{_term.Id}'");
                coursesListView.ItemsSource = coursesForTerm;
            }
            await Navigation.PushAsync(new TermsScreen(_terms));
        }

        */
        async private void btnAddCourse_Clicked(object sender, EventArgs e)
        {
            var count = 0;
            var courseCount = await _connection.QueryAsync<Courses>($"SELECT * FROM Courses WHERE Term = '{_currentTerm.Id}'");

            count = courseCount.Count;

            if (count < 6)
            {
                await Navigation.PushModalAsync(new AddCourse(_currentTerm));
            }
            else
            {
                await DisplayAlert("Alert!", "You cannot exceed more than six courses in a term.", "OK");
            }
        }

        private async void btnDeleteTerm_Clicked(object sender, EventArgs e)
        {
            // delete assessments, then course
            var result = await DisplayAlert("Alert!", "Ready to delete this term?", "Yes", "No");
            if (result)
            {
                await _connection.DeleteAsync(_currentTerm);
                await Navigation.PopAsync();
            }
        }


        private async void btnEditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTerms(_currentTerm));
        }

    }
}