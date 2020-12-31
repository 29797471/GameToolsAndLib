using CqCore;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCore
{
    /// <summary>
    /// ColorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ColorUserControl : UserControl
    {
        public static readonly DependencyProperty HtmlColorProperty =
            DependencyProperty.Register("HtmlColor", typeof(string), typeof(ColorUserControl),
            new FrameworkPropertyMetadata("#FFFFFFFF", new PropertyChangedCallback(HtmlColorPropertyChangedCallback)));
        public event EventHandler<EventArgs> HtmlColorChanged;

        private static void HtmlColorPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is ColorUserControl)
            {
                ColorUserControl con = sender as ColorUserControl;
                if (arg.NewValue != null)
                {
                    con.HtmlColor= (string)arg.NewValue;
                    //AssemblyUtil.SetFieldValue(con.DataContext, arg.Property.Name, arg.NewValue);

                    //con.HtmlColor = 
                    //arg.Property.Name
                    //con.OnColorUpdated((DateTime)arg.OldValue, (DateTime)arg.NewValue);
                }else
                {
                    con.HtmlColor = "#FFFFFFFF";
                    var t = con.GetBindingExpression(HtmlColorProperty);
                    AssemblyUtil.SetMemberValue(t.DataItem, t.ParentBinding.Path.Path, con.HtmlColor);
                }
            }
        }

        [Description("获取或设置颜色")]
        [Category("Common Properties")]
        public string HtmlColor
        {
            get
            {
                return br.Color.ToString();
            }
            set
            {
                //this.SetValue(HtmlColorProperty, value);//设置值到所有控件,???
                
                br.Color = (Color)ColorConverter.ConvertFromString(value);
                if( ((byte)slider.Value )!= br.Color.A) slider.Value = br.Color.A;
                //OnHtmlColorUpdated(value);

            }
        }
        
        public ColorUserControl()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.Color = System.Drawing.ColorTranslator.FromHtml(br.ToString());

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HtmlColor = CqCore.ColorUtil.ColorIntToHtml(colorDialog.Color.ToArgb());
                var t = GetBindingExpression(HtmlColorProperty);
                //t.UpdateSource();
                //this.SetCurrentValue(HtmlColorProperty, HtmlColor);
                AssemblyUtil.SetMemberValue(t.DataItem, t.ParentBinding.Path.Path, HtmlColor);
                
                if(HtmlColorChanged!=null) HtmlColorChanged(this, EventArgs.Empty);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.Value = (byte)(slider.Value);
            Color c = br.Color;
            c.A = (byte)(slider.Value);
            br.Color = c;
            var t = GetBindingExpression(HtmlColorProperty);
            AssemblyUtil.SetMemberValue(t.DataItem, t.ParentBinding.Path.Path, HtmlColor);
            
        }
    }
}
