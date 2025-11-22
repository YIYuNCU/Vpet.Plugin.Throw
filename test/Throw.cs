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
using System.Diagnostics;
using Panuon.WPF.UI;
using System.Threading.Tasks;

namespace VPET.Evian.Throw
{
    public class Throw : MainPlugin
    {
        public bool IsOpen = true;
        private bool IsCheck;
        private bool IsBegin;
        private double LLeft = -0.1;
        private double LUp = -0.1;
        private double SpeedX;
        private double SpeedY;
        private double SpeedYO;
        private double SpeedXforLLM;
        private double SpeedYforLLM;
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
        public const double def_LimitSpeedX = 10.0;
        public double LimitSpeedY;
        public const double def_LimitSpeedY = 10.0;
        public double MulSpeedX;
        public const double def_MulSpeedX = 1.0;
        public double MulSpeedY;
        public const double def_MulSpeedY = 1.0;
        public double MulSpeed;
        public const double def_MulSpeed = 1.0;
        public bool LLMEnable;
        private Stopwatch llmTimeTick;
        private bool llmCooling = false;
        private bool llmStarted = false;
        private long presstime;
        public winSetting winSetting;
        System.Threading.Timer SpeedTimer;
        System.Threading.Timer UITimer;
        private bool Lock = false;
        public double Rate = 30;
        public const double def_Rate = 30;

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
                LimitSpeedX = def_LimitSpeedX;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("LimitSpeedY")))
            {
                LimitSpeedY = MW.GameSavesData["Throw"][(gdbe)"LimitSpeedY"];
            }
            else
            {
                LimitSpeedY = def_LimitSpeedY;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeedX")))
            {
                MulSpeedX = MW.GameSavesData["Throw"][(gdbe)"MulSpeedX"];
            }
            else
            {
                MulSpeedX = def_MulSpeedX;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeedY")))
            {
                MulSpeedY = MW.GameSavesData["Throw"][(gdbe)"MulSpeedY"];
            }
            else
            {
                MulSpeedY = def_LimitSpeedY;
            }
            if (!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("MulSpeed")))
            {
                MulSpeed = MW.GameSavesData["Throw"][(gdbe)"MulSpeed"];
            }
            else
            {
                MulSpeed = def_MulSpeed;
            }
            if(!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("LLMEnable")))
            {
                LLMEnable = MW.GameSavesData["Throw"][(gbol)"LLMEnable"];
            }
            else
            {
                if (MW.TalkBoxCurr == null)
                {
                    LLMEnable = false;
                }
                else
                    LLMEnable = (MW.TalkBoxCurr.APIName == "VPetLLM");
            }
            if(!string.IsNullOrEmpty(MW.GameSavesData["Throw"].GetString("Rate")))
            {
                Rate = MW.GameSavesData["Throw"][(gdbe)"Rate"];
            }
            else
            {
                Rate = def_Rate;
            }
            MenuItem menuMODConfig = MW.Main.ToolBar.MenuMODConfig;
            menuMODConfig.Visibility = Visibility.Visible;
            MenuItem menuItem = new MenuItem
            {
                Header = "Throw".Translate(),
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
            };
            menuItem.Click += delegate
            {
                Setting();
            };
            menuMODConfig.Items.Add(menuItem);
            SpeedTimer = new(_ => { MSpeedtimer(); }, state: null, dueTime: TimeSpan.Zero, period: TimeSpan.FromMilliseconds(1000 / 30));
            UITimer = new(_ => { MUItimer(); }, state: null, dueTime: TimeSpan.Zero, period: TimeSpan.FromMilliseconds(1000 / Rate));
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
        }
        public void TimerChange()
        {
            try
            {
                SpeedTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000 / 30));
                UITimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000 / Rate));
            }
            catch (Exception ex)
            {
                _ = MW.Dispatcher.BeginInvoke(() => MessageBoxX.Show("改变定时器计时周期错误，堆栈信息为：{0}".Translate(ex.Message)));
            }
        }
        public void MCheck()
        {
            if (IsOpen)
            {
                IsCheck = true;
                IsBegin = false;
                MW.Main.MoveTimer.Enabled = false;
                Lock = false;
                SpeedX = 0;
                SpeedY = 0;
                SpeedYO = 0;
                StopThrowForLLM();
            }
        }

        private void MSpeedtimer()
        {
            if (!IsOpen || !IsCheck)
            {
                return;
            }
            if (Lock) return;
            Lock = true;
            double windowsDistanceLeft = 0;
            double windowsDistanceUp = 0;
            Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            {
                windowsDistanceLeft = MW.Core.Controller.GetWindowsDistanceLeft();
                windowsDistanceUp = MW.Core.Controller.GetWindowsDistanceUp();
            }));
            var speedRate = 1.0 / (1000 / Rate);
            if (LLeft > 0.0)
            {
                if (Math.Abs(SpeedX) > speedRate)
                {
                    SpeedX = ((SpeedX + (windowsDistanceLeft - LLeft)) / 2.0);
                }
                else
                {
                    SpeedX = (windowsDistanceLeft - LLeft) * speedRate;
                }
                if (Math.Abs(SpeedY) > speedRate)
                {
                    SpeedY = ((SpeedY + (windowsDistanceUp - LUp)) / 2.0);
                }
                else
                {
                    SpeedY = (windowsDistanceUp - LUp) * speedRate;
                }
            }
            LLeft = windowsDistanceLeft;
            LUp = windowsDistanceUp;
            Lock = false;
        }

        public int MCheckPosition()
        {
            try
            {
                var windowScreen = GetScreenFromGrid(MW.Main.MainGrid);
                var screenWidth = 0.0;
                var screenHeight = 0.0;
                if (windowScreen == null)
                {
                    screenWidth = windowScreen.Bounds.Width;
                    screenHeight = windowScreen.Bounds.Height;
                }
                else
                {
                    screenWidth = SystemParameters.PrimaryScreenWidth;
                    screenHeight = SystemParameters.PrimaryScreenHeight;
                }
                if ((MW.Core.Controller.GetWindowsDistanceUp() < 0.0 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceDown() < screenHeight) || MW.Core.Controller.GetWindowsDistanceDown() > screenHeight)
                {
                    return 1;
                }
                if (MW.Core.Controller.GetWindowsDistanceDown() < -0.05 * MW.Main.ActualHeight && MW.Core.Controller.GetWindowsDistanceUp() < screenHeight)
                {
                    return 2;
                }
                if (MW.Core.Controller.GetWindowsDistanceLeft() < 0)
                {
                    return 3;
                }
                if (MW.Core.Controller.GetWindowsDistanceRight() < 0)
                {
                    return 4;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _= MW.Dispatcher.BeginInvoke(() => MessageBoxX.Show("检测桌宠位置时出现错误，堆栈信息为：{0}".Translate(ex.Message)));
                return 0;
            }
        }

        public int CheckPosition()
        {
            return MW.Dispatcher.Invoke((Func<int>)MCheckPosition);
        }

        private async void MUItimer()
        {
            if (!IsOpen || !IsBegin)
            {
                return;
            }
            if (Lock) return;
            Lock = true;
            var speedRate = 30 / Rate;
            double num = -1.0 * MulSpeed * speedRate;
            await MW.Dispatcher.BeginInvoke(()=>
            {
                MW.Core.Controller.MoveWindows(SpeedX * speedRate, SpeedY * speedRate);
            });
            if (SpeedYO > -speedRate && SpeedYO <= 0.0 && Math.Abs(SpeedX) > speedRate)
            {
                SpeedYO = -speedRate;
                SpeedY = -speedRate;
            }
            else if (SpeedYO > -speedRate && SpeedYO <= 0.0)
            {
                SpeedYO = -speedRate;
                SpeedY = -speedRate;
            }
            if (((SpeedY - num) / SpeedYO >= -1.5) && ((SpeedY - num) / SpeedYO < 5))
            {
                SpeedY -= num;
            }
            else
            {
                SpeedY = 0.0;
                SpeedYO = 0.001;
            }
            if ((!(SpeedYO > 0.0) || !(SpeedY >= MulSpeed * MulSpeedY * Math.Min(100.0, Math.Max(20.0, Math.Max(SpeedYO, Math.Abs(SpeedX)) / 5.0)))) && SpeedYO != 0.001 && CheckPosition() == 0)
            {
                Lock = false;
                return;
            }
            SpeedY = 0.0;
            SpeedYO = 0.0;
            LLeft = -0.1;
            LUp = -0.1;
            IsBegin = false;
            switch (CheckPosition())
            {
                case 1:
                    if (SpeedX < 0.0)
                    {
                        if (LeftUpWallType == "BLoop")
                        {
                            if (MW.Dispatcher.Invoke(()=> MW.Core.Graph.FindGraph(LeftUpWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal)) != null)
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
                                MW.Dispatcher.Invoke(() =>
                                {
                                    MW.Main.DisplayCEndtoNomal("fall.left");
                                });
                            }
                        }
                        else if (MW.Dispatcher.Invoke(()=> MW.Core.Graph.FindGraph(LeftUpWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
                        {
                            MW.Dispatcher.Invoke(() =>
                            {
                                MW.Main.DisplayCEndtoNomal(LeftUpWall);
                            });
                        }
                        else
                        {
                            MW.Dispatcher.Invoke(() =>
                            {
                                MW.Main.DisplayCEndtoNomal("fall.left");
                            });
                        }
                    }
                    else if (RightUpWallType == "BLoop")
                    {
                        if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(RightUpWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal)) != null)
                        {
                            MW.Dispatcher.Invoke(()=>
                            {
                                MW.Core.Controller.MoveWindows(0.0, -200.0);
                                MW.Core.Controller.ResetPosition();
                                MW.Main.DisplayBLoopingForce(RightUpWall);
                            });
                        }
                        else
                        {
                            MW.Dispatcher.Invoke(() =>
                            {
                                MW.Main.DisplayCEndtoNomal("fall.right");
                            });
                        }
                    }
                    else if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(RightUpWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal(RightUpWall);
                        });
                    }
                    else
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal("fall.right");
                        });
                    }
                    break;
                case 4:
                    if (RightWallType == "BLoop")
                    {
                        if (MW.Dispatcher.Invoke(()=> MW.Core.Graph.FindGraph(RightWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal)) != null)
                        {
                            MW.Dispatcher.Invoke(()=>
                            {
                                MW.Core.Controller.ResetPosition();
                                MW.Main.DisplayBLoopingForce(RightWall);
								(new Action(async () => {
									int num = 0;
									while (MW.Core.Controller.GetWindowsDistanceRight() != 0 && num <= 100) {
                                        MW.Core.Controller.MoveWindows(MW.Core.Controller.GetWindowsDistanceRight(), 0.0);
                                        num++;
                                    }
								}))();
							});
                        }
                        else
                        {
                            MW.Dispatcher.Invoke(() =>
                            {
                                MW.Main.DisplayCEndtoNomal("fall.right");
                            });
                        }
                    }
                    else if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(RightWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal(RightWall);
                        });
                    }
                    else
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal("fall.right");
                        });
                    }
                    break;
                case 3:
                    if (LeftWallType == "BLoop")
                    {
                        if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(LeftWall, GraphInfo.AnimatType.B_Loop, IGameSave.ModeType.Nomal)) != null)
                        {
                            MW.Dispatcher.Invoke(()=>
                            {
                                MW.Core.Controller.ResetPosition();
								MW.Main.DisplayBLoopingForce(LeftWall);
								(new Action(async () => {
                                    int num = 0;
									while (MW.Core.Controller.GetWindowsDistanceLeft() != 0 && num<=100) {
										MW.Core.Controller.MoveWindows(-MW.Core.Controller.GetWindowsDistanceLeft(), 0.0);
                                        num++;
									}
								}))();
							});
                        }
                        else
                        {
                            MW.Dispatcher.Invoke(() =>
                            {
                                MW.Main.DisplayCEndtoNomal("fall.left");
                            });
                        }
                    }
                    else if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(LeftWall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal(LeftWall);
                        });
                    }
                    else
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal("fall.left");
                        });
                    }
                    break;
                default:
                    if (SpeedX > 0.0)
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal("fall.right");
                        });
                    }
                    else
                    {
                        MW.Dispatcher.Invoke(() =>
                        {
                            MW.Main.DisplayCEndtoNomal("fall.left");
                        });
                    }
                    break;
            }
            SpeedX = 0.0;
            await MW.Dispatcher.BeginInvoke(async () =>
            {
                if (!await EnableLLM()) return;
                    EndThrowForLLM();
            });
            Lock = false;
        }

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsOpen)
                return;
            SpeedX *= MulSpeedX;
            SpeedY *= MulSpeedY;

            if (Math.Abs(SpeedX) < LimitSpeedX)
            {
                SpeedX = 0.0;
            }
            if (Math.Abs(SpeedY) < LimitSpeedY)
            {
                SpeedY = 0.0;
                SpeedYO = 0.0;
            }

            SpeedX *= MulSpeed;
            SpeedY *= MulSpeed;

            SpeedY = Math.Min(Math.Max(SpeedY, -30), 30);
            SpeedYO = SpeedY; // Synchronize SpeedYO with SpeedY

            // Apply bounds for SpeedX as well
            SpeedX = Math.Min(Math.Max(SpeedX, -50), 50);

            // If SpeedYO is zero, stop movement entirely
            if (SpeedX == 0)
            {
                SpeedX = 0.0;
                SpeedY = 0.0;
                SpeedYO = 0.0;
                LLeft = -0.1;
                LUp = -0.1;
                IsBegin = false;
                IsCheck = false;
                return;
            }
            _=StartThrowForLLM();
            HandleMovement();
            MW.Dispatcher.Invoke(() =>
            {
                if (SpeedX > 0) MW.Main.DisplayBLoopingForce("fall.right");
                else if (SpeedX < 0) MW.Main.DisplayBLoopingForce("fall.left");
            });
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
            if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(wall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
            {
                MW.Dispatcher.BeginInvoke(delegate
                {
                    MW.Main.Display(wall, GraphInfo.AnimatType.B_Loop, () =>
                    {
                        MW.Main.DisplayBLoopingForce(wall);
                    });
                });
            }
            else
            {
                MW.Dispatcher.BeginInvoke(delegate
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
            if (MW.Dispatcher.Invoke(()=>MW.Core.Graph.FindGraph(wall, GraphInfo.AnimatType.C_End, IGameSave.ModeType.Nomal)) != null)
            {
                MW.Dispatcher.BeginInvoke(delegate
                {
                    MW.Main.Display(wall, GraphInfo.AnimatType.B_Loop, () =>
                    {
                        MW.Main.DisplayBLoopingForce(wall);
                    });
                });
            }
            else
            {
                MW.Dispatcher.BeginInvoke(delegate
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
        }

        public async void EndThrowForLLM()
        {
            try
            {
                var chatmessage = "";
                llmTimeTick.Stop();
                VPetLLM.Utils.Logger.Log("The pet flew in the air for {0} ms".Translate(llmTimeTick.ElapsedMilliseconds));
                chatmessage = "用户将你扔到了空中，你飞行了{0}ms，你的最大水平速度是{1}px/s，你的最大垂直速度是{2}px/s".Translate(llmTimeTick.ElapsedMilliseconds, SpeedXforLLM, SpeedYforLLM);
                if (llmTimeTick.ElapsedMilliseconds > 200)
                {
                    VPetLLM.Handlers.PluginHandler.SendPluginMessage("Throw", chatmessage);
                }
                SpeedXforLLM = 0;
                SpeedYforLLM = 0;
                llmCooling = true;
                llmStarted = false;
                await Task.Delay(30000);
                llmCooling = false;
            }
            catch (Exception ex)
            {
                SpeedXforLLM = 0;
                SpeedYforLLM = 0;
                llmStarted = false;
                _ = MW.Dispatcher.BeginInvoke(() => MessageBoxX.Show("LLM service processing failed,the error stack is {0}".Translate(ex.Message)));
            }
        }

        public async Task StopThrowForLLM()
        {
            if (!await EnableLLM()) return;
            if (!llmStarted) return;
            llmTimeTick.Stop();
            SpeedXforLLM = 0;
            SpeedYforLLM = 0;
            llmStarted = false;
        }

        public async Task StartThrowForLLM()
        {
            if(!await EnableLLM()) return;
            SpeedXforLLM = SpeedX  / 30;
            SpeedYforLLM = SpeedYO / 30;
            llmTimeTick = Stopwatch.StartNew();
            llmStarted = true;
        }

        private async Task<bool> EnableLLM()
        {
            if (!LLMEnable) return false;
            if (llmCooling) return false;
            var isllmDisable = await MW.Dispatcher.InvokeAsync(() =>
            {
                return MW.TalkBoxCurr.APIName != "VPetLLM";
            });
            if (isllmDisable)
            {
                return false;
            }
            return true;
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

        public static System.Windows.Forms.Screen GetScreenFromGrid(Grid grid)
        {
            // 获取Grid所在的Window
            Window window = Window.GetWindow(grid);
            if (window == null) return null;

            // 获取窗口的句柄
            var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(window);
            IntPtr windowHandle = windowInteropHelper.Handle;

            // 根据窗口句柄获取所在的屏幕
            return System.Windows.Forms.Screen.FromHandle(windowHandle);
        }
    }
}
