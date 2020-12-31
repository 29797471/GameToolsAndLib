using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TimeLineTool
{
    public class TempDataType : NotifyObject, ITimeLineDataItem
    {
        //public float Start
        //{
        //    get { return mStart; }
        //    set { mStart = value; Update("Start"); Update("Delta"); }
        //}
        //public float mStart;

        //public float End
        //{
        //    get { return mEnd; }
        //    set { mEnd = value; Update("End");Update("Delta"); }
        //}
        //public float mEnd;

        //public float Delta
        //{
        //    get
        //    {
        //        return End - Start;
        //    }
        //}

        public DateTime? StartTime
        {
            get
            {
                if (mStartTime == null) mStartTime = DateTime.Today + new TimeSpan(startTicks) ;
                return mStartTime;
            }
            set
            {
                mStartTime = value;
                startTicks = mStartTime.Value.Ticks- DateTime.Today.Ticks;
                Update("StartTime");
                Update("Duration");
            }
        }
        DateTime? mStartTime;
        public long startTicks;
        public DateTime? EndTime
        {
            get
            {
                if (mEndTime == null) mEndTime = DateTime.Today + new TimeSpan(endTicks);
                return mEndTime;
            }
            set
            {
                mEndTime = value;
                endTicks = mEndTime.Value.Ticks- DateTime.Today.Ticks;
                Update("EndTime");
                Update("Duration");
            }
        }
        DateTime? mEndTime;
        public long endTicks;
        public bool TimelineViewExpanded { get; set; }
        public string Name { get { return mName; } set { mName = value; Update("Name"); } }
        public string mName;
        public TimeSpan? Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }
    }
}
