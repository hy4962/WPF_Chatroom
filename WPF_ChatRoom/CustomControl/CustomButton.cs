using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPF_ChatRoom.CustomControl
{
    public class CustomButton : Button
    {
        /// <summary>
        /// icon属性
        /// </summary>
        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(CustomButton));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
