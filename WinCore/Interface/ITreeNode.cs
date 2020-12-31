using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ITreeNode
{
    void SetParent(IList node);
    IList GetParent();
    IList GetChildList();
}