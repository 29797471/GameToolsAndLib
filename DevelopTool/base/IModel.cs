using DevelopTool;
using System.Collections;

public interface IModel
{
    bool Save();
    bool OnSave();
    IEnumerator MakeFiles();
    void MakeFileCommand();
    void OnShow();
    void OnHide();
    Setting Setting { get; }


}