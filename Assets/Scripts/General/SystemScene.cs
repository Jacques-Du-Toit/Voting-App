using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemScene : MonoBehaviour
{
    [SerializeField] TMP_Text systemNameText;

    public void SwitchScene()
    {
        SceneManager.LoadScene(systemNameText.text);
    }
}
