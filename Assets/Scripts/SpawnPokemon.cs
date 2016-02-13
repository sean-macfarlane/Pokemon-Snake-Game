using UnityEngine;
using System.Collections.Generic;

///<summary>
///The Controller for changing a Pokemon's direction
///</summary>
public class SpawnPokemon : MonoBehaviour
{
    int _id;    //The ID of the Pokemon
    Vector2 _direction; //The current direction of the Pokemon
    GameObject _player; //Reference to the Player GameObject
    List<Vector2> _directionChangeList = new List<Vector2>();   //List to store all the directions the Pokemon has to change to
    List<Vector2> _positionChangeList = new List<Vector2>();    //List to store all the positions the Pokemon has to change direction at
    float _epsilon = 0.01f;     //Epsilon for when calculating the pokemon is at the same position
    public Sprite[] sprites;    //All the sprites for that pokemon

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_directionChangeList.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, _positionChangeList[0]);
            if (distance <= _epsilon)
            {
                _direction = _directionChangeList[0];
                if (_player.GetComponent<PlayerController>().getCount() - 1 == _id)
                {
                    _player.GetComponent<PlayerController>().getPositionChangedList().RemoveAt(0);
                    _player.GetComponent<PlayerController>().getDirectionChangedList().RemoveAt(0);
                    _player.GetComponent<PlayerController>().decrementDirectionCount();
                }
                else
                {
                    _player.GetComponent<PlayerController>().getPokemonList()[_id + 1].GetComponent<SpawnPokemon>().addNewPosition(_positionChangeList[0]);
                    _player.GetComponent<PlayerController>().getPokemonList()[_id + 1].GetComponent<SpawnPokemon>().addNewDirection(_directionChangeList[0]);
                }
                _directionChangeList.RemoveAt(0);
                _positionChangeList.RemoveAt(0);
            }
        }

        if (_direction == Vector2.right)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else if (_direction == Vector2.down)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else if (_direction == Vector2.left)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        else if (_direction == Vector2.up)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }
        transform.Translate(_direction * _player.GetComponent<PlayerController>().speed);
    }

    ///<summary>
    ///Setter for ID
    ///</summary>
    ///<param name=”id”>The id to set the Pokemon GameObject</param>
    public void setID(int id)
    {
        this._id = id;
    }

    ///<summary>
    ///Setter for direction
    ///</summary>
    ///<param name=”dir”>The direction to set the Pokemon GameObject</param>
    public void setDirection(Vector2 dir)
    {
        this._direction = dir;
    }

    ///<summary>
    ///Setter for player
    ///</summary>
    ///<param name=”player”>The player to set the Pokemon GameObject</param>
    public void setPlayer(GameObject player)
    {
        this._player = player;
    }

    ///<summary>
    ///To add a new direction to the List of directions that pokemon has to change
    ///</summary>
    ///<param name=”dir”>The direction the pokemon has to change to</param>
    public void addNewDirection(Vector2 dir)
    {
        _directionChangeList.Add(dir);
    }

    ///<summary>
    ///To add a new position to the List of postions that pokemon has to change direction at.
    ///</summary>
    ///<param name=”pos”>The position the pokemon has to change direction at.</param>
    public void addNewPosition(Vector2 pos)
    {
        _positionChangeList.Add(pos);
    }

    ///<summary>
    ///Getter for the Pokemon's direction
    ///</summary>
    ///<return>The pokemon's current direction</return>
    public Vector2 getDirection()
    {
        return _direction;
    }

    ///<summary>
    ///To change the Pokemon's Sprite when it changes direction.
    ///</summary>
    ///<param name=”num”>The number of the sprite to load</param>
    public void setSprites(int num)
    {
        sprites = Resources.LoadAll<Sprite>("pokemon/" + num);
    }
}
