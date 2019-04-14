using Predmetni1.Model;
using Predmetni1.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Predmetni1
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<User> Users { get; set; }
        public MyICommand DeleteCommand { get; private set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand LogInCommand { get; set; }
        private UserViewModel userViewModel;
        private BindableBase currentViewModel;
        private User currentUser = new User();
        private string pSText;
        private string nText;

        public MainWindowViewModel()
        {
            LoadUsers();
            AddCommand = new MyICommand(Registration);
            LogInCommand = new MyICommand(LogIn);

        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        public void LoadUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>() { new User { Name = "Jelena", Password = "996"},
            new User{ Name = "Natalija", Password = "996"} };
            Users = users;
        }

        public string NText
        {
            get { return nText; }
            set
            {
                if (nText != value)
                {
                    nText = value;
                    OnPropertyChanged("NText");
                }
            }
        }

        

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        private void Registration()
        {
            XDocument doc = XDocument.Load("Test.xml");
            bool exsist = false;

            foreach (XElement cell in doc.Element("Instagram").Elements("User"))
            {
                if (cell.Element("Name").Value == CurrentUser.Name)
                {
                    exsist = true;
                    NText = "User already exist!";
                }
            }
                CurrentUser.Validate();


            if (CurrentUser.IsValid && !exsist)
            {
                Users.Add(CurrentUser);
                CurrentViewModel = userViewModel;
                userViewModel = new UserViewModel(CurrentUser.Name);

                if (!File.Exists("Test.xml"))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineOnAttributes = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create("Test.xml", xmlWriterSettings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement("Instagram");

                        xmlWriter.WriteStartElement("User");
                        xmlWriter.WriteElementString("Name", CurrentUser.Name);
                        xmlWriter.WriteElementString("Password", CurrentUser.Password);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                }
                else
                {

                    XDocument xDocument = XDocument.Load("Test.xml");
                    XElement root = xDocument.Element("Instagram");
                    IEnumerable<XElement> rows = root.Descendants("User");
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                       new XElement("User",
                       new XElement("Name", CurrentUser.Name),
                       new XElement("Password", CurrentUser.Password)));
                    xDocument.Save("Test.xml");
                }
            }
        }

      

        private void LogIn() {

            XDocument doc = XDocument.Load("Test.xml");
            var names = from name in doc.Root.Elements("User")
                            where name.Element("Name").Value.Contains(CurrentUser.Name)
                            select name;
            if (!names.Count().Equals(0))
            {
                CurrentViewModel = userViewModel;
                userViewModel = new UserViewModel(CurrentUser.Name);


            }
            else {
                NText = "User not exist!";

            }

        }

    }
}

