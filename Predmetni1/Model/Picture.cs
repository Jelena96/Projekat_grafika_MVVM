using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Predmetni1.Model
{
    public class Picture:ValidationBase
    {
        private string title;
        private string description;
        private string imagePath;
        private string dateTime;

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");

                }
            }
        }

        public string DateTime
        {
            get { return dateTime; }
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    OnPropertyChanged("DateTime");

                }
            }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged("ImagePath");

                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.title))
            {
                this.ValidationErrors["Title"] = "Title is required.";
            }
            if (string.IsNullOrWhiteSpace(this.imagePath))
            {
                this.ValidationErrors["ImagePath"] = "ImagePath field cannot be empty.";
            }
        }


    }

    public class ImageItem
    {
        public string Source { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
    }
}
