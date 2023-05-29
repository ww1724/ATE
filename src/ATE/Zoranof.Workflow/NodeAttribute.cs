using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoranof.Workflow
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeAttribute : Attribute
    {
        public NodeAttribute(string title="", string group="base", string name = "", string description = "")
        {
            Title = title;
            Group = group;
            Name = name;
            Description = description;
        }

        private string title;
        private string group;
        private string name;
        private string description;


        public string Title { get => title; set => title = value; }

        public string Name { get => name; set => name = value; }

        public string Group { get => group; set => group = value; }

        public string Description { get => description; set => description = value; }



    }
}
