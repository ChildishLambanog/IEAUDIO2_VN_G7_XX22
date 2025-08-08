using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialogueLine[] dialogueLines;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource voiceAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public Image backgroundImageUI;
    //public Image characterImageUI;

    public GameObject characterObject;
    private GameObject currentCharacterObject;


    public float typingSpeed = 0.05f; // Adjust speed here
    private Coroutine typingCoroutine;


    private int currentLine = 0;

    public GameObject choicePanel;
    public Button[] choiceButtons;

    [Header("EndScene")]
    public GameObject endProloguePanel;
    public float fadeDuration = 1f;
    public float endDelay = 2f;
    public string nextSceneName = "NextScene";

    void Start()
    {
        ShowLine();
    }

    void Update()
    {
        if (choicePanel.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            TryAdvanceDialogue();
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryAdvanceDialogue();
        }

        // Check if player presses Space or Enter
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            TryAdvanceDialogue();
        }

        // Check for mouse click anywhere not on UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryAdvanceDialogue();
        }
    }

    void TryAdvanceDialogue()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            Debug.Log("End of dialogue");
        }

        if (currentLine >= dialogueLines.Length)
        {
            StartCoroutine(ShowEndOfPrologue());
            return;
        }

    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[currentLine];


        // If it's a choice, pause dialogue and show choices
        if (line.isChoice)
        {
            ShowChoices(line.choices, line.nextLineIndices);
            return;
        }

        // Update UI
        speakerText.text = line.speaker;
        dialogueText.text = line.text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line.text));

        // Play voice
        if (line.voiceClip != null)
        {
            voiceAudioSource.clip = line.voiceClip;
            voiceAudioSource.Play();
        }

        // Change music if a music clip is defined
        if (line.musicClip != null)
        {
            musicAudioSource.clip = line.musicClip;
            musicAudioSource.Play();
        }

        if (line.sfxClip != null)
        {
            sfxAudioSource.PlayOneShot(line.sfxClip);
        }

        // Change background
        if (line.backgroundImage != null)
        {
            backgroundImageUI.sprite = line.backgroundImage;
        }


        if (currentCharacterObject != null)
        {
            currentCharacterObject.SetActive(false);
        }

        // Activate new character if defined
        if (line.characterObject != null)
        {
            currentCharacterObject = line.characterObject;
            currentCharacterObject.SetActive(true);
        }
        else
        {
            currentCharacterObject.SetActive(false);
        }

        if (line.goToSceneName != null)
        {
            nextSceneName = line.goToSceneName;
        }

        //if (line.isEndOfBranch)
        //{
        //    StartCoroutine(ShowEndOfPrologue());
        //    return;
        //}

    }


    void ShowChoices(string[] options, int[] nextIndices)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < options.Length)
            {
                int index = i; // Local copy for closure
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextIndices[index]));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnChoiceSelected(int choiceIndex) //nextLine
    {
        choicePanel.SetActive(false);
        currentLine = choiceIndex;
        ShowLine();

        //choicePanel.SetActive(false);

        //DialogueLine choiceLine = dialogueLines[currentLine];

        //DialogueLine current = dialogueLines[currentLine];


        //// Check if this choice has a scene name assigned
        ////if (choiceLine.choiceSceneNames != null)
        ////{
        ////    Debug.Log(choiceIndex);
        ////    // Override the nextSceneName for the end fade
        ////    nextSceneName = choiceLine.choiceSceneNames[choiceIndex];
        ////    StartCoroutine(ShowEndOfPrologue());
        ////}

        //if (current.choiceSceneNames != null)
        //{
        //    nextSceneName = current.choiceSceneNames[choiceIndex];
        //    StartCoroutine(ShowEndOfPrologue());
        //}
        //else
        //{
        //    // Just go to the next dialogue line
        //    currentLine = choiceIndex;//current.nextLineIndices[choiceIndex];
        //    ShowLine();
        //}
    }



    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null; // done typing
    }

    IEnumerator ShowEndOfPrologue()
    {
        endProloguePanel.SetActive(true);

        // Optional fade-in
        CanvasGroup cg = endProloguePanel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0;
            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(endDelay);

        // Optional: auto-load next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

}