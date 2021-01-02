using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    GameObject dialogueOption;

    GameObject dialogueMenu;

    [SerializeField]
    GameObject dialogueText;

    [SerializeField]
    String pathToFile;

   public void StartDialogue()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
        GameObject canvas = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        dialogueMenu = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        GameObject button = canvas.transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        button.SetActive(false);
        dialogueMenu.SetActive(true);

        //Build dialogue tree from textasset
        TextAsset dialogueFile = Resources.Load(pathToFile) as TextAsset;
        string[] dialogueTree = Regex.Split(dialogueFile.text, "\n");
        List <DialogueNode> dialogueList = new List<DialogueNode>();
        List<List<string>> responseList = new List<List<string>>();


        int pos = 2;

        int textPos = 2;
        int numResponsesPos = 3;
        int responsePos = 4;
        int endNodePos = 5;
        for (int i = 0; i < Int32.Parse(dialogueTree[0]); i++)
        {
            DialogueNode node = new DialogueNode();
            for (int j = pos; j < pos + 4; j++)
            {
               if (j == textPos)
                {
                    node.text = Regex.Split(dialogueTree[j], ": ")[1];
                }
                else if (j == numResponsesPos)
                {
                    node.numResponses = Int32.Parse(Regex.Split(dialogueTree[j], ": ")[1]);
                }
                else if(j == responsePos && node.numResponses > 0)
                {
                    string responseText = Regex.Split(dialogueTree[j], ": ")[1];
                    responseList.Add(new List<string>(responseText.Split('|')));
                }
               else if(j == endNodePos)
               {
                    node.isEndNode = bool.Parse(Regex.Split(dialogueTree[j], ": ")[1]);
               }
            }
            dialogueList.Add(node);
            pos += 6;
            textPos += 6;
            responsePos += 6;
            numResponsesPos += 6;
            endNodePos += 6;
        }

        int counter = 0;
        foreach (List<string> responses in responseList)
        {
            dialogueList[counter].responses = new List<KeyValuePair<string, DialogueNode>>();
            foreach (string responsePair in responses)
            {
                string responseText = responsePair.Split(':')[0];
                int index = Int32.Parse(responsePair.Split(':')[1][4].ToString());
                dialogueList[counter].responses.Add(new KeyValuePair<string, DialogueNode>(responseText, dialogueList[index]));
            }
            counter++;
        }


        GoIntoThisNode(dialogueList[0]);

    }

    //Loads in this node's npc lines and creates buttons for player responses to the npc. Each button will call another GoIntoThisNode.
    public void GoIntoThisNode(DialogueNode node)
    {
        

        foreach (Transform child in dialogueMenu.transform)
        {
            if (child.gameObject.name != "End Dialogue"){
                Destroy(child.gameObject);
            }
            
        }
        List<GameObject> buttons = new List<GameObject>();
        GameObject displayText = Instantiate(dialogueText);
        displayText.transform.parent = dialogueMenu.transform;
        displayText.transform.localPosition = new Vector3(0, 0, 0);
        displayText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = node.text;
        Vector3 prev = displayText.transform.localPosition;

        for (int i = 0; i<node.numResponses; i++)
        {
            buttons.Add(Instantiate(dialogueOption));
            buttons[i].transform.parent = dialogueMenu.transform;
            buttons[i].transform.localPosition = new Vector3(0, 0, 0);
            prev = buttons[i].transform.localPosition;
        }

        if (node.numResponses != 0){
            int counter = 0;
            foreach(KeyValuePair<string, DialogueNode> response in node.responses)
            {
                buttons[counter].GetComponent<Button>().GetComponentInChildren<TMPro.TextMeshProUGUI>().text = response.Key;
                //buttons[counter].GetComponent<NodeContainer>().node = response.Value;
                buttons[counter].GetComponent<Button>().onClick.AddListener(delegate { GoIntoThisNode(response.Value); }) ;
                counter++;
            }
        }

        if (node.isEndNode)
        {
            //GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
            GameObject endDialog = gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            endDialog.transform.SetAsLastSibling();
            endDialog.SetActive(true);
            endDialog.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    //Ends the dialogue by renabling the player controller and destroying dialogue components so that dialogue is not triggered again.
    public void EndDialogue()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        Destroy(gameObject.transform.GetChild(0).gameObject); //idk
    }
}
