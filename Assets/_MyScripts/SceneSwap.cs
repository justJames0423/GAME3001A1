using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwap : MonoBehaviour
{
    public void ButtonClick()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
