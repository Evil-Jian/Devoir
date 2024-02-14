using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Gun : MonoBehaviour
{
    public GameObject bullet;
    public float launchForce;
    public Transform shotPoint;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 direction = new Vector2(0f, 0f);
        transform.right = direction;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot()
    {
        GameObject newMushBullet = Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        newMushBullet.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
    }
}
