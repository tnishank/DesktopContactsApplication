using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using DesktopContactsApplication.Classes;
using SQLite;

namespace DesktopContactsApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Contact> contacts;
        public MainWindow()
        {
            InitializeComponent();
            contacts = new List<Contact>();
            ReadDatabase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();
            ReadDatabase();
        }

        public void ReadDatabase()
        {
            
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Contact>();
                contacts = connection.Table<Contact>().ToList();
            }

            if(contacts != null)
            {
                /*foreach (var c in contacts)
                {
                    contactListView.Items.Add(new ListViewItem()
                    {
                        Content = c
                    });
                }*/
                contactListView.ItemsSource = contacts;
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchItem = sender as TextBox;
            //var filteredList = contacts.Where(c => c.Name.ToLower().Contains(searchItem.Text.ToLower())).ToList();
            var filteredList = (from c in contacts
                           where c.Name.ToLower().Contains(searchItem.Text.ToLower())
                           orderby c.Name
                           select c).ToList();
            contactListView.ItemsSource = filteredList;
        }

        private void contactListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact contact = (Contact)contactListView.SelectedItem;

            if(contact != null)
            {
                ContactDetailsWindow newContactWindow = new ContactDetailsWindow(contact);
                newContactWindow.ShowDialog();
                ReadDatabase();
            }
        }
    }
}
