
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Proto
{
    [EditorAttribute]
    public class ProtoOption : BaseTreeNotifyObject
    {
        [PriorityAttribute(1)]
        [TextBox("键")]
        public string OKey { get { return mOKey; } set { mOKey = value; Update("OKey"); } }
        public string mOKey;

        [PriorityAttribute(2)]
        [TextBox("值")]
        public string OValue { get { return mOValue; } set { mOValue = value; Update("OValue"); } }
        public string mOValue;

    }
    public enum EOptimize
    {
        /// <summary>
        /// protocol buffer编译器将通过在消息类型上执行序列化、语法分析及其他通用的操作。这种代码是最优的。
        /// </summary>
        SPEED,
        /// <summary>
        /// protocol buffer编译器将会产生最少量的类，通过共享或基于反射的代码来实现序列化、语法分析及各种其它操作。
        /// 采用该方式产生的代码将比SPEED要少得多， 但是操作要相对慢些。当然实现的类及其对外的API与SPEED模式都是一样的。
        /// 这种方式经常用在一些包含大量的.proto文件而且并不盲目追求速度的 应用中。
        /// </summary>
        CODE_SIZE,
        /// <summary>
        /// protocol buffer编译器依赖于运行时核心类库来生成代码（即采用libprotobuf-lite 替代libprotobuf）。
        /// 这种核心类库由于忽略了一 些描述符及反射，要比全类库小得多。
        /// 这种模式经常在移动手机平台应用多一些。编译器采用该模式产生的方法实现与SPEED模式不相上下，
        /// 产生的类通过实现 MessageLite接口，但它仅仅是Messager接口的一个子集。
        /// </summary>
        LITE_RUNTIME,
    }

}

