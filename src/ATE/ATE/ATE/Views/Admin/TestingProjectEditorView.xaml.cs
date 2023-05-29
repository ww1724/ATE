﻿using System;
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
using Zoranof.Workflow.Common;

namespace ATE.Views.Admin
{
    /// <summary>
    /// TestingProjectEditorView.xaml 的交互逻辑
    /// </summary>
    public partial class TestingProjectEditorView : UserControl
    {
        public TestingProjectEditorView()
        {
            InitializeComponent();
        }

        private void NodeSelectTree_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source != null)
                {
                    var view = sender as ListView;
                    if (view.SelectedItem == null) return;
                    DataObject data = new DataObject(DataTypeExtension.DragDataModelFormat, (view.SelectedItem as NodeItemMeta).Type);
                    DragDrop.DoDragDrop(view, data, DragDropEffects.Move);
                }
            }
        }
    }
}
