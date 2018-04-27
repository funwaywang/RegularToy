using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegularToy
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty MatchesProperty = DependencyProperty.Register("Matches", typeof(MatchResult[]), typeof(MainWindow));
        public static readonly RoutedUICommand SetOptionCommand = new RoutedUICommand("Set Option", "SetOption", typeof(MainWindow));
        public static readonly RoutedUICommand AddFavoriteCommand = new RoutedUICommand("Add Favorite", "AddFavorite", typeof(MainWindow));
        public static readonly RoutedUICommand DeleteFavoriteCommand = new RoutedUICommand("Delete Favorite", "DeleteFavorite", typeof(MainWindow));
        public static readonly RoutedUICommand UseFavoriteCommand = new RoutedUICommand("Use Favorite", "UseFavorite", typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }

        public MatchResult[] Matches
        {
            get { return (MatchResult[])GetValue(MatchesProperty); }
            set { SetValue(MatchesProperty, value); }
        }

        void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pattern;
                if (TxbRegular.SelectionLength > 0)
                    pattern = TxbRegular.SelectedText;
                else
                    pattern = TxbRegular.Text;

                var regex = new Regex(pattern, Options.RegexOptions);
                string text = TxbStr.Text;
                MessageBox.Show(this, regex.IsMatch(text) ? "Match" : "Do not match", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                Options.PushRegexHistory(pattern);
                Options.LastExpression = pattern;
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void BtnGetMatches_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxbRegular.Text))
            {
                MessageBox.Show(this, "Regular can't be empty.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string pattern;
                if (TxbRegular.SelectionLength > 0)
                    pattern = TxbRegular.SelectedText;
                else
                    pattern = TxbRegular.Text;

                var regex = new Regex(pattern, Options.RegexOptions);
                var matches = regex.Matches(TxbStr.Text);
                Matches = MatchResult.Build(matches);
                Options.LastExpression = pattern;
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        void BtnOptions_Click(object sender, RoutedEventArgs e)
        {
            var options = (from opt in Enum.GetValues(typeof(RegexOptions)).Cast<RegexOptions>()
                           where opt != RegexOptions.None
                           select new OptionItem
                           {
                               Option = opt,
                               Name = opt.ToString(),
                               Value = (Options.RegexOptions & opt) == opt
                           }).ToArray();
            LstOptions.ItemsSource = options;
            PopupOptions.IsOpen = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            TxbRegular.Text = Options.LastExpression;
            TxbStr.Text = Options.LastText;
            if(Options.MainFormSize.HasValue)
            {
                Width = Options.MainFormSize.Value.Width;
                Height = Options.MainFormSize.Value.Height;
            }
            WindowState = Options.WindowState;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Options.LastText = TxbStr.Text;
            Options.MainFormSize = new Size(Width, Height);
            Options.WindowState = WindowState;
        }

        void SetOptionCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Parameter is RegexOptions && e.OriginalSource is CheckBox)
            {
                var ckb = (CheckBox)e.OriginalSource;
                var opt = (RegexOptions)e.Parameter;

                if (ckb.IsChecked ?? false)
                    Options.RegexOptions |= opt;
                else
                    Options.RegexOptions &= ~opt;
            }
        }
        
        void BtnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            if(PopupFavorite.IsOpen && PopupFavorite.DataContext is Favorite)
            {
                var fav = (Favorite)PopupFavorite.DataContext;
                Favorites.List.Add(fav);

                try
                {
                    Favorites.Save();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                PopupFavorite.IsOpen = false;
                PopupFavorite.DataContext = null;
            }
        }

        void AddFavoriteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(TxbRegular.Text);
        }

        void AddFavoriteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TxbRegular.Text == string.Empty)
                return;

            string name;
            int index = 1;
            do
            {
                name = string.Format("My Favorite {0}", index);
                index++;
            }
            while (Favorites.List.Any(f => StringComparer.OrdinalIgnoreCase.Equals(f.Name, name)));

            var fav = new Favorite()
            {
                Name = name,
                Regular = TxbRegular.Text
            };
            PopupFavorite.DataContext = fav;
            PopupFavorite.IsOpen = true;
        }

        void BtnManageFavorites_Click(object sender, RoutedEventArgs e)
        {
            PopupFavorites.IsOpen = true;
        }

        void DeleteFavoriteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = LsvFavorites != null && LsvFavorites.SelectedItem != null;
        }

        void DeleteFavoriteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(LsvFavorites != null && LsvFavorites.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show(this, string.Format("Are you sure Delete {0} items?", LsvFavorites.SelectedItems.Count), Title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result != MessageBoxResult.OK)
                    return;

                var favs = LsvFavorites.SelectedItems.OfType<Favorite>().ToArray();
                foreach(var fav in favs)
                {
                    Favorites.List.Remove(fav);
                }

                try
                {
                    Favorites.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void UseFavoriteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = LsvFavorites != null && LsvFavorites.SelectedItems.Count == 1;
        }

        void UseFavoriteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(LsvFavorites != null && LsvFavorites.SelectedItem != null)
            {
                TxbRegular.Text = ((Favorite)LsvFavorites.SelectedItem).Regular;
            }

            PopupFavorites.IsOpen = false;
        }

        void BtnCloseFavorites_Click(object sender, RoutedEventArgs e)
        {
            PopupFavorites.IsOpen = false;
        }
    }
}
