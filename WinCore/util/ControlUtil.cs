using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

public class ControlUtil
{
    public static void ListViewAutoWidth(ListView listView)
    {
        GridView gv = listView.View as GridView;
        if (gv != null)
        {
            foreach (GridViewColumn gvc in gv.Columns)
            {
                gvc.Width = gvc.ActualWidth;
                gvc.Width = Double.NaN;
            }
        }
    }
    public static TreeViewItem FindTreeViewItem(ItemsControl item, object data)
    {
        TreeViewItem findItem = null;
        bool itemIsExpand = false;
        if (item is TreeViewItem)
        {
            TreeViewItem tviCurrent = item as TreeViewItem;
            itemIsExpand = tviCurrent.IsExpanded;
            if (!tviCurrent.IsExpanded)
            {
                //如果这个TreeViewItem未展开过，则不能通过ItemContainerGenerator来获得TreeViewItem
                tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, true);
                //必须使用UpdaeLayour才能获取到TreeViewItem
                tviCurrent.UpdateLayout();
            }
        }
        for (int i = 0; i < item.Items.Count; i++)
        {
            TreeViewItem tvItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
            if (tvItem == null)
                continue;
            object itemData = item.Items[i];
            if (itemData == data)
            {
                findItem = tvItem;
                break;
            }
            else if (tvItem.Items.Count > 0)
            {
                findItem = FindTreeViewItem(tvItem, data);
                if (findItem != null)
                    break;
            }

        }
        if (findItem == null)
        {
            TreeViewItem tviCurrent = item as TreeViewItem;
            tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, itemIsExpand);
            tviCurrent.UpdateLayout();
        }
        return findItem;
    }
}