using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    GameObject dialogueOption;

    GameObject dialogueMenu;

    [SerializeField]
    GameObject dialogueText;

    [SerializeField]
    string relativePath;

    //filled with dummy values. need to generalise
   public void StartDialogue()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
        GameObject canvas = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        dialogueMenu = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        GameObject button = canvas.transform.GetChild(0).gameObject;
        canvas.SetActive(false);
        button.SetActive(false);
        dialogueMenu.SetActive(true);


        string path = Path.Combine(Application.streamingAssetsPath, relativePath);
        string[] dialogueTree = File.ReadAllLines(path);
        List<DialogueNode> dialogueList = new List<DialogueNode>();
        List<List<string>> responseList = new List<List<string>>();


        /*int pos = 0;
        for (int i = 0; i < Int32.Parse(dialogueTree[0]); i++)
        {
            for(int j = pos; pos<pos+5; j++)
            {
                if j
            }
        } */


        DialogueNode node1 = new DialogueNode();
        DialogueNode node2 = new DialogueNode();
        DialogueNode node3 = new DialogueNode();

        node1.text = "Stop right there! State your business here.";
        node2.text = "Show me the letter.";
        node3.text = "Ok big man. Let's see what you've got. [COMBAT]";

        node1.responses = new List<KeyValuePair<string, DialogueNode>>();
        node1.numResponses = 2;

        node1.responses.Add(new KeyValuePair<string, DialogueNode>("I received a letter from the king, he is expecting me.", node2));
        node1.responses.Add(new KeyValuePair<string, DialogueNode>("Let me in you stupid naai.", node3));

        node3.isEndNode = true;

        DialogueNode node4 = new DialogueNode();
        DialogueNode node5 = new DialogueNode();

        node4.text = "I see. Right this way.";
        node5.text = "You won't be getting in. Out of my sight.";
        node4.isEndNode = true;
        node5.isEndNode = true;


        node2.responses = new List<KeyValuePair<string, DialogueNode>>();
        node2.numResponses = 2;

        node2.responses.Add(new KeyValuePair<string, DialogueNode>("[Show letter]", node4));
        node2.responses.Add(new KeyValuePair<string, DialogueNode>("This letter is not to be seen by commoners.", node5));






        //node4.responses = new List<KeyValuePair<string, DialogueNode>>();
        //node4.numResponses = 2;


        GoIntoThisNode(node1);

        //displayText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = node1.text;


        //optionButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I like this.";
        //optionButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "I don't like this.";

    }

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
            endDialog.SetActive(true);
            endDialog.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        Destroy(gameObject.transform.GetChild(0).gameObject); //idk
    }
}
