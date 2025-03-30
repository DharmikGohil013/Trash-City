using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button continueButton;
    private string[] dialogues = {
        "Hey, Welcome to Trash Town! I'm the only one left in this once-thriving town.",
        "Today, I appoint you as the mayor. Please restore our town to its former glory!"
    };
    private int currentDialogueIndex = 0;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (continueButton == null || dialogueText == null || gameManager == null)
        {
            Debug.LogError("Missing references in DialogueManager!");
            return;
        }
        continueButton.onClick.AddListener(NextDialogue);
        ShowDialogue();
    }

    void ShowDialogue()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
        }
        else
        {
            gameObject.SetActive(false); // Hide dialogue UI
            gameManager.StartGame();     // Show truck button after dialogues
        }
    }

    void NextDialogue()
    {
        currentDialogueIndex++;
        ShowDialogue();
    }

    void OnDestroy()
    {
        continueButton.onClick.RemoveListener(NextDialogue);
    }
}
