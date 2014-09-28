using System.ComponentModel;

namespace ListTreeView
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Tree = new TreeList();
            for (int i = 0; i < 3; ++i)
            {
                this.Tree.AddChild(new TreeViewNode(null, 4));
            }

            this.AvailTree = new TreeList();
            for (int i = 0; i < 5; ++i)
            {
                this.AvailTree.AddChild(new TreeViewNode2(null, 2));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TreeList Tree { get; set; }

        public TreeList AvailTree { get; set; }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}