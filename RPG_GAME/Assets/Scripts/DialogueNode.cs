using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents a node in a dialogue graph/tree. 
//text is typically a npc's lines.
//responses is a list of key value pairs <string, dialoguenode>. The string is a possible player response and the dialoguenode 
//is the node that is travelled to when ther player selects that response. (Conceptually a player response can be thought of as an edge between two nodes.)
//numResponses is self explanatory.
//isEndNode indicates whether this is one of the final nodes in a dialogue with an npc. End nodes will trigger an end dialogue button.
public class DialogueNode : MonoBehaviour
{
    public string text;
    public List <KeyValuePair<string, DialogueNode>> responses;
    public int numResponses;
    public bool isEndNode;

}
