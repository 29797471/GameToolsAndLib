using System.Collections.ObjectModel;
using System.Windows;

namespace DevelopTool
{
    [Priority(20)]
    [Editor("快捷方式", "/DevelopTool;component/Res/Images/Icons/app.ico", System.Windows.Controls.Orientation.Horizontal)]
    public class ShortcutModel : SingleModel<ShortcutModel>
    {
        public override Setting Setting { get { return setting; } }
        public ShortcutSetting setting { get { return SettingModel.instance.GetSetting<ShortcutSetting>(); } }
        
        [ListBoxObject(null), Width(800), Height(500)]
        [DropHandle("OnDrop")]
        public ObservableCollection<ShortcutData> NodeList { get { return mNodeList; } set { mNodeList = value;Update("NodeList"); } }
        ObservableCollection<ShortcutData> mNodeList;

        public ShortcutModel()
        {
            string str = FileOpr.ReadFile(setting.SetPath);
            NodeList = Torsion.TryDeserialize<ObservableCollection<ShortcutData>>(str);
        }
        
        public override bool OnSave()
        {
            FileOpr.SaveFile(setting.SetPath, Torsion.Serialize(NodeList));
            return true;
        }

        public override System.Collections.IEnumerator MakeFiles()
        {
            yield return null;
        }

        public void OnDrop(object sender, DragEventArgs e)
        {
            if (e.RoutedEvent == DragDrop.DropEvent)
            {
                var d = e.Data.GetFormats();
                if (e.Data.GetDataPresent("FileDrop"))
                {
                    string[] files = e.Data.GetData("FileDrop") as string[];
                    foreach (var file in files)
                    {
                        /*
                        if (FileLnk.IsLnk(file))
                        {
                            CustomMessageBox.ShowDialog("使用快捷方式还是链接的源文件", "", (result) =>
                            {
                                if (result)
                                {
                                    NodeList.Add(new ShortcutData()
                                    {
                                        Btn = new ShortBtn()
                                        {
                                            Name = FileOpr.GetNameByShort(file),
                                            ShortcutPath = file
                                        }
                                    });
                                }
                                else
                                {
                                    
                                    NodeList.Add(new ShortcutData()
                                    {
                                        Btn = new ShortBtn()
                                        {
                                            Name = FileOpr.GetNameByShort(file),
                                            ShortcutPath = FileLnk.GetLnk(file).TargetPath
                                        }
                                    });
                                }
                                Update("NodeList");
                            });
                        }
                        else*/
                        {
                            
                            NodeList.Add(new ShortcutData()
                            {
                                Btn = new ShortBtn()
                                {
                                    Name = FileOpr.GetNameByShort(file),
                                    ShortcutPath = file
                                }
                            });
                            Update("NodeList");
                        }

                    }

                    //MessageBox.Show(Torsion.Serialize(e.Data.GetData("FileDrop")));
                    //MessageBox.Show(Torsion.Serialize(e.Data.GetData("FileName")));
                    //MessageBox.Show(Torsion.Serialize(e.Data.GetData("FileNameW")));
                }
            }

        }
    }
}

