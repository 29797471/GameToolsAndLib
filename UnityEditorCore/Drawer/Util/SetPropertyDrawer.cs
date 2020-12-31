// Copyright (c) 2014 Luminary LLC
// Licensed under The MIT License (See LICENSE for full text)
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

/// <summary>
/// 提供Inspector编辑属性时,回调方法
/// </summary>
[CustomPropertyDrawer(typeof(SetPropertyAttribute))]
public class SetPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Rely on the default inspector GUI
		EditorGUI.BeginChangeCheck ();

		// Update only when necessary
		SetPropertyAttribute setProperty = attribute as SetPropertyAttribute;

        //EditorGUI.PropertyField(position, property, setProperty.Label==null?label:setProperty.Label);
        if (EditorGUI.EndChangeCheck())
		{
			// When a SerializedProperty is modified the actual field does not have the current value set (i.e.  
			// FieldInfo.GetValue() will return the prior value that was set) until after this OnGUI call has completed. 
			// Therefore, we need to mark this property as dirty, so that it can be updated with a subsequent OnGUI event 
			// (e.g. Repaint)
			setProperty.IsDirty = true;
		} 
		else if (setProperty.IsDirty)
		{
			// The propertyPath may reference something that is a child field of a field on this Object, so it is necessary
			// to find which object is the actual parent before attempting to set the property with the current value.
            var v = AssemblyUtil.GetMemberValue(property.serializedObject.targetObject, property.propertyPath);
            
            AssemblyUtil.SetMemberValue(property.serializedObject.targetObject, setProperty.Name, v);
			setProperty.IsDirty = false;
		}
	}
	
}
