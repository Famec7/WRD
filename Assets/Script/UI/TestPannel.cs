using UnityEngine;

public class TestPannel : MonoBehaviour
{
    [SerializeField] private GameObject testButton;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            testButton.SetActive(!testButton.activeSelf);
        }
    }
}