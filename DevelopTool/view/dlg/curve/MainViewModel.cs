using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DevelopTool
{
    public class MainViewModel:NotifyObject
    {

        private ObservableCollection<PointViewModel> _Points;

        public ObservableCollection<PointViewModel> Points
        {
            get { return _Points ?? (_Points = GetAllPoints()); }
        }

        private ObservableCollection<PointViewModel> GetAllPoints()
        {
            var toRet = new ObservableCollection<PointViewModel>();

            //TODO: Add initials the items
            toRet.Add(new PointViewModel(0, 0));
            toRet.Add(new PointViewModel(0.3f, 0.5f));
            toRet.Add(new PointViewModel(1, 1f));
            foreach (var it in toRet)
            {
                it.X = it.X * 500 + 66;
                it.Y = 520 - it.Y * 500;
            }

            toRet.CollectionChanged += OnPointsCollectionChanged;

            return toRet;
        }

        private void OnPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private bool _IsClosedCurve;

        public bool IsClosedCurve
        {
            get { return _IsClosedCurve; }
            set
            {
                if (_IsClosedCurve != value)
                {
                    _IsClosedCurve = value;
                    Update("IsClosedCurve");
                }
            }
        }

    }
    
}
