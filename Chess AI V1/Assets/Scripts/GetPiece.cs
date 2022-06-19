using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPiece : MonoBehaviour
{
    [SerializeField]
    GameObject piece;
    // Start is called before the first frame update
    void Start()
    {
        piece = null;
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //}

        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        piece = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        piece = null;
    }
    
    public GameObject Piece()
    {
        return piece;
    }


}
