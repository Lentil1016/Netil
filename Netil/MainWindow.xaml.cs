using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;


namespace Netil
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 提取Url的正则表达式
        /// </summary>
        private Regex WebUrlRegex = new Regex(@"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");

        /// <summary>
        /// 开始爬取
        /// </summary>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = false;
            string Urls = UrlsTextBox.Text;
            WebUrlRegex.Matches(Urls);
        }

        private void LockDomain_Changed(object sender, RoutedEventArgs e)
        {
            SubOnly.IsEnabled = (bool)LockDomain.IsChecked;
            SubOnly.IsChecked = false;
        }

        #region 页面调整动效
        private bool IsExpanded = false;//TextBox是否已经展开的标志

        //Double类型Back缓动收缩动画
        private DoubleAnimation BackShrink = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = -60,//收缩量
            BeginTime = TimeSpan.FromSeconds(0.05),//延迟量
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut }//缓动函数设置，详情参见MSDN关于缓动函数的文档：https://msdn.microsoft.com/zh-cn/library/ee308751(v=vs.110).aspx
        };

        //Double类型Back缓动扩展动画
        private DoubleAnimation BackExpand = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = 60,//扩展量
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut }//缓动函数设置
        };

        //Double类型Cubic缓动收缩动画
        private DoubleAnimation CubicShrink = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = -60,//收缩量
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };

        //Double类型Cubic缓动扩展动画
        private DoubleAnimation CubicExpand = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = 60,//扩展量
            BeginTime = TimeSpan.FromSeconds(0.1),//延迟量
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };

        //Double类型Cubic扩展动画
        private DoubleAnimation CubicFollow = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.5),//跟踪灵敏度
            BeginTime = TimeSpan.FromSeconds(0.1),//信号消抖
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };
        

        /// <summary>
        /// 在MainUrl文本框中改变文本触发
        /// </summary>
        private void MainUrl_Changed(object sender, TextChangedEventArgs e)
        {
            if (UrlsTextBox.LineCount >= 2)
            {
                if (!IsExpanded)
                {
                    IsExpanded = true;
                    this.UrlGroup.BeginAnimation(GroupBox.HeightProperty, BackExpand);
                    this.MainPanelGrid.BeginAnimation(Grid.HeightProperty, BackShrink);
                }
            }
            else
            {
                if (IsExpanded)
                {
                    IsExpanded = false;
                    this.UrlGroup.BeginAnimation(GroupBox.HeightProperty, CubicShrink);
                    this.MainPanelGrid.BeginAnimation(Grid.HeightProperty, CubicExpand);
                }
            }
        }

        private void Refresh(object sender, SizeChangedEventArgs e)
        {
            CubicFollow.To = this.ActualHeight - Convert.ToDouble(IsExpanded) * 60 - 75 - 31;
            MainPanelGrid.BeginAnimation(Grid.HeightProperty, CubicFollow);
        }

        private void Render(object sender, EventArgs e)
        {
            var ignore_me = RenderAsync();//To avoid the CS4014 Warning http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        }

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
    }
}