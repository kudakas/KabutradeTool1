using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace KabuTradeTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 以下を追加
            this.btnDownload.Click += (sender, e) =>
            {
                var client = new System.Net.WebClient();
                byte[] buffer = client.DownloadData("http://k-db.com/?p=all&download=csv");
                string str = Encoding.Default.GetString(buffer);
                string[] rows = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                // １行目をラベルに表示
                this.lblTitle.Content = rows[0];
                // ２行目以下はカンマ区切りから文字列の配列に変換しておく
                List<string[]> list = new List<string[]>();
                rows.ToList().ForEach(row => list.Add(row.Split(new char[] { ',' })));
                // ヘッダの作成（データバインド用の設計）
                GridView view = new GridView();
                list.First().Select((item, cnt) => new { Count = cnt, Item = item }).ToList().ForEach(header =>
                {
                    view.Columns.Add(
                        new GridViewColumn()
                        {
                            Header = header.Item,
                            DisplayMemberBinding = new Binding(string.Format("[{0}]", header.Count))
                        });
                });
                // これらをリストビューに設定
                lvStockList.View = view;
                lvStockList.ItemsSource = list.Skip(1);
            };
        }
    }
}
