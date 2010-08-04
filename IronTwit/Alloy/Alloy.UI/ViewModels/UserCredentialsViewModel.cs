﻿using System.Windows;

namespace Unite.UI.ViewModels
{
    public class UserCredentialsViewModel : DependencyObject
    {
        public static DependencyProperty UserNameProperty = DependencyProperty
            .Register("UserName", typeof (string), typeof (UserCredentialsViewModel));
        public string UserName
        {
            get
            {
                return (string)GetValue(UserNameProperty);
            }
            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        public static DependencyProperty PasswordProperty = DependencyProperty
            .Register("Password", typeof(string), typeof(UserCredentialsViewModel));
        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        public static DependencyProperty SavePasswordProperty = DependencyProperty
            .Register("SavePassword", typeof (bool), typeof (UserCredentialsViewModel));
        public bool SavePassword
        {
            get
            {
                return (bool) GetValue(SavePasswordProperty);
            }
            set
            {
                SetValue(SavePasswordProperty, value);
            }
        }

        public string Caption { get; set; }
    }
}
