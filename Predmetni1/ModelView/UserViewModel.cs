using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml.Linq;
using Predmetni1.Model;
using System.Windows.Media.Imaging;

namespace Predmetni1.ModelView
{
    public class UserViewModel : BindableBase
    {
        public static string Username;

        private string imagePath;
        private string source;
        private string homeViewStartMessage = "Hello!";
        private int selectedIndex;//otvaraj mi drugi
        private string description;
        private string title;
        public List<string> Pictures = new List<string>();
        public List<string> picturesList = new List<string>();
        public ObservableCollection<Picture> imageList = new ObservableCollection<Picture>();
        private Picture currentPicture = new Picture();
        ObservableCollection<object> _children;
        public MyICommand UpdateCommand { get; set; }
        private User updateUser = new User();


        public UserViewModel(string name)
        {
            if (name != null)
                Username = name;
            

        }

        public UserViewModel()
        {
            UpdateCommand = new MyICommand(ChangeData);
            _children = new ObservableCollection<object>();
           // _children.Add(new ChangeDataModelView(Username));
        }

        public ObservableCollection<object> Children { get { return _children; } }


        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = 2; }

        }

        

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        public List<string> PicturesList
        {
            get { return picturesList; }
            set
            {
                picturesList = value;
                OnPropertyChanged("picturesList");
            }
        }

        public ObservableCollection<Picture> ImageList
        {
            get { return imageList; }
            set
            {
                imageList = value;
                OnPropertyChanged("imageList");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public User UpdateUser
        {
            get { return updateUser; }
            set
            {
                updateUser = value;
                 OnPropertyChanged("UpdateUser");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        ICommand _loadImageCommand;
        public ICommand LoadImageCommand
        {
            get
            {
                if (_loadImageCommand == null)
                {
                    _loadImageCommand = new MyICommand(LoadImage);
                }
                return _loadImageCommand;
            }
        }

        ICommand _loadAllImagesCommand;
        public ICommand LoadAllImagesCommand
        {
            get
            {
                if (_loadAllImagesCommand == null)
                {
                    _loadAllImagesCommand = new MyICommand(ReturnAllPictures);
                }
                return _loadAllImagesCommand;
            }
        }

        public Picture CurrentPicture
        {
            get { return currentPicture; }
            set
            {
                currentPicture = value;
                OnPropertyChanged("CurrentPicture");
            }
        }

        private void LoadImage()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = (".png");
            open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

            if (open.ShowDialog() == true)
                CurrentPicture.ImagePath = open.FileName;//ovde se setuje
        }

        ICommand _saveImageCommand;
        public ICommand SaveImageCommand
        {
            get
            {
                if (_saveImageCommand == null)
                {
                    _saveImageCommand = new MyICommand(SaveImage);
                }
                return _saveImageCommand;
            }
        }

        

        private void SaveImage()
        {
            CurrentPicture.Validate();

            if (CurrentPicture.IsValid)
            {
                if (!File.Exists("Images.xml"))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineOnAttributes = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create("Images.xml", xmlWriterSettings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement("Instagram");

                        xmlWriter.WriteStartElement("User");
                        xmlWriter.WriteElementString("Name", Username);
                        xmlWriter.WriteElementString("Title", CurrentPicture.Title);
                        xmlWriter.WriteElementString("Description", CurrentPicture.Description);
                        xmlWriter.WriteElementString("DateTime", DateTime.Now.ToString());
                        xmlWriter.WriteElementString("ImagePath", CurrentPicture.ImagePath);

                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                }
                else
                {

                    XDocument xDocument = XDocument.Load("Images.xml");
                    XElement root = xDocument.Element("Instagram");
                    IEnumerable<XElement> rows = root.Descendants("User");
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                    new XElement("User",
                    new XElement("Name", Username),
                    new XElement("Title", CurrentPicture.Title),
                    new XElement("Description", CurrentPicture.Description),
                    new XElement("DateTime", DateTime.Now.ToString()),
                    new XElement("ImagePath", CurrentPicture.ImagePath)));

                    xDocument.Save("Images.xml");
                }
            }
        }

        

        private void ReturnAllPictures()
        {

            XDocument doc = XDocument.Load("Images.xml");
            var pictures = from name in doc.Root.Elements("User")
                           where name.Element("Name").Value.Contains(Username)
                           select name.Element("ImagePath").Value;

             PicturesList = pictures.ToList();

            foreach (string pl in PicturesList)
            {
                var titles = from name in doc.Root.Elements("User")
                             where name.Element("ImagePath").Value.Contains(pl)
                             select name.Element("Title").Value;
                List<string> titlesStr = titles.ToList();

                var descriptions = from name in doc.Root.Elements("User")
                                   where name.Element("ImagePath").Value.Contains(pl)
                                   select name.Element("Description").Value;
                List<string> descrpStr = descriptions.ToList();

                var dateTime = from name in doc.Root.Elements("User")
                                   where name.Element("ImagePath").Value.Contains(pl)
                                   select name.Element("DateTime").Value;
                List<string> dateTimeStr = dateTime.ToList();

                Picture p = new Picture();
                p.ImagePath = pl;
                p.Title = titlesStr[0];
                p.Description = descrpStr[0];
                p.DateTime = dateTimeStr[0];
                imageList.Add(p);

            }

            ImageList = imageList;
           
        }

        private void ChangeData()
        {
            string nameU;
            string pas;
            XDocument doc = XDocument.Load("Test.xml");
            XDocument doc2 = XDocument.Load("Images.xml");

            var names = from name in doc.Root.Elements("User")
                        where name.Element("Name").Value.Contains(Username)
                        select name;

            var password = from name in doc.Root.Elements("User")
                        where name.Element("Name").Value.Contains(Username)
                        select name.Element("Password").Value;

            List<string> ps = password.ToList();
            if (!names.Count().Equals(0))
            {
                    names.ToList().ForEach(x => x.Remove());
                    doc.Save("Test.xml");

                    if (UpdateUser.Name != null)
                        nameU = UpdateUser.Name;
                    else
                        nameU = Username;

                    if (UpdateUser.Password != null)
                        pas = UpdateUser.Password;
                    else
                        pas = ps[0];

                    XDocument xDocument = XDocument.Load("Test.xml");
                    XElement root = xDocument.Element("Instagram");
                    IEnumerable<XElement> rows = root.Descendants("User");
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                       new XElement("User",
                       new XElement("Name", nameU),
                       new XElement("Password", pas)));
                    xDocument.Save("Test.xml");


                var q = from name in doc2.Root.Elements("User")
                               where name.Element("Name").Value.Contains(Username)
                               select name;

                if (!q.Count().Equals(0))
                {
                    foreach (XElement cell in doc2.Element("Instagram").Elements("User"))
                    {
                        if (cell.Element("Name").Value == Username)
                        {
                            cell.Element("Name").Value = UpdateUser.Name;
                        }
                    }

                    doc2.Save("Images.xml");
                }
            }
        }
    }
}
