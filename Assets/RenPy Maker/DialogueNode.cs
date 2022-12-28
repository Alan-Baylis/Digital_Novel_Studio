﻿using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Nodes/Dialogue")]
public class DialogueNode : BaseNode
{
    [Input] public int entry;

    [TextArea]
    public string dialogue;
    public string character;
    public Texture2D image;
    public Color color = Color.white;
    [HideInInspector]
    public bool initialized; 
    [HideInInspector]
    [SerializeField]
    public int _characterIndex;
    [HideInInspector]
    public bool errorStatus;
    private int jumpIndex;
    private bool evaluated;
    private List<string> labels = new List<string>();

    private void Reset()
    {
        this.AddDynamicOutput(typeof(int), ConnectionType.Override, TypeConstraint.None, "exit");
    }
    
    public int GetCharacterIndex()
    {
        return _characterIndex;
    }
    
    public void SetCharacterIndex(int res)
    {
        _characterIndex = res;
    }
    
    public override string GetCharacter()
    {
        return character;
    }

    public override void SetCharacter(string characterName)
    {
        character = characterName;
    }

    public override Color GetColor()
    {
        return color;
    }

    public override string GetNodeType()
    {
        return "DialogueNode";
    }

    public override string GetDialogue()
    {
        return dialogue;
    }

    public override Texture2D GetImage()
    {
        return image;
    }

    public override void SetJumpIndex(int index)
    {
        jumpIndex = index;
    }

    public override int GetJumpIndex()
    {
        return jumpIndex;
    }

    public override void AddToLabelList(string newLabel)
    {
        labels.Add(newLabel);
    }

    public override List<string> GetLabelList()
    {
        return labels;
    }
	
    public override void ClearLabelList()
    {
        labels.Clear();
    }

    public override bool GetEvaluated()
    {
        return evaluated;
    }

    public override void SetEvaluated(bool flag)
    {
        evaluated = flag;
    }

    public override void SetError()
    {
        errorStatus = true;
    }

    public override string GetError()
    {
        errorStatus = false;
        
        foreach (NodePort p in Inputs)
            if (!p.IsConnected)
                errorStatus = true;

        foreach (NodePort p in Outputs)
            if (!p.IsConnected)
                errorStatus = true;

        if (errorStatus)
            return "Unconnected ports";

        if (!RenpyMaker.CheckForAlphaNumeric(character))
        {
            errorStatus = true;
            return "Character name must start with a letter and use alphanumeric characters";
        }

        NodePort port = GetOutputPort("exit");
        if (port != null)
        {
            BaseNode nextNode = port.Connection.node as BaseNode;
            if (nextNode != null && nextNode == this)
            {
                errorStatus = true;
                return "Cannot connect outputs to self";
            }
        }

        if (string.IsNullOrEmpty(character))
        {
            errorStatus = true;
            return "No character name";
        }

        if (string.IsNullOrEmpty(dialogue))
        {
            errorStatus = true;
            return "No dialogue text";
        }

        if (image == null)
        {
            errorStatus = true;
            return "No image texture";
        }

        return "No Error";
    }
}