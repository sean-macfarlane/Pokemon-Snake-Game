using UnityEngine;
using System.Collections;

///<summary>
///The Controller for Spawning the Pokeball
///</summary>
public class SpawnPokeball : MonoBehaviour
{
    public Transform borderTop;     //The Top Border
    public Transform borderBottom;  //The Bottom Border
    public Transform borderLeft;    //The Left Border
    public Transform borderRight;   //The Right Border

    // Use this for initialization
    void Start()
    {
        float x = Random.Range(borderLeft.position.x + 1, borderRight.position.x - 1);
        float y = Random.Range(borderBottom.position.y + 1, borderTop.position.y - 1);
        transform.position = new Vector2(x, y);
        name = "pokeball";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
