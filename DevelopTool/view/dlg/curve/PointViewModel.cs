using CqCore;

namespace DevelopTool
{
    public class PointViewModel : NotifyObject
    {

        public PointViewModel(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        private float x;

        public float X
        {
            get { return x; }
            set
            {
                
                if (x.EqualsByEpsilon( value)) return;
                x = value;
                Update("X");
            }
        }


        private float y;

        public float Y
        {
            get { return y; }
            set
            {
                if (y.EqualsByEpsilon( value)) return;
                y = value;
                Update("Y");
            }
        }

    }
}
