using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

///<summary>
///The Controller for a Player: including movement, handling collisions, and Spawning Pokemon.
///</summary>
public class PlayerController : MonoBehaviour
{
    public Sprite[] sprites;        // The Sprites for the Player
    public GameObject[] lifePrefabs;    //The Sprites for the Lives
    public string startSceneName;   //  Name of Start Scene
    public string sceneName;    //Name of Game Scene
    public GameObject pokemonPrefabToSpawn;     //Prefab of Pokemon to Spawn
    public GameObject pokeballPrefabToSpawn;    //Prefab of Pokeball to Spawn
    public float speed;     //Speed of the Player
    public Text score;      //Scoreboard

    List<Vector2> _positionChangeList = new List<Vector2>();    //List to store all the positions the Pokemon has to change direction at
    List<Vector2> _directionChangeList = new List<Vector2>();   //List to store all the directions the Pokemon has to change to
    List<GameObject> _pokemonList = new List<GameObject>();     //List of all the Pokemons
    Vector2 _direction;     //Current Direction of Player
    float _hGap;    //The horizontal gap between player and pokemon
    float _vGap;    //The vertical gap between player and pokemon
    int _directionCount = 0;    //The counter for direction changes
    int _spawnCount = 0;       //the counter for amount of Pokemon spawned
    bool _changed = false;      //Flag for if the direction changed
    int _scoreCount;        //The counter for the Score
    int _lives = 3;     //the amount of lives the player has
    string _scoreKey = "VALUE_SCORE";   //Stores the PlayerPref key for Score
    string _livesKey = "VALUE_LIVES";   // Stores the PlayerPref key for Lives
    string _directionKey = "VALUE_DIRECTION";   // Stores the PlayerPref key for Direction

