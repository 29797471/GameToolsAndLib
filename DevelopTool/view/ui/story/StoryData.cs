using DevelopTool;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TimeLineTool;

[Editor("剧情片段",null,typeof(StoryUserControl))]
public class StoryData : NotifyObject
{
    public string Name
    {
        get { if (name == null) name = "temp"; return name; }
        set { name = value; Update("Name"); }
    }
    public string name;

    public int StoryId { get { return storyId; } set { storyId = value; Update("StoryId"); } }
    public int storyId;

    public int timeline;

    
    public ObservableCollection<TimeLineList> LineList
    {
        get { if (mLineList == null) mLineList = new ObservableCollection<TimeLineList>(); return mLineList; }
        set { mLineList = value; }
    }
    public ObservableCollection<TimeLineList> mLineList;
}
public class TimeLineList: NotifyObject,IDropTarget
{
    public ObservableCollection<ITimeLineDataItem> LineData
    {
        get { if (mLineData == null) mLineData = new ObservableCollection<ITimeLineDataItem>(); return mLineData; }
        set { mLineData = value; }
    }

    public ObservableCollection<ITimeLineDataItem> mLineData;
    DateTime? IDropTarget._DragOverTime
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }


    void IDropTarget.DragOver(DropInfo dropInfo)
    {
        //if (dropInfo.Data is PupilViewModel && dropInfo.TargetItem is SchoolViewModel)
        //{
        //    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        //    dropInfo.Effects = DragDropEffects.Move;
        //}
    }

    void IDropTarget.DragOut(DropInfo dropInfo)
    {
        //throw new NotImplementedException();
    }

    void IDropTarget.Drop(DropInfo dropInfo)
    {
        //SchoolViewModel school = (SchoolViewModel)dropInfo.TargetItem;
        //PupilViewModel pupil = (PupilViewModel)dropInfo.Data;
        //school.Pupils.Add(pupil);
        //((IList)dropInfo.DragInfo.SourceCollection).Remove(pupil);
    }
}