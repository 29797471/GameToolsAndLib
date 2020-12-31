using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace DevelopTool
{
    /// <summary>
    /// 新手指引 设置
    /// </summary>
    [Priority(2)]
    [Editor("新手引导编辑器", "/DevelopTool;component/Res/Images/Icons/guide.ico")]
    public class GuideSetting : Setting
    {
        
        /// <summary>
        /// 生成文件导出路径
        /// </summary>
        [FolderPath("导出目录", true), Priority(3)]
        public string MakePath { get { return makePath; } set { makePath = value; Update("MakePath"); } }
        public string makePath;
        

        [Priority(4, 2)]
        [Button("指引链模版"),Click]
        public MakeFile TemplateGuideLink
        {
            get { if (mTemplateGuideLink == null) mTemplateGuideLink = new MakeFile(); return mTemplateGuideLink; }
            set { mTemplateGuideLink = value; Update("TemplateGuideLink"); }
        }
        public MakeFile mTemplateGuideLink;

        [Priority(4, 1)]
        [Button("指引导入模版"),Click]
        public MakeFile TemplateRequire
        {
            get { if (mTemplateRequire == null) mTemplateRequire = new MakeFile(); return mTemplateRequire; }
            set { mTemplateRequire = value; Update("TemplateRequire"); }
        }
        public MakeFile mTemplateRequire;


        public CustomList<KeyValue> mNodeTypesList;

        [Priority(7)]
        [ListView("节点完成方式->名称")]
        public CustomList<KeyValue> NodeTypesList
        {
            get
            {
                if (mNodeTypesList == null)
                {
                    mNodeTypesList = new CustomList<KeyValue>();
                }
                return mNodeTypesList;
            }
            set { mNodeTypesList = value; }
        }

        public CustomList<KeyValue> mLinkTypesList;

        [Priority(9)]
        [ListView("链执行方式->名称")]
        public CustomList<KeyValue> LinkTypesList
        {
            get
            {
                if (mLinkTypesList == null)
                {
                    mLinkTypesList = new CustomList<KeyValue>();
                }
                return mLinkTypesList;
            }
            set { mLinkTypesList = value; }
        }

        public CustomList<KeyValue> mRolePossList;

        [Priority(10)]
        [ListView("指引指示")]
        public CustomList<KeyValue<string>> GuideIcon
        {
            get
            {
                if (mGuideIcon == null)
                {
                    mGuideIcon = new CustomList<KeyValue<string>>();
                }
                return mGuideIcon;
            }
            set { mGuideIcon = value; }
        }

        public CustomList<KeyValue<string>> mGuideIcon;

        [Priority(11)]
        [ListView("手方向")]
        public CustomList<KeyValue<int>> GuideDir
        {
            get
            {
                if (mGuideDir == null)
                {
                    mGuideDir = new CustomList<KeyValue<int>>();
                }
                return mGuideDir;
            }
            set { mGuideDir = value; }
        }

        public CustomList<KeyValue<int>> mGuideDir;

        [Priority(12)]
        [ListView("箭头方向")]
        public CustomList<KeyValue<int>> GuideArrowDir
        {
            get
            {
                if (mGuideArrowDir == null)
                {
                    mGuideArrowDir = new CustomList<KeyValue<int>>();
                }
                return mGuideArrowDir;
            }
            set { mGuideArrowDir = value; }
        }

        public CustomList<KeyValue<int>> mGuideArrowDir;

        [Priority(15)]
        [ListView("剧情动作")]
        public CustomList<KeyValue<string>> StoryAction
        {
            get
            {
                if (mStoryAction == null)
                {
                    mStoryAction = new CustomList<KeyValue<string>>();
                }
                return mStoryAction;
            }
            set { mStoryAction = value; }
        }

        public CustomList<KeyValue<string>> mStoryAction;

    }
}