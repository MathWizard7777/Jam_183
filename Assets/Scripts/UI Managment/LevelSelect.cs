using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private MachineDataScriptableObject[] levels;
    [SerializeField] private UIDocument document;

    private Button backButton;
    private Button[] levelButtons;

    void Start()
    {
        VisualElement root = document.rootVisualElement;

        levelButtons = new Button[20];

        int total = Mathf.Min(levels.Length, 20);
        levelButtons = new Button[total];

        for (int n = 1; n <= total; n++)
        {
            var btn = root.Q<Button>($"Level{n}");
            if (btn == null) Debug.LogWarning($"Level button not found: Level{n}");
            else
            {
                levelButtons[n - 1] = btn;
                btn.RegisterCallback<ClickEvent>(LoadLevelN(n));
            }
        }

        backButton = root.Q<Button>("Back");
        backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);
    }

    private void OnBackButtonClick(ClickEvent evt)
    {
        SceneManager.LoadScene("Main Menu");
    }

    private EventCallback<ClickEvent> LoadLevelN(int n)
    {
        return evt =>
        {
            LevelDataHolder.selectedLevelData = levels[n - 1];
            SceneManager.LoadScene("Level");
        };
    }
}