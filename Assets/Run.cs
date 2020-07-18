using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Run : MonoBehaviour
{
    /*
    These rules, which compare the behavior of the automaton to real life, can be condensed into the following:
    Any live cell with two or three live neighbours survives.
    Any dead cell with three live neighbours becomes a live cell.
    All other live cells die in the next generation. Similarly, all other dead cells stay dead.
    */
    
    public Material aliveMaterial;
    public Material deadMaterial;
    public GameObject cell;
    
    public int numberOfColumns = 100;
    public int numberOfRows = 100;
    public float spaceBetweenCells = 0.8f;
    public float timeToEvolveInSeconds = 0.10f;
    
    private List<GameObject> _cubes = new List<GameObject>();
    private GameObject[,] _world;
    
    // Start is called before the first frame update
    // It initializes the world by generating cells from the "cell" prefab
    // We also generate an initial population of alive cells to get this thing started 
    void Start()
    {
        // World Array 
        _world = new GameObject[numberOfColumns, numberOfRows];

        // Put cells into work
        for (int co = 0; co < numberOfColumns; co++)
        {
            for (int li = 0; li < numberOfRows; li++)
            {
                _world[co, li] = Instantiate(cell);
                _world[co, li].transform.position = new Vector3((float) (co + (co * spaceBetweenCells)), 0, (float) (li + (li * spaceBetweenCells)));
                
                // Random chance of life...
                if (Random.value > 0.33f)
                {
                    _world[co, li].GetComponent<Renderer>().material = aliveMaterial;
                    _world[co, li].GetComponent<Cell>().isAlive = true;
                }
            }
        }
        
        // Let the cells do their thing
        InvokeRepeating(nameof(Evolve), 0, timeToEvolveInSeconds);
    }

    private void Evolve()
    {
        bool[,] newState = new bool[numberOfColumns, numberOfRows];

        // Calculate the cell moves into a new world state
        for (int co = 0; co < numberOfColumns; co++)
        {
            for (int li = 0; li < numberOfRows; li++)
            {
                if (_world[co, li].GetComponent<Cell>().isAlive)
                {
                    newState[co, li] = NumberOfAliveNeighbours(co, li) == 2 || NumberOfAliveNeighbours(co, li) == 3;
                } 
                else 
                {
                    newState[co, li] = NumberOfAliveNeighbours(co, li) == 3;
                }
            }
        }

        // Apply the new world state to the world
        for (int co = 0; co < numberOfColumns; co++)
        {
            for (int li = 0; li < numberOfRows; li++)
            {
                if (_world[co, li].GetComponent<Cell>().isAlive != newState[co, li]) {
                    // If the state of the cell changed, then we perform an update
                    _world[co, li].GetComponent<Renderer>().material = newState[co, li] ? aliveMaterial : deadMaterial;
                    _world[co, li].GetComponent<Cell>().isAlive = newState[co, li];
                }
            }
        }
    }

    // Calculate the number of alive neighbours to determine what happens to the cell next
    // According to the rules, 2 or 3 neighbours means staying alive. And having exactly 3 neighbours brings a dead cell back to life
    int NumberOfAliveNeighbours(int co, int li)
    {
        int count = 0;

        for (int coScanner = -1; coScanner <= 1; coScanner++)
        {
            for (int liScanner = -1; liScanner <= 1; liScanner++)
            {
                int calculatedCo = co - coScanner;
                int calculatedLi = li - liScanner;
                if (calculatedCo >= 0 && calculatedCo < numberOfColumns && calculatedLi >= 0 && calculatedLi < numberOfRows)
                {
                    if (co != calculatedCo || li != calculatedLi) {
                        if (_world[calculatedCo, calculatedLi].GetComponent<Cell>().isAlive)
                        {
                            count++;
                        }
                    }
                }
            }
        }

        return count;
    }
}
