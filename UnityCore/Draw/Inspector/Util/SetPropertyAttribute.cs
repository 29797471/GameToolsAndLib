// Copyright (c) 2014 Luminary LLC
// Licensed under The MIT License (See LICENSE for full text)
using UnityEngine;
using System.Collections;

/*
 * [SerializeField, SetProperty("Number")]
	private float number;
	public float Number
	{
		get
		{
			return number;
		}
		private set
		{
			number = Mathf.Clamp01(value);
		}
	}
 */
public class SetPropertyAttribute : PropertyAttribute
{
	public string Name { get; private set; }
    public GUIContent Label { get; private set; }
    public bool IsDirty { get; set; }

	public SetPropertyAttribute(string name,string label=null)
	{
		this.Name = name;
        if(label!=null) this.Label = new GUIContent(label);
	}
}




