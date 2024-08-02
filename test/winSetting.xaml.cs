using LinePutScript.Converter;
using LinePutScript.Localization.WPF;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using VPet_Simulator.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Panuon.WPF.UI;
using System.Diagnostics;
using System.Windows.Shapes;
using LinePutScript;

namespace VPET.Evian.Throw
{
    /// <summary>
    /// winSetting.xaml 的交互逻辑
    /// </summary>
    public partial class winSetting : Window
    {
        Throw vts;

        public winSetting(Throw vts)
        {///前两行代码不可缺少
            InitializeComponent();
            this.vts = vts;
            SwitchOn.IsChecked = vts.IsOpen;
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
            Close();
        }

    }
}
