using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

///<summary>
///The GameManager to the Start Scene of the game to wait for a player's input to 
/// change scene and start the game
///</summary>
public class GameManager : MonoBehaviour {

    public string sceneName;        //Name of Scene to change too
    int _start_direction;        //Stores the direction of the arrow key pressed
    string _scoreKey = "VALUE_SCORE";   //Stores the PlayerPref key for Score
    string _livesKey = "VALUE_LIVES";   // Stores the PlayerPref key for Lives
    string _directionKey = "VALUE_DIRECTION";   // Stores the PlayerPref key for Direction

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.DeleteAll();    
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _start_direction = 0;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _start_direction = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _start_direction = 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _start_direction = 3;
        }

        if (Input.anyKeyDown)
        {
            PlayerPrefs.SetInt(_directionKey, _start_direction);
            ChangeSceneWithName();
        }
    }

    ///<summary>
    ///Changes the Scene to the Scene with the name entered.
    ///</summary>
    public void ChangeSceneWithName()
    {
        SceneManager.LoadScene(sceneName);
    }
}
