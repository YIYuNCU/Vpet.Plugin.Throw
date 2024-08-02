using LinePutScript;
using LinePutScript.Localization.WPF;
using Panuon.WPF.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;
using System.Windows.Media;
using System.Threading;
using System.Windows.Input;
using System.Security.AccessControl;
using static VPet_Simulator.Core.GraphInfo;
using System.Xml.Linq;

namespace VPET.Evian.Throw
{
    public class Throw : MainPlugin
    {
        public bool IsOpen = true;
        private bool IsCheck = false;
        private bool IsBegin = false;
        private double LLeft = -1.0;
        private double LUp = -1.0;
        private double SpeedX;
        private double SpeedY;
        private double SpeedYO;
        public override string PluginName => "Throw";
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public Throw(IMainWindow mainwin) : base(mainwin)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
        }
        /// <summary>
        /// 当桌宠开启，加载mod的时候会调用这个函数
        /// </summary>
        public override void LoadPlugin()
        {
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("IsOpen")))
                IsOpen = MW.GameSavesData["Throw"][(gbol)"IsOpen"];
            else
                IsOpen = true;
            ///添加列表项
            MenuItem modset = MW.Main.ToolBar.MenuMODConfig;
            modset.Visibility = Visibility.Visible;
            var menuItem = new MenuItem()
            {
                Header = "Throw".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            menuItem.Click += (s, e) => { Setting(); };
            modset.Items.Add(menuItem);
            ///添加定时器
            DispatcherTimer mSpeedtimer = new DispatcherTimer();
            mSpeedtimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            mSpeedtimer.Tick += new EventHandler(MSpeedtimer);
            DispatcherTimer mUItimer = new DispatcherTimer();
            mUItimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            mUItimer.Tick += new EventHandler(MUItimer);
            MW.Main.MainGrid.MouseLeftButtonUp += new MouseButtonEventHandler(this.MainGrid_MouseLeftButtonUp);
            for (int i = 0; i < 4; i++)
            {
                IGameSave.ModeType m = (IGameSave.ModeType)i;
                MW.Core.TouchEvent.Insert(0,new TouchArea(MW.Core.Graph.GraphConfig.TouchRaisedLocate[i], MW.Core.Graph.GraphConfig.TouchRaisedSize[i],
                    () =>
                    {
                        if (MW.Core.Save.Mode == m)
                        {
                            MCheck(); 
                        }
                        return false;
                    }, true));
            }
            mUItimer.Start();
            mSpeedtimer.Start();
        }
        long presstime;
        public void MCheck()
        {
            if (IsOpen)
            {
                IsCheck = true;
                IsBegin = false;
                MW.Main.MoveTimer.Enabled = false;
            }
        }
        private void MSpeedtimer(object? sender, EventArgs e)
        {
            if (!IsOpen)
            {
                return;
            }
            if (IsCheck == true)
            {
                var Left = MW.Core.Controller.GetWindowsDistanceLeft();
                var Up = MW.Core.Controller.GetWindowsDistanceUp();
                var MLeft = Left;
                var MUp = Up;
                if (LLeft > 0)
                {
                    if (Math.Abs(SpeedX) > 10)
                    {
                        SpeedX = (SpeedX + (MLeft - LLeft)*100) / 2;
                    }
                    else
                    {
                        SpeedX = MLeft - LLeft;
                        SpeedX *= 100;
                    }
                    if (Math.Abs(SpeedY) > 10)
                    {
                        SpeedY = (SpeedY + (MUp - LUp)*100) / 2;
                    }
                    else
                    {
                        SpeedY = MUp - LUp;
                        SpeedY *= 100;
                    }
                }
                if(Math.Abs(SpeedX) < 100)
                {
                    SpeedX = 0;
                }
                if (Math.Abs(SpeedY) < 100)
                {
                    SpeedY = 0;
                }
                SpeedY = Math.Min(SpeedY, 2000);
                SpeedYO = SpeedY;
                LLeft = MLeft;
                LUp = MUp;
            }
        }
        public bool CheckPosition() => MW.Dispatcher.Invoke(() =>
               MW.Core.Controller.GetWindowsDistanceUp() < -0.05 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceDown() < System.Windows.SystemParameters.PrimaryScreenHeight
            || MW.Core.Controller.GetWindowsDistanceDown() < -0.05 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceUp() < System.Windows.SystemParameters.PrimaryScreenHeight
            || MW.Core.Controller.GetWindowsDistanceLeft() < -0.1 * MW.Main.ActualWidth && MW.Core.Controller.GetWindowsDistanceRight() < System.Windows.SystemParameters.PrimaryScreenWidth
            || MW.Core.Controller.GetWindowsDistanceRight() < -0.1 * MW.Main.ActualWidth && MW.Core.Controller.GetWindowsDistanceLeft() < System.Windows.SystemParameters.PrimaryScreenWidth
        );
        private void MUItimer(object? sender, EventArgs e)
        {
            if (!IsOpen)
            {
                return;
            }
            if (IsBegin == true)
            {
                double g = -100;
                MW.Core.Controller.MoveWindows(SpeedX/400, SpeedY/300);
                if(SpeedYO > -1000 && SpeedYO <= 0 && Math.Abs(SpeedX) > 300)
                {
                    SpeedYO = 10;
                    SpeedY = 10;
                }
                else if (SpeedYO > -1000 && SpeedYO <= 0)
                {
                    SpeedYO = 1;
                    SpeedY = 1;
                }
                if ((SpeedY - g) / SpeedYO >= -1.5) 
                {
                    SpeedY -= g;
                }
                else
                {
                    SpeedY = 0;
                }
                if (SpeedY / SpeedYO <= -0.8 || (SpeedYO > 0 && SpeedY >= Math.Min(10000,Math.Max(2000,Math.Max(2.3 * SpeedYO, Math.Abs(2.3 * SpeedX))/5))) || (SpeedYO == 1) || CheckPosition())
                {
                    if (SpeedX > 0)
                    {
                        MW.Main.DisplayCEndtoNomal("fall.right");
                    }
                    else
                    {
                        MW.Main.DisplayCEndtoNomal("fall.left");
                    }
                    SpeedX = 0;SpeedY = 0;SpeedYO = 0;
                    LLeft = -1.0;LUp = -1.0;
                    IsBegin = false;
                }
            }
        }
        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsOpen)
            {
                return;
            }
            if (SpeedX > 0 && IsCheck == true)
            {
                MW.Main.Display(MW.Core.Graph.FindGraph("fall.right", AnimatType.B_Loop,MW.GameSavesData.GameSave.Mode));
            }
            else if (SpeedX <= 0 && IsCheck == true)
            {
                MW.Main.Display(MW.Core.Graph.FindGraph("fall.left", AnimatType.B_Loop, MW.GameSavesData.GameSave.Mode));              
            }
            if (IsCheck == true)
            {
                IsCheck = false;
                IsBegin = true;
            }  
            
        }
        public winSetting winSetting;
        public override void Setting()
        {
            if (winSetting == null)
            {
                winSetting = new winSetting(this);
                winSetting.Show();
            }
            else
            {
                winSetting.Topmost = true;
            }
        }

    }
}

