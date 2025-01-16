// Throw, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// VPET.Evian.Throw.winSetting
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using LinePutScript;
using Panuon.WPF.UI;
namespace VPET.Evian.Throw
{
    public partial class winSetting : Window
    {
        private Throw vts;

        public winSetting(Throw vts)
        {
            InitializeComponent();
            this.vts = vts;
            SwitchOn.IsChecked = vts.IsOpen;
            LimitSpeedXBox.Text = vts.LimitSpeedX.ToString();
            LimitSpeedYBox.Text = vts.LimitSpeedY.ToString();
            MulSpeedXBox.Text = vts.MulSpeedX.ToString();
            MulSpeedYBox.Text = vts.MulSpeedY.ToString();
            MulSpeed.Text = vts.MulSpeed.ToString();
            Grid.SetColumn(Save, 1);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            vts.winSetting = null;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (vts.IsOpen != SwitchOn.IsChecked.Value)
            {
                vts.IsOpen = Convert.ToBoolean(SwitchOn.IsChecked.Value);
            }
            vts.LimitSpeedX = Convert.ToDouble(LimitSpeedXBox.Text);
            if (vts.LimitSpeedX < 0.0)
            {
                vts.LimitSpeedX = 0.0;
            }
            vts.MW.GameSavesData["Throw"][(gdbe)"LimitSpeedX"] = vts.LimitSpeedX;
            vts.LimitSpeedY = Convert.ToDouble(LimitSpeedYBox.Text);
            if (vts.LimitSpeedY < 0.0)
            {
                vts.LimitSpeedY = 0.0;
            }
            vts.MW.GameSavesData["Throw"][(gdbe)"LimitSpeedY"] = vts.LimitSpeedY;
            vts.MulSpeedX = Convert.ToDouble(MulSpeedXBox.Text);
            if (vts.MulSpeedX > 10.0)
            {
                vts.MulSpeedX = 10.0;
            }
            vts.MW.GameSavesData["Throw"][(gdbe)"MulSpeedX"] = vts.MulSpeedX;
            vts.MulSpeedY = Convert.ToDouble(MulSpeedYBox.Text);
            if (vts.MulSpeedY > 10.0)
            {
                vts.MulSpeedY = 10.0;
            }
            vts.MW.GameSavesData["Throw"][(gdbe)"MulSpeedY"] = vts.MulSpeedY;
            vts.MulSpeed = Convert.ToDouble(MulSpeed.Text);
            if (vts.MulSpeed > 10.0)
            {
                vts.MulSpeed = 10.0;
            }
            vts.MW.GameSavesData["Throw"][(gdbe)"MulSpeed"] = vts.MulSpeed;
            Close();
        }

        private void TestMode_Checked(object sender, RoutedEventArgs e)
        {
            if (TestMode.IsChecked == true)
            {
                LimitSpeedXBox.Visibility = Visibility.Visible;
                LimitSpeedYBox.Visibility = Visibility.Visible;
                LimitSpeedXBlock.Visibility = Visibility.Visible;
                LimitSpeedYBlock.Visibility = Visibility.Visible;
                MulSpeedXBox.Visibility = Visibility.Visible;
                MulSpeedXBlock.Visibility = Visibility.Visible;
                MulSpeedYBox.Visibility = Visibility.Visible;
                MulSpeedYBlock.Visibility = Visibility.Visible;
                MulSpeed.Visibility = Visibility.Visible;
                MulSpeedBlock.Visibility = Visibility.Visible;
                Grid.SetColumn(Save, 2);
            }
            else
            {
                LimitSpeedXBox.Visibility = Visibility.Collapsed;
                LimitSpeedXBlock.Visibility = Visibility.Collapsed;
                LimitSpeedYBox.Visibility = Visibility.Collapsed;
                LimitSpeedYBlock.Visibility = Visibility.Collapsed;
                MulSpeedXBox.Visibility = Visibility.Collapsed;
                MulSpeedXBlock.Visibility = Visibility.Collapsed;
                MulSpeedYBox.Visibility = Visibility.Collapsed;
                MulSpeedYBlock.Visibility = Visibility.Collapsed;
                MulSpeed.Visibility = Visibility.Collapsed;
                MulSpeedBlock.Visibility = Visibility.Collapsed;
                Grid.SetColumn(Save, 1);
            }
        }
    }
}
