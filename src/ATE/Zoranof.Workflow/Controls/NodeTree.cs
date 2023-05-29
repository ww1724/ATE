using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using Zoranof.WorkFlow;

namespace Zoranof.Workflow.Controls
{
    public class NodeItemMeta
    {
        public string Title { get; set; }

        public string Guid { get; set; }

        public Dictionary<string, object> Attrs { get; set; }

        public Type Type { get; set; }

    }
    public class NodeTree : ListView
    {
        static NodeTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeTree), new FrameworkPropertyMetadata(typeof(NodeTree)));
        }

        private ObservableCollection<NodeItemMeta> m_types = new();
        private List<string> m_loadedAssemblyFiles { get; set; }

        public NodeTree()
        {
            Loaded += NodeTree_Loaded;
        }

        private void NodeTree_Loaded(object sender, RoutedEventArgs e)
        {

        }

        


        // public slots
        public void LoadAssembly(string assemblyFile) {
            m_loadedAssemblyFiles.Add(assemblyFile);
        }
        
    }
}
