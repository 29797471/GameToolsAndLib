using CqCore;
using ResModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

public class TreeNodeX : TreeNode
{
    [MenuItem("打开文件"), Priority(1)]
    public void OpenFileOrFolder(TreeNode SelectedNode/*, Type type*/)
    {
        if (SelectedNode == null) return;
        if (SelectedNode.nodeObj is FolderNode)
        {
            var node = SelectedNode.nodeObj as FolderNode;
            FileOpr.RunByRelativePath(node.Path);
        }
        else if (SelectedNode.nodeObj is FileNode)
        {
            var node = SelectedNode.nodeObj as FileNode;
            FileOpr.RunByRelativePath(node.Path);
        }
    }

    [MenuItem("打开文件位置"), Priority(2)]
    public void OpenPosByFileOrFolder(TreeNode SelectedNode/*, Type type*/)
    {
        if (SelectedNode == null) return;
        if(SelectedNode.nodeObj is FolderNode)
        {
            var node = SelectedNode.nodeObj as FolderNode;
            ProcessUtil.OpenFileOrFolderByExplorer(node.Path);
        }
        else if(SelectedNode.nodeObj is FileNode)
        {
            var node = SelectedNode.nodeObj as FileNode;
            ProcessUtil.OpenFileOrFolderByExplorer(node.Path);
        }
    }
}
