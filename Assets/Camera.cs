using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject game;

    private float _rotateAroundX;
    private float _rotateAroundZ;
    
    void Start()
    {
        _rotateAroundX = game.GetComponent<Run>().numberOfRows / 2.0f + ((game.GetComponent<Run>().numberOfRows * game.GetComponent<Run>().spaceBetweenCells) / 2);
        _rotateAroundZ = game.GetComponent<Run>().numberOfColumns / 2.0f + ((game.GetComponent<Run>().numberOfColumns * game.GetComponent<Run>().spaceBetweenCells) / 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround (new Vector3(_rotateAroundX, 0 , _rotateAroundZ), Vector3.up, 10.0f * Time.deltaTime);
    }
}
