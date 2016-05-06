using UnityEngine;
using System.Collections;

public class leftGoalKeeperScript : MonoBehaviour {

    public float speed = 3;
    public bool goUp = true;
    Vector3 up = new Vector3(-7, 2);
    Vector3 down = new Vector3(-7, -2);
    // Use this for initialization
    void Start()
    {
        //set up of goal:

    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    void Chase()
    {
        float step = speed * Time.deltaTime;
        if (goUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, up, step);
            if (transform.position == up)
                goUp = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, down, step);
            if (transform.position == down)
                goUp = true;
        }
    }
}