    // Use this for initialization
    void Start()
    {
        ///Determines if the player has died and sets lives, direction, and score
        if (PlayerPrefs.HasKey(_scoreKey))
        {
            _scoreCount = PlayerPrefs.GetInt(_scoreKey);
        }
        if (PlayerPrefs.HasKey(_livesKey))
        {
            _lives = PlayerPrefs.GetInt(_livesKey);
        }
        if (PlayerPrefs.HasKey(_directionKey))
        {
            if(PlayerPrefs.GetInt(_directionKey) == 0)
            {
                _direction = Vector2.right;
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
            }
            else if (PlayerPrefs.GetInt(_directionKey) == 1)
            {
                _direction = Vector2.down;
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
            else if (PlayerPrefs.GetInt(_directionKey) == 2)
            {
                _direction = Vector2.left;
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else
            {
                _direction = Vector2.up;
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
        }
        if(_lives == 2)
        {
            Destroy(lifePrefabs[_lives]);
        }
        else if(_lives == 1)
        {
            Destroy(lifePrefabs[_lives+1]);
            Destroy(lifePrefabs[_lives]);
        }
        else if(_lives == 0)
        {
            Destroy(lifePrefabs[_lives + 2]);
            Destroy(lifePrefabs[_lives + 1]);
            Destroy(lifePrefabs[_lives]);
        }

        ///Spawns first pokeball
        GameObject pokeball = GameObject.Instantiate(pokeballPrefabToSpawn) as GameObject;
        pokeball.transform.position = new Vector2(2, 2);
        _pokemonList.Add(gameObject);
        setScore();
    }

    // Update is called once per frame
    void Update()
    {
        ///Determines if the direction changed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _directionChangeList.Add(Vector2.right);
            _positionChangeList.Add(transform.position);
            _changed = true;
            _directionCount++;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _directionChangeList.Add(Vector2.down);
            _positionChangeList.Add(transform.position);
            _changed = true;
            _directionCount++;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _directionChangeList.Add(Vector2.left);
            _positionChangeList.Add(transform.position);
            _changed = true;
            _directionCount++;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _directionChangeList.Add(Vector2.up);
            _positionChangeList.Add(transform.position);
            _changed = true;
            _directionCount++;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }

        ///If the direction changed
        if (_changed == true)
        {
            _direction = _directionChangeList[_directionCount - 1];
            if (_pokemonList.Count > 1)
            {
                ///Adds the direction the Player changed to, to the list of directions the next pokemon has to change
                _pokemonList[1].GetComponent<SpawnPokemon>().addNewDirection(_directionChangeList[_directionCount - 1]);
                ///Adds the position the Player changed direction, to the list of positions the next pokemon has to change at
                _pokemonList[1].GetComponent<SpawnPokemon>().addNewPosition(_positionChangeList[_directionCount - 1]);
            }
            else
            {
                ///If its only the Player remove changes from lists
                _directionChangeList.RemoveAt(_directionCount - 1);
                _positionChangeList.RemoveAt(_directionCount - 1);
                _directionCount--;
            }
            _changed = false;
        }
        _pokemonList[0].transform.Translate(_direction * speed);
    }

    ///<summary>
    ///Collision Detection for Player
    ///</summary>
    ///<param name=”collision”>The collision object the player has collided with</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pickup"))
        {

            GameObject pokemon = GameObject.Instantiate(pokemonPrefabToSpawn) as GameObject;
            pokemon.name = "pokemon" + _spawnCount;
            _spawnCount++;
            pokemon.GetComponent<SpawnPokemon>().setID(_spawnCount);
            int sprite = Random.Range(1, 5);
            pokemon.GetComponent<SpawnPokemon>().setSprites(sprite);
            Vector2 new_direction;

            if (_pokemonList.Count > 1)
            {
                pokemon.GetComponent<SpawnPokemon>().setDirection(_pokemonList[_pokemonList.Count - 1].GetComponent<SpawnPokemon>().getDirection());
                new_direction = _pokemonList[_pokemonList.Count - 1].GetComponent<SpawnPokemon>().getDirection();
            }
            else
            {
                pokemon.GetComponent<SpawnPokemon>().setDirection(_direction);
                new_direction = _direction;
            }
            pokemon.GetComponent<SpawnPokemon>().setPlayer(gameObject);

            if (new_direction == Vector2.right)
            {
                _hGap = -1f;
                _vGap = 0f;
            }
            else if (new_direction == Vector2.down)
            {
                _vGap = 1f;
                _hGap = 0f;
            }
            else if (new_direction == Vector2.left)
            {
                _hGap = 1f;
                _vGap = 0f;
            }
            else if (new_direction == Vector2.up)
            {
                _vGap = -1f;
                _hGap = 0f;
            }

            pokemon.transform.position = (Vector2)_pokemonList[_pokemonList.Count - 1].transform.position + (new Vector2(_hGap, _vGap));
            _scoreCount++;
            setScore();
            _pokemonList.Add(pokemon);

            Destroy(collision.gameObject);
            GameObject pokeball = GameObject.Instantiate(pokeballPrefabToSpawn) as GameObject;

        }
        else if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("border"))
        {
            if (_lives == 0)
            {
                SceneManager.LoadScene(startSceneName);
                PlayerPrefs.SetInt(_scoreKey, 0);
                PlayerPrefs.SetInt(_livesKey, 3);
            }
            else
            {
                _lives--;
                PlayerPrefs.SetInt(_livesKey, _lives);
                PlayerPrefs.SetInt(_scoreKey, _scoreCount);
                SceneManager.LoadScene(sceneName);
            }    
        }
    }

    ///<summary>
    ///Getter for the Player's list of directions changed
    ///</summary>
    ///<return>The Player's list of directions changed</return>
    public List<Vector2> getDirectionChangedList()
    {
        return _directionChangeList;
    }

    ///<summary>
    ///Getter for the Player's list of positions a direction changed at
    ///</summary>
    ///<return>The Player's list of positions a direction changed at</return>
    public List<Vector2> getPositionChangedList()
    {
        return _positionChangeList;
    }

    ///<summary>
    ///Getter for the amount of Pokemon in the "Snake"
    ///</summary>
    ///<return>The length of the list of Pokemon GameObjects</return>
    public int getCount()
    {
        return _pokemonList.Count;
    }

    ///<summary>
    ///Decrements the counter of Direction Changes by 1
    ///</summary>
    public void decrementDirectionCount()
    {
        _directionCount--;
    }

    ///<summary>
    ///Getter for the Player's list of Pokemon
    ///</summary>
    ///<return>The Player's list of Pokemon</return>
    public List<GameObject> getPokemonList()
    {
        return _pokemonList;
    }

    ///<summary>
    ///Setter for the Player's current direction
    ///</summary>
    ///<param name="dir">The direction to change to</return>
    public void setDirection(Vector2 dir)
    {
        this._direction = dir;
    }

    ///<summary>
    ///Setter for the Player's score
    ///</summary>
    void setScore()
    {
        score.text = "Score: " + _scoreCount;
    }
}
