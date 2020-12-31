using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CqCore;

namespace WinCore
{
    /// <summary>
    /// CustomListText.xaml 的交互逻辑
    /// </summary>
    public partial class CustomListText : UserControl
    {
        public class ListTextItemStruct:NotifyObject
        {
            public Visibility Visibility { get { return mVisibility; } set { mVisibility = value; Update("Visibility"); } }
            Visibility mVisibility = Visibility.Collapsed;
            public string Content { get { return mContent; }set { mContent = value; Update("Content"); } }
            public string mContent;
        }

        public static readonly DependencyProperty RealItemValuesProperty =
           DependencyProperty.Register("RealItemValues", typeof(ObservableCollection<ListTextItemStruct>), typeof(CustomListText));

        
        ObservableCollection<ListTextItemStruct> mRealItemValues = new ObservableCollection<ListTextItemStruct>();
        public ObservableCollection<string> ItemSource
        {
            get
            {
                return (ObservableCollection<string>)GetValue(ItemSourceProperty);
            }
            set
            {
                this.SetValueX(ItemSourceProperty, value);
            }
        }
        public void UpdateViewData_ItemSource()
        {
            mRealItemValues.Clear();
            if (ItemSource == null) return;
            for (int i = 0; i < ItemSource.Count; i++)
            {
                mRealItemValues.Add(new ListTextItemStruct() { Content = ItemSource[i] });
            }
            listBox.ItemsSource = mRealItemValues;
            mRealItemValues.CollectionChanged += MRealItemValues_CollectionChanged;
        }

        public static readonly DependencyProperty ItemSourceProperty =
           DependencyProperty.Register("ItemSource", typeof(ObservableCollection<string>), typeof(CustomListText),
               new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemSourcePropertyChangedCallback)));

        private static void ItemSourcePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            var com = sender as CustomListText;
            com.UpdateViewData_ItemSource();
        }
        public CustomListText()
        {
            InitializeComponent();

            KeyEventHandler fun = (sender, e) =>
            {

                if (MathUtil.StateCheck(Keyboard.Modifiers, ModifierKeys.Control))
                {

                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.Add:
                            button_add_Click(null, null);
                            break;
                        case Key.Delete:
                            button_del_Click(null, null);
                            break;
                    }
                }
            };
            AddHandler(Keyboard.KeyDownEvent, fun);
        }

        public string SelectedValue
        {
            get
            {
                return (listBox.SelectedValue as ListTextItemStruct).Content;
            }
        }
        private void MRealItemValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ItemSource == null) return;
            ItemSource.Clear();
            foreach(var item in mRealItemValues)
            {
                ItemSource.Add(item.Content);
            }
        }

        private void CustomListText_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (ListTextItemStruct item in mRealItemValues)
            {
                item.Visibility = Visibility.Collapsed;
            }
        }


        /// <summary>  
        /// 双击项时显示编辑框  
        /// </summary>  
        private void listBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (listBox.SelectedItem as ListTextItemStruct);
            item.Visibility = Visibility.Visible;
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedIndex==-1)
            {
                mRealItemValues.Add(new ListTextItemStruct());
            }
            else
            {
                mRealItemValues.Insert(listBox.SelectedIndex, new ListTextItemStruct() { Content = mRealItemValues[listBox.SelectedIndex].Content });
            }
        }

        private void button_del_Click(object sender, RoutedEventArgs e)
        {
            mRealItemValues.RemoveAt(listBox.SelectedIndex);
        }


        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(ListTextItemStruct item in e.RemovedItems)
            {
                item.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemSource[listBox.SelectedIndex] = (sender as TextBox).Text;
            //var item = (sender as FrameworkElement).DataContext as ListTextItemStruct;
            //ItemSource[listBox.SelectedIndex] = mRealItemValues[listBox.SelectedIndex].Content;
        }
    }
}
