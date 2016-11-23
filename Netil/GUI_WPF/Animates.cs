using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Netil
{
    static class Animates
    {
        //Double类型Back缓动收缩动画
        static public DoubleAnimation BackShrink = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = -60,//收缩量
            BeginTime = TimeSpan.FromSeconds(0.05),//延迟量
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut }//缓动函数设置，详情参见MSDN关于缓动函数的文档：https://msdn.microsoft.com/zh-cn/library/ee308751(v=vs.110).aspx
        };

        //Double类型Back缓动扩展动画
        static public DoubleAnimation BackExpand = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = 60,//扩展量
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut }//缓动函数设置
        };

        //Double类型Cubic缓动收缩动画
        static public DoubleAnimation CubicShrink = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = -60,//收缩量
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };

        //Double类型Cubic缓动扩展动画
        static public DoubleAnimation CubicExpand = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//动画持续时间为0.3秒
            By = 60,//扩展量
            BeginTime = TimeSpan.FromSeconds(0.1),//延迟量
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };
        /*
        //Double类型Cubic扩展动画
        static public DoubleAnimation CubicFollow = new DoubleAnimation()
        {
            Duration = TimeSpan.FromSeconds(0.3),//跟踪灵敏度
            BeginTime = TimeSpan.FromSeconds(0.01),//信号消抖
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }//缓动函数设置
        };
        */
    }
}
