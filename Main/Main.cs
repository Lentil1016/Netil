using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Main.MonoWin;

namespace Main
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        
        static void Main(string[] args)
        {
            Console.WriteLine("dhifdsa");
            if (RuntimeFramework.CurrentFramework.Runtime == RuntimeType.Net)
            {
                //Assembly assm =EmbeddedAssembly.Load("Main.WPF.dll","WPF.dll");
                Assembly assm = Assembly.LoadFrom( "WPF.dll");
                Object App = assm.CreateInstance("WPF.App");
                Type t = assm.GetType("WPF.App");
                t.GetMethod("RunInstance").Invoke(App,null);
                //App app = new App();
                //app.RunInstance();
            }

            if (RuntimeFramework.CurrentFramework.Runtime == RuntimeType.Mono)
            {
                //EmbeddedAssembly.Load("Main.Netil.dll", "Netil.dll");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Mono());
            }
        }
    }

    public enum RuntimeType
    {
        Any,   // Any supported runtime framework
        Net,   // Microsoft .NET Framework
        NetCF, // Microsoft .NET Compact Framework
        SSCLI, // Microsoft Shared Source CLI
        Mono,  // Mono
    }

    // See http://nunit.org, this class from NUnit Project's RuntimeFramework.cs
    // RuntimeFramework represents a particular version of a common language runtime implementation.
    [Serializable]
    public sealed class RuntimeFramework
    {
        public RuntimeType Runtime { get; private set; }
        public Version Version { get; private set; }
        public string DisplayName { get; private set; }
        static RuntimeFramework currentFramework;

        public static RuntimeFramework CurrentFramework
        {
            get
            {
                if (currentFramework == null)
                {
                    var monoRuntimeType = Type.GetType("Mono.Runtime", false);
                    var runtime = monoRuntimeType != null ? RuntimeType.Mono : RuntimeType.Net;
                    currentFramework = new RuntimeFramework(runtime, Environment.Version);
                    if (monoRuntimeType != null)
                    {
                        var method = monoRuntimeType.GetMethod("GetDisplayName", BindingFlags.Static |
                          BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding);
                        if (method != null) currentFramework.DisplayName = (string)method.Invoke(null, new object[0]);
                    }
                }
                return currentFramework;
            }
        }
        
        public RuntimeFramework(RuntimeType runtime, Version version)
        {
            Runtime = runtime;
            Version = version;
            DisplayName = runtime.ToString() + " " + version.ToString();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
