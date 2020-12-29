using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : MonoBehaviour
{
    public string text;
    public List <KeyValuePair<string, DialogueNode>> responses;
    public int numResponses;
    public bool isEndNode;

}
