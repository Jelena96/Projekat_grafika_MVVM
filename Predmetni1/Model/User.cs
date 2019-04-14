using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predmetni1.Model
{
    public class User : ValidationBase
    {
        private string name;
        private string password;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    RaisePropertyChanged("Password");
                   
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected override void ValidateSelf()
        {
            //if (string.IsNullOrWhiteSpace(this.name))
            //{
            //    this.ValidationErrors["Name"] = "Name is required.";
            //}
             if (this.name[0]>48 && this.name[0]<57)
            {
                this.ValidationErrors["Name"] = "First character couldnt be a number.";
            }
            //if (string.IsNullOrWhiteSpace(this.password))
            //{
            //    this.ValidationErrors["Password"] = "Password is required.";
            //}
             if (this.password.Count()<=6)
            {
                this.ValidationErrors["Password"] = "Password is too short";
            }
        }
    }
}
