﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Netil;


namespace WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private core Core = new core();
        private List<RuleControl> CtrlList = new List<RuleControl>();
        public MainWindow()
        {
            InitializeComponent();

            //测试控件添加和删除
            //TODO：封装stackpanel控件的添加和删除
            CtrlList.Add(new RuleControl());
            RuleCtrlPanel.Children.Add(CtrlList[0]);
            RuleCtrlPanel.Children.Remove(CtrlList[0]);
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("123"+Core.ToString());
            Start.IsEnabled = false;
            //foreach (string Url in Urls)
        }

        private void LockDomain_Changed(object sender, RoutedEventArgs e)
        {
            SubOnly.IsEnabled = (bool)LockDomain.IsChecked;
            SubOnly.IsChecked = false;
        }

        #region 页面调整动效

        private bool IsExpanded = false;//TextBox是否已经展开的标志

        /// <summary>
        /// 在MainUrl文本框中改变文本触发，用于实现文本框的伸缩。
        /// </summary>
        private void MainUrl_Changed(object sender, TextChangedEventArgs e)
        {
            if (UrlsTextBox.LineCount >= 2)
            {
                if (!IsExpanded)
                {
                    IsExpanded = true;
                    this.UrlGroup.BeginAnimation(GroupBox.HeightProperty, Animates.BackExpand);
                    this.MainPanelGrid.BeginAnimation(Grid.HeightProperty, Animates.BackShrink);
                }
            }
            else
            {
                if (IsExpanded)
                {
                    IsExpanded = false;
                    this.UrlGroup.BeginAnimation(GroupBox.HeightProperty, Animates.CubicShrink);
                    this.MainPanelGrid.BeginAnimation(Grid.HeightProperty, Animates.CubicExpand);
                }
            }
        }

        /// <summary>
        /// MainWindow的SizeChanged事件函数，用于动态调整控件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, SizeChangedEventArgs e)
        {
            MainPanelGrid.Height = this.ActualHeight - Convert.ToDouble(IsExpanded) * 60 - 75 - 31;
            //Animates.CubicFollow.To = this.ActualHeight - Convert.ToDouble(IsExpanded) * 60 - 75 - 31;
            //MainPanelGrid.BeginAnimation(Grid.HeightProperty, Animates.CubicFollow);
        }

        /// <summary>
        /// MainWindow的Initialized事件函数，用于在窗体初始化完成后为动态背景方法创建线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgRender(object sender, EventArgs e)
        {
            var FireAndIgnore = RenderAsync();//To avoid the CS4014 Warning http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        }

        /// <summary>
        /// 实现了动态背景的方法，异步执行一次即可循环变换背景。
        /// </summary>
        /// <returns>Microsoft不建议在Async方法中使用Void返回值，由于此方法为Fire and ignore，因此Task返回值实际并没有作用</returns>
        private async Task RenderAsync()
        {
            //Double类型Cubic扩展动画
            DoubleAnimation leave = new DoubleAnimation()
            {
                From = 0.6,
                To = 0.65,
                Duration = TimeSpan.FromSeconds(5),//跟踪灵敏度
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
            };

            DoubleAnimation back = new DoubleAnimation()
            {
                From = 0.65,
                To = 0.6,
                Duration = TimeSpan.FromSeconds(5),//跟踪灵敏度
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
            };
            while (true)
            {
                this.Point1.BeginAnimation(GradientStop.OffsetProperty, leave);
                await Task.Delay(TimeSpan.FromSeconds(5.1));
                this.Point1.BeginAnimation(GradientStop.OffsetProperty, back);
                await Task.Delay(TimeSpan.FromSeconds(5.1));
            }
        }

        #endregion

        private void PipeLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddRuleBox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}