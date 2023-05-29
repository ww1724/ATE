using Zoranof.WorkFlow;

namespace Zoranof.Workflow.Common
{
    public static class DataTypeExtension
    {
        public static readonly string DragDataModelFormat = "NodeItemType";
    }

    public class NodeItemMeta
    {
        public string Title { get; set; }

        public string Guid { get; set; }

        public string Group { get; set; }

        public Dictionary<string, object> Attrs { get; set; }

        public Type Type { get; set; }

    }

    #region Enums
    public enum ExecOptionType
    {
        From, To, Input, Oupput
    }

    public enum Alignment
    {
        AlignLeft, AlignRight, AlignHCenter, AlignJustify, AlignTop, AlignBottom, AlignVCenter, AlignBaseline
    }

    public enum OptionLocation
    {
        Left, Top, Right, Bottom
    }


    public enum ItemSelectionMode
    {
        ContainsItemShape, IntersectsItemShape, ContainsItemBoundingRect, IntersectsItemBoundingRect,
    }
    #endregion


    public class WorkflowEventArgs : EventArgs
    {
        private WorkflowNode item;

        public WorkflowNode Item
        {
            get { return item; }
            set { item = value; }
        }

        public WorkflowEventArgs()
        {

        }

        public WorkflowEventArgs(WorkflowNode item) { Item = item; }
    }

}
