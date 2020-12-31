using System;
using System.Collections.Generic;

namespace CqCore
{
    /// <summary>
    /// 按队列先后逐一处理事务
    /// </summary>
    public class QueueAction
    {
        Queue<Action<Action>> actions;
        /// <summary>
        /// 按队列先后逐一处理事务
        /// </summary>
        public QueueAction()
        {
            actions = new Queue<Action<Action>>();
        }

        /// <summary>
        /// 正在处理事务
        /// </summary>
        public bool Doing
        {
            get
            {
                return actions.Count > 0;
            }
        }
        public void AddAction(Action<Action> act)
        {
            actions.Enqueue(act);
            if (actions.Count == 1)
            {
                Next();
            }
        }
        public void Next()
        {
            if (actions.Count > 0)
            {
                actions.Peek()?.Invoke(Complete);
            }
        }
        void Complete()
        {
            actions.Dequeue();
            Next();
        }
    }
}
