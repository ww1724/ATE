using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Test.Contract
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestingProjectAttribute : Attribute
    {
        public TestingProjectAttribute(string name, string title = "", string group = "base", string description = "")
        {
            Title = title;
            Group = group;
            Name = name;
            Description = description;
        }

        private string name;
        private string title;
        private string description;
        private string group;

        public string Title { get => title; set => title = value; }

        public string Name { get => name; set => name = value; }

        public string Group { get => group; set => group = value; }

        public string Description { get => description; set => description = value; }



    }
}
