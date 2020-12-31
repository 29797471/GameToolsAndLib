using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TimeLineTool;

namespace DevelopTool
{
    /// <summary>
    /// StoryUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class StoryUserControl : UserControl
    {
        
        public StoryUserControl()
        {
            InitializeComponent();


            ObservableCollection<ITimeLineDataItem> listboxData = new ObservableCollection<ITimeLineDataItem>();
            string[] ary = new string[] { "角色", "肖像", "声音", "字幕", "屏幕", "摄像机","可摧毁物", "特效"};
            foreach(var it in ary)
            {
                listboxData.Add(new TempDataType() {
                    Name =it,
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today.AddHours(48),
                });
            }

            list.ItemsSource = listboxData;

            

            DataContextChanged += StoryUserControl_DataContextChanged;
        }

        private void StoryUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var data = (StoryData)DataContext;
            int i = 0;
            foreach (TimeLineControl timeline in items.Items)
            {
                timeline.StartDate = DateTime.Today;
                if (data.LineList.Count <= i) data.LineList.Add(new TimeLineList());
                timeline.Items = data.LineList[i].LineData;
                i++;
            }
        }

        private void Slider_Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (TimeLineControl timeline in items.Items)
            {
                timeline.UnitSize = Slider_Scale.Value;
            }
        }
    }
}

