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

namespace ui
{
    /// <summary>
    /// Interaction logic for Property.xaml
    /// </summary>
    public partial class Property : UserControl
    {
        public Property()
        {
            InitializeComponent();
        }

        public object Label { get => LabelObject.Content; set => LabelObject.Content = value; }

        private UIElement content;

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            UIElement pend = newContent as UIElement;
            Grid.Children.Remove(content);
            if (pend == null) return;
            content = pend;
            Grid.SetColumn(content, 1);
        }
    }
}
