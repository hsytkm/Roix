using RoixApp.Wpf.Models;
using System;
using System.Windows;

namespace RoixApp.Wpf
{
    public partial class App : Application
    {
        // AppクラスにModelインスタンスを持たせる
        // https://github.com/runceel/Livet-samples/blob/master/CommunicationBetweenViewModels/App.xaml.cs
        public static new App Current => (App)Application.Current;

        internal SourceImages SourceImages { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // 初期値として設定したら Resource を読めず Exception になったので OnStartup で ctor する
            SourceImages = new SourceImages();
        }

    }
}
