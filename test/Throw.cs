// Throw, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// VPET.Evian.Throw.Throw
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using LinePutScript;
using LinePutScript.Localization.WPF;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;
using VPET.Evian.Throw;
namespace VPET.Evian.Throw
{
    public class Throw : MainPlugin
    {
        public bool IsOpen = true;
        private bool IsCheck;
        private bool IsBegin;
        private double LLeft = -1.0;
        private double LUp = -1.0;
        private double SpeedX;
        private double SpeedY;
        private double SpeedYO;
        public LpsDocument MConfig;
        private string LeftUp;
        private string LeftDown;
        private string RightUp;
        private string RightDown;
        private string LeftWall;
        private string LeftWallType;
        private string RightWall;
        private string RightWallType;
        private string LeftUpWall;
        private string LeftUpWallType;
        private string RightUpWall;
        private string RightUpWallType;
        public double LimitSpeedX;
        public double LimitSpeedY;
        public double MulSpeedX;
        public double MulSpeedY;
        public double MulSpeed;
        private long presstime;
        public winSetting winSetting;

        public override string PluginName => "Throw";

        public Throw(IMainWindow mainwin)
            : base(mainwin)
        {
        }

        public override void LoadPlugin()
        {
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("IsOpen")))
            {
                IsOpen = MW.GameSavesData["Throw"][(gbol)"IsOpen"];
            }
            else
            {
                IsOpen = true;
            }
            string sourceFileName = LoaddllPath("Throw") + "\\Resources\\config.lps";
            string text = LoaddllPath("Throw") + "\\Resources\\config_user.lps";
            if (!File.Exists(text))
            {
                File.Copy(sourceFileName, text);
            }
            MConfig = new LpsDocument(File.ReadAllText(text));
            if (!string.IsNullOrEmpty(MConfig["LeftUp"].GetString("LeftUp")))
            {
                LeftUp = MConfig["LeftUp"][(gstr)"LeftUp"];
            }
            else
            {
                MConfig["LeftUp"][(gstr)"LeftUp"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["LeftDown"].GetString("LeftDown")))
            {
                LeftDown = MConfig["LeftDown"][(gstr)"LeftDown"];
            }
            else
            {
                MConfig["LeftDown"][(gstr)"LeftDown"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightUp"].GetString("RightUp")))
            {
                RightUp = MConfig["RightUp"][(gstr)"RightUp"];
            }
            else
            {
                MConfig["RightUp"][(gstr)"RightUp"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightDown"].GetString("RightDown")))
            {
                RightDown = MConfig["RightDown"][(gstr)"RightDown"];
            }
            else
            {
                MConfig["RightDown"][(gstr)"RightDown"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["LeftWall"].GetString("LeftWall")))
            {
                LeftWall = MConfig["LeftWall"][(gstr)"LeftWall"];
            }
            else
            {
                MConfig["LeftWall"][(gstr)"LeftWall"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["LeftUpWall"].GetString("LeftUpWall")))
            {
                LeftUpWall = MConfig["LeftUpWall"][(gstr)"LeftUpWall"];
            }
            else
            {
                MConfig["LeftUpWall"][(gstr)"LeftUpWall"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["LeftUpWall"].GetString("Type")))
            {
                LeftUpWallType = MConfig["LeftUpWall"][(gstr)"Type"];
            }
            else
            {
                MConfig["LeftUpWall"][(gstr)"Type"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightWall"].GetString("RightWall")))
            {
                RightWall = MConfig["RightWall"][(gstr)"RightWall"];
            }
            else
            {
                MConfig["RightWall"][(gstr)"RightWall"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightUpWall"].GetString("RightUpWall")))
            {
                RightUpWall = MConfig["RightUpWall"][(gstr)"RightUpWall"];
            }
            else
            {
                MConfig["RightUpWall"][(gstr)"RightUpWall"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightUpWall"].GetString("Type")))
            {
                RightUpWallType = MConfig["RightUpWall"][(gstr)"Type"];
            }
            else
            {
                MConfig["RightUpWall"][(gstr)"Type"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["LeftWall"].GetString("Type")))
            {
                LeftWallType = MConfig["LeftWall"][(gstr)"Type"];
            }
            else
            {
                MConfig["LeftWall"][(gstr)"Type"] = "0";
            }
            if (!string.IsNullOrEmpty(MConfig["RightWall"].GetString("Type")))
            {
                RightWallType = MConfig["RightWall"][(gstr)"Type"];
            }
            else
            {
                MConfig["RightWall"][(gstr)"Type"] = "0";
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("LimitSpeedX")))
            {
                LimitSpeedX = MW.GameSavesData["Throw"][(gdbe)"LimitSpeedX"];
            }
            else
            {
                LimitSpeedX = 100.0;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("LimitSpeedY")))
            {
                LimitSpeedY = MW.GameSavesData["Throw"][(gdbe)"LimitSpeedY"];
            }
            else
            {
                LimitSpeedY = 100.0;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeedX")))
            {
                MulSpeedX = MW.GameSavesData["Throw"][(gdbe)"MulSpeedX"];
            }
            else
            {
                MulSpeedX = 1.0;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeedY")))
            {
                MulSpeedY = MW.GameSavesData["Throw"][(gdbe)"MulSpeedY"];
            }
            else
            {
                MulSpeedY = 1.0;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeed")))
            {
                MulSpeed = MW.GameSavesData["Throw"][(gdbe)"MulSpeed"];
            }
            else
            {
                MulSpeed = 1.0;
            }
            MenuItem menuMODConfig = MW.Main.ToolBar.MenuMODConfig;
            menuMODConfig.Visibility = Visibility.Visible;
            MenuItem menuItem = new MenuItem
            {
                Header = "Throw".Translate(),
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            menuItem.Click += delegate
            {
                Setting();
            };
            menuMODConfig.Items.Add(menuItem);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Tick += MSpeedtimer;
            DispatcherTimer dispatcherTimer2 = new DispatcherTimer();
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer2.Tick += MUItimer;
            MW.Main.MainGrid.MouseLeftButtonUp += MainGrid_MouseLeftButtonUp;
            for (int i = 0; i < 4; i++)
            {
                IGameSave.ModeType m = (IGameSave.ModeType)i;
                MW.Core.TouchEvent.Insert(0, new TouchArea(MW.Core.Graph.GraphConfig.TouchRaisedLocate[i], MW.Core.Graph.GraphConfig.TouchRaisedSize[i], delegate
                {
                    if (MW.Core.Save.Mode == m)
                    {
                        MCheck();
                    }
                    return false;
                }, isPress: true));
            }
            dispatcherTimer2.Start();
            dispatcherTimer.Start();
        }

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
            if (!IsOpen || !IsCheck)
            {
                return;
            }
            double windowsDistanceLeft = MW.Core.Controller.GetWindowsDistanceLeft();
            double windowsDistanceUp = MW.Core.Controller.GetWindowsDistanceUp();
            double num = windowsDistanceLeft;
            double num2 = windowsDistanceUp;
            if (LLeft > 0.0)
            {
                if (Math.Abs(SpeedX) > 10.0)
                {
                    SpeedX = (SpeedX + (num - LLeft) * 100.0) / 2.0;
                }
                else
                {
                    SpeedX = num - LLeft;
                    SpeedX *= 100.0;
                }
                if (Math.Abs(SpeedY) > 10.0)
                {
                    SpeedY = (SpeedY + (num2 - LUp) * 100.0) / 2.0;
                }
                else
                {
                    SpeedY = num2 - LUp;
                    SpeedY *= 100.0;
                }
            }
            LLeft = num;
            LUp = num2;
        }

        public int MCheckPosition()
        {
            if ((MW.Core.Controller.GetWindowsDistanceUp() < 0.0 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceDown() < SystemParameters.PrimaryScreenHeight) || MW.Core.Controller.GetWindowsDistanceDown() > SystemParameters.PrimaryScreenHeight)
            {
                return 1;
            }
            if (MW.Core.Controller.GetWindowsDistanceDown() < -0.05 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceUp() < SystemParameters.PrimaryScreenHeight)
            {
                return 2;
            }
            if ((MW.Core.Controller.GetWindowsDistanceLeft() < 0.0 * MW.Main.ActualWidth && MW.Core.Controller.GetWindowsDistanceRight() < SystemParameters.PrimaryScreenWidth) || MW.Core.Controller.GetWindowsDistanceRight() > SystemParameters.PrimaryScreenWidth)
            {
                return 3;
            }
            if ((MW.Core.Controller.GetWindowsDistanceRight() < 0.0 * MW.Main.ActualWidth && MW.Core.Controller.GetWindowsDistanceLeft() < SystemParameters.PrimaryScreenWidth) || MW.Core.Controller.GetWindowsDistanceLeft() > SystemParameters.PrimaryScreenWidth)
            {
                return 4;
            }
            return 0;
        }

        public int CheckPosition()
        {
            return MW.Dispatcher.Invoke((Func<int>)MCheckPosition);
        }

        private void MUItimer(object? sender, EventArgs e)
        {
            if (!IsOpen || !IsBegin)
            {
                return;
            }
            double num = -100.0 * MulSpeed;
            MW.Dispatcher.Invoke(delegate
            {
                MW.Core.Controller.MoveWindows(SpeedX / 400.0, SpeedY / 300.0);
            });
            if (SpeedYO > -1000.0 && SpeedYO <= 0.0 && Math.Abs(SpeedX) > 300.0)
            {
                SpeedYO = 10.0;
                SpeedY = 10.0;
            }
            else if (SpeedYO > -1000.0 && SpeedYO <= 0.0)
            {
                SpeedYO = 1.0;
                SpeedY = 1.0;
            }
            if ((SpeedY - num) / SpeedYO >= -1.5)
            {
                SpeedY -= num;
            }
            else
            {
                SpeedY = 0.0;
            }
            if (!(SpeedY / SpeedYO <= -0.8) && (!(SpeedYO > 0.0) || !(SpeedY >= MulSpeed * MulSpeedY * Math.Min(10000.0, Math.Max(2000.0, Math.Max(2.3 * SpeedYO, Math.Abs(2.3 * SpeedX)) / 5.0)))) && SpeedYO != 1.0 && CheckPosition() == 0)
            {
                return;
            }
            int num2 = CheckPosition();
            SpeedY = 0.0;
            SpeedYO = 0.0;
            LLeft = -1.0;
            LUp = -1.0;
            IsBegin = false;
            switch (num2)
            {
                case 1:
                    if (SpeedX < 0.0)
                    {
                        if (LeftUpWallType == "BLoop")
                        {
                            if (MW.Core.Graph.FindGraph(LeftUpWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal) != null)
                            {
                                MW.Dispatcher.Invoke(delegate
                                {
                                    MW.Core.Controller.MoveWindows(0.0, -200.0);
                                    MW.Core.Controller.ResetPosition();
                                    MW.Main.DisplayBLoopingForce(LeftUpWall);
                                });
                            }
                            else
                            {
                                MW.Main.DisplayCEndtoNomal("fall.left");
                            }
                        }
                        else if (MW.Core.Graph.FindGraph(LeftUpWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
                        {
                            MW.Main.DisplayCEndtoNomal(LeftUpWall);
                        }
                        else
                        {
                            MW.Main.DisplayCEndtoNomal("fall.left");
                        }
                    }
                    else if (RightUpWallType == "BLoop")
                    {
                        if (MW.Core.Graph.FindGraph(RightUpWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal) != null)
                        {
                            MW.Dispatcher.Invoke(delegate
                            {
                                MW.Core.Controller.MoveWindows(0.0, -200.0);
                                MW.Core.Controller.ResetPosition();
                                MW.Main.DisplayBLoopingForce(RightUpWall);
                            });
                        }
                        else
                        {
                            MW.Main.DisplayCEndtoNomal("fall.right");
                        }
                    }
                    else if (MW.Core.Graph.FindGraph(RightUpWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
                    {
                        MW.Main.DisplayCEndtoNomal(RightUpWall);
                    }
                    else
                    {
                        MW.Main.DisplayCEndtoNomal("fall.right");
                    }
                    break;
                case 4:
                    if (RightWallType == "BLoop")
                    {
                        if (MW.Core.Graph.FindGraph(RightWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal) != null)
                        {
                            MW.Dispatcher.Invoke(delegate
                            {
                                MW.Core.Controller.MoveWindows(200.0, 0.0);
                                MW.Core.Controller.ResetPosition();
                                MW.Main.DisplayBLoopingForce(RightWall);
                            });
                        }
                        else
                        {
                            MW.Main.DisplayCEndtoNomal("fall.right");
                        }
                    }
                    else if (MW.Core.Graph.FindGraph(RightWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
                    {
                        MW.Main.DisplayCEndtoNomal(RightWall);
                    }
                    else
                    {
                        MW.Main.DisplayCEndtoNomal("fall.right");
                    }
                    break;
                case 3:
                    if (LeftWallType == "BLoop")
                    {
                        if (MW.Core.Graph.FindGraph(LeftWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal) != null)
                        {
                            MW.Dispatcher.Invoke(delegate
                            {
                                if (MW.Core.Controller.GetWindowsDistanceLeft() > -100.0)
                                {
                                    MW.Core.Controller.MoveWindows(-200.0, 0.0);
                                }
                                MW.Core.Controller.ResetPosition();
                                MW.Main.DisplayBLoopingForce(LeftWall);
                            });
                        }
                        else
                        {
                            MW.Main.DisplayCEndtoNomal("fall.left");
                        }
                    }
                    else if (MW.Core.Graph.FindGraph(LeftWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
                    {
                        MW.Main.DisplayCEndtoNomal(LeftWall);
                    }
                    else
                    {
                        MW.Main.DisplayCEndtoNomal("fall.left");
                    }
                    break;
                default:
                    if (SpeedX > 0.0)
                    {
                        MW.Main.DisplayCEndtoNomal("fall.right");
                    }
                    else
                    {
                        MW.Main.DisplayCEndtoNomal("fall.left");
                    }
                    break;
            }
            SpeedX = 0.0;
        }

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsOpen)
                return;

            // Apply a smooth speed multiplier when the mouse is released
            SpeedX *= MulSpeedX;
            SpeedY *= MulSpeedY;

            // Ensure the speed doesn't fall below a threshold
            if (Math.Abs(SpeedX) < LimitSpeedX)
                SpeedX = 0.0;
            if (Math.Abs(SpeedY) < LimitSpeedY)
            {
                SpeedY = 0.0;
                SpeedYO = 0.0;
            }

            // Apply another multiplier to SpeedX and SpeedY for final adjustments
            SpeedX *= MulSpeed;
            SpeedY *= MulSpeed;

            // Ensure SpeedY stays within a reasonable range
            SpeedY = Math.Min(Math.Max(SpeedY, -20000.0), 20000.0);
            SpeedYO = SpeedY; // Synchronize SpeedYO with SpeedY

            // Apply bounds for SpeedX as well
            SpeedX = Math.Min(Math.Max(SpeedX, -25000.0), 25000.0);

            // If SpeedYO is zero, stop movement entirely
            if (SpeedYO == 0.0)
            {
                SpeedX = 0.0;
                SpeedY = 0.0;
                SpeedYO = 0.0;
                LLeft = -1.0;
                LUp = -1.0;
                IsBegin = false;
                return;
            }

            // Handle the movement logic based on SpeedX and SpeedY values
            HandleMovement();

            // Check if the object is in motion and update the flags accordingly
            if (IsCheck)
            {
                IsCheck = false;
                IsBegin = true;
            }
        }

        private void HandleMovement()
        {
            if (SpeedX > 0.0 && IsCheck)
            {
                HandleFlyingWallCollision(RightUp, RightDown, "right");
            }
            else if (SpeedX <= 0.0 && IsCheck)
            {
                HandleFlyingWallCollision(LeftUp, LeftDown, "left");
            }
        }

        private void HandleFlyingWallCollision(string upWall, string downWall, string direction)
        {
            if (SpeedY < 0.0)
            {
                HandleUpperWallCollision(upWall, direction);
            }
            else if (SpeedY > 0.0)
            {
                HandleLowerWallCollision(downWall, direction);
            }
        }

        private void HandleUpperWallCollision(string wall, string direction)
        {
            if (MW.Core.Graph.FindGraph(wall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
            {
                MW.Dispatcher.Invoke(delegate
                {
                    MW.Main.Display(wall, GraphInfo.AnimatType.B_Loop, () =>
                    {
                        MW.Main.DisplayBLoopingForce(wall);
                    });
                });
            }
            else
            {
                MW.Dispatcher.Invoke(delegate
                {
                    MW.Main.Display(MW.Core.Graph.FindGraph($"fall.{direction}", GraphInfo.AnimatType.B_Loop, MW.GameSavesData.GameSave.Mode), () =>
                    {
                        MW.Main.DisplayBLoopingForce($"fall.{direction}");
                    });
                });
            }
        }

        private void HandleLowerWallCollision(string wall, string direction)
        {
            if (MW.Core.Graph.FindGraph(wall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal) != null)
            {
                MW.Dispatcher.Invoke(delegate
                {
                    MW.Main.Display(wall, GraphInfo.AnimatType.B_Loop, () =>
                    {
                        MW.Main.DisplayBLoopingForce(wall);
                    });
                });
            }
            else
            {
                MW.Dispatcher.Invoke(delegate
                {
                    MW.Main.Display(MW.Core.Graph.FindGraph($"fall.{direction}", GraphInfo.AnimatType.B_Loop, MW.GameSavesData.GameSave.Mode), () =>
                    {
                        MW.Main.DisplayBLoopingForce($"fall.{direction}");
                    });
                });
            }
        }

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

        public override void EndGame()
        {
            base.Save();
            base.EndGame();
        }

        public string LoaddllPath(string dll)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.GetName().Name == dll)
                {
                    return Directory.GetParent(Path.GetDirectoryName(assembly.Location)).FullName;
                }
            }
            return "";
        }
    }
}
