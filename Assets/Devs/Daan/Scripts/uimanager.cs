using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class uimanager : MonoBehaviour
{
public void quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void play()
    {
   SceneManager.LoadScene("Daniël Scene");
    }
    public void credits()
    {
        SceneManager.LoadScene("");
    }

}
