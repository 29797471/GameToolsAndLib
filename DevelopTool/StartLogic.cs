using CodeStyle;
using CqCore;
using CqEvent;
using Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using TimeLineTool;
using Trigger;
using WinCore;

namespace DevelopTool
{
    public static class StartLogic
    {
        
        public static void InitModel()
        {
            AssemblyUtil.RegType(
                typeof(Color),
                typeof(MakeFile),
                typeof(ObservableCollection<>),
                typeof(ExprCodeTemplate),
                typeof(ValueCodeTemplate),
                typeof(EventArgCodeTemplate),
                typeof(TriggerNode),
                typeof(TriggerGroup),
                typeof(EventNode),
                typeof(EventGroup),
                typeof(GuideNode),
                typeof(GuideLink),
                typeof(CodeStyleNode),
                typeof(CodeStyleGroup),
                typeof(TempDataType)
            );
            AssemblyUtil.RegType(AssemblyUtil.GetTypesByNamespace("CqBehavior.Task"));

            modelList = new List<IModel>();
            var tts = AssemblyUtil.GetTypesByNamespace("DevelopTool").ToList().FindAll(x => x.GetInterface("IModel") != null);
            foreach (var it in tts)
            {
                if (it.IsGenericType == true) continue;
                var x = AssemblyUtil.GetStaticMemberValue(it,"instance") as IModel;
                if(x==null)
                {
                    throw new Exception(string.Format("初始化模块({0})失败，检查配置是否有冲突", it));
                }
                else
                {
                    modelList.Add(x);
                }
            }
            ListUtil.Sort(modelList, x =>
            {
                var attr = AssemblyUtil.GetClassAttribute<PriorityAttribute>(x);
                return attr != null ? attr.pri1 : 0;
            });
        }
        public static List<IModel> modelList;
        public static void CMDMakeAll()
        {
            foreach (var it in modelList)
            {
                it.MakeFileCommand();
            }
        }
        static IModel mSelect;
        public static IModel Select
        {
            get
            {
                return mSelect;
            }
            set
            {
                if(mSelect!=value)
                {
                    if(mSelect!=null)mSelect.OnHide();
                    mSelect = value;
                    if (mSelect != null) mSelect.OnShow();
                    UserSetting.Data.LastSelectItem = modelList.IndexOf(mSelect);
                    if (OnSelectChange!=null) OnSelectChange();
                }
            }
        }
        public static Action OnSelectChange;
    }
}
