using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using CqCore;

/// <summary>
/// 通过面板编辑器选择一个组件的属性或者字段或者方法
/// </summary>
public class ComponentPropertyWindow : EditorWindow
{
    /// <summary>
    /// 弹出一个面板,列出obj 的组件列表,
    /// 选中组件后列出所有是allowTypes之中的类型的成员.
    /// 回调选中的成员
    /// </summary>
    public void EditObject(GameObject obj, Action<Component, string, MemberTypes> OnSelectProperty, Type[] allowTypes = null, MemberTypes memberTypes = MemberTypes.Property)
    {
        Show();
        _go = obj;
        this.OnSelectTarget = OnSelectProperty;
        this.allowTypes = allowTypes;
        this.memberTypes = memberTypes;
    }
    static ComponentPropertyWindow mInstance;
    public static ComponentPropertyWindow Instance
    {
        get
        {
            if (mInstance == null) mInstance = EditorWindow.GetWindow<ComponentPropertyWindow>("属性关联窗口");
            return mInstance;
        }
    }
    private int selectionIndex = -1;


    private Component[] arrComponents
    {
        get
        {
            if (_arrComponents == null && _go!=null)
            {
                _arrComponents = _go.GetComponents(typeof(Component));
            }
            return _arrComponents;
        }
    }
    Component[] _arrComponents;


    private GameObject _go;
    private Vector2 scrollView;
    /// <summary>
    /// Component 组件,string 属性名/字段名/方法名
    /// </summary>
    private Action<Component, string, MemberTypes> OnSelectTarget;
    //private Vector2 scrollViewComponent;
    const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

    void OnEnable()
    {
        mInstance = this;

        this.titleContent.text = "ComponentPropertyWindow";
        this.minSize = new Vector2(273f, 102f);
        this.wantsMouseMove = true;
        scrollView = new Vector2(0f, 0f);
        // define styles		
    }
    void OnDisable()
    {
        mInstance = null;
    }
    void OnHierarchyChange()
    {
    }
    Type[] allowTypes;
    MemberTypes memberTypes;
    
    void Update()
    {
        if (EditorWindow.mouseOverWindow == this) this.Repaint();
    }
    void OnGUI()
    {
        if (_go == null) return;
        GUILayout.Label(_go.name + " - 选择一个组件的属性" /*, styleLabel*/);
        scrollView = GUILayout.BeginScrollView(scrollView);
        GUILayout.BeginHorizontal();
        // field button
        if (GUILayout.Button("null", GUILayout.MinWidth(150f)))
        {
            OnSelectTarget(null, null, 0);
            Close();
        }
        GUILayout.EndHorizontal();
        if (arrComponents != null && arrComponents.Length > 0)
        {
            for (int i = 0; i < arrComponents.Length; i++)
            {
                //当丢失时跳过
                var myComponent = arrComponents[i];
                if (myComponent == null || myComponent.GetType() == typeof(Behaviour)) continue;

                // component button
                GUILayout.BeginHorizontal(GUILayout.Width(position.width - 5f));
                string componentName = myComponent.GetType().Name;
                if (GUILayout.Button(componentName/*,buttonStyle*/))
                {
                    if (selectionIndex != i) selectionIndex = i;
                    else selectionIndex = -1;
                }
                string lblToggle;
                if (selectionIndex != i) lblToggle = "+";
                else lblToggle = "-";

                GUILayout.Label(lblToggle, GUILayout.Width(15f));

                GUILayout.EndHorizontal();

                if (selectionIndex == i)
                {
                    int numberOfProperties = 0;
                    if (MathUtil.StateCheck(memberTypes, MemberTypes.Method))
                    {
                        var Methods = myComponent.GetType().GetMethods();

                        foreach (var methodInfo in Methods)
                        {
                            if (methodInfo.ReturnType== typeof(void))
                            {
                                GUILayout.BeginHorizontal();
                                // field button
                                if (GUILayout.Button(methodInfo.Name+"()", GUILayout.MinWidth(150f)))
                                {
                                    OnSelectTarget(myComponent, methodInfo.Name, MemberTypes.Method);
                                    Close();
                                }
                                GUILayout.EndHorizontal();
                                numberOfProperties++;
                            }
                        }
                    }
                    
                    if(MathUtil.StateCheck( memberTypes,MemberTypes.Field))
                    {
                        var fields = myComponent.GetType().GetFields();
                        // loop through all fields sfields
                        foreach (FieldInfo fieldInfo in fields)
                        {
                            if (isValidType(fieldInfo.FieldType))
                            {
                                GUILayout.BeginHorizontal();
                                // field button
                                if (GUILayout.Button(fieldInfo.Name, GUILayout.MinWidth(150f)))
                                {
                                    OnSelectTarget(myComponent, fieldInfo.Name, MemberTypes.Field);
                                    Close();
                                }
                                GUILayout.Label(fieldInfo.GetValue(myComponent).ToString());
                                GUILayout.EndHorizontal();
                                numberOfProperties++;
                            }
                        }
                    }
                    if (MathUtil.StateCheck(memberTypes, MemberTypes.Property))
                    {
                        var Properties = myComponent.GetType().GetProperties();

                        foreach (PropertyInfo propertyInfo in Properties)
                        {
                            if ( isValidType(propertyInfo.PropertyType))
                            {
                                GUILayout.BeginHorizontal();
                                if (GUILayout.Button(string.Format("{0} {{{1}{2}}}",propertyInfo.Name, propertyInfo.CanWrite?"set;":"",propertyInfo.CanRead?"get;":""), GUILayout.MinWidth(150f)))
                                {
                                    OnSelectTarget(myComponent, propertyInfo.Name, MemberTypes.Property);
                                    Close();
                                }
                                if(propertyInfo.CanRead)
                                {
                                    object propertyValue = AssemblyUtil.GetMemberValue(myComponent, propertyInfo.Name);
                                    if (propertyValue == null) propertyValue = "null";
                                    GUILayout.Label(propertyValue.ToString());
                                }
                                else
                                {
                                    GUILayout.Label("");
                                }
                                GUILayout.EndHorizontal();
                                numberOfProperties++;
                            }
                        }
                    }
                        
                    if (numberOfProperties <= 0)
                    {
                        GUILayout.Label("No found");
                    }
                    //GUILayout.EndScrollView();
                }
            }
        }
        GUILayout.EndScrollView();
    }
    /// <summary>
    /// 是否是一个满足需求的类型
    /// </summary>
    public bool isValidType(Type t)
    {
        return allowTypes.Any(x => x == t || x == t.BaseType);
        //return allowTypes.ToList().Contains(t);
    }
    
    
}
