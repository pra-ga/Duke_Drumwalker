using UnityEngine;

public class FireTileManager : MonoBehaviour
{
    GameObject player;
    public ParticleSystem fireParticle;
    PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireParticle.Play();
        //playerMovement = player.GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && fireParticle.isEmitting )
        {
            player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerMovement>().playerDieByFire = true;
        }
    }

}
