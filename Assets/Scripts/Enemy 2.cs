using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     transform.Translate(new Vector3(2, -3, 0) * Time.deltaTime * 2f);
        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }    
    }
}
