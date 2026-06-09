using UnityEngine;

public class Pausemenu : MonoBehaviour
{
    public GameObject gameObject;
    public void Start()
    {
       gameObject.SetActive(false); 
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            gameObject.SetActive(true); 
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1;
                Debug.Log("Game Resumed");
            }
           
        } 
        if (Time.timeScale == 0)
            {
                gameObject.SetActive(true); 
            }
            else
            {
                gameObject.SetActive(false);    
            }
    }
}
