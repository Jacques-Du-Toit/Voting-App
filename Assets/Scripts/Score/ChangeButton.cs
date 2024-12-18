using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    UIScore UIScript;

    void Start()
    {
        UIScript = UIManager.GetComponent<UIScore>();
    }

    public void ChangeSystem(int direction)
    {
        if (direction != 1 && direction != -1)
        {
            Debug.LogError($"Direction {direction} from ChangeButton not valid.");
            return;
        }
        UIScript.ChangeSystem(direction);
    }
}
