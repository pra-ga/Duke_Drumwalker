using UnityEngine;

public class FireTileManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    public ParticleSystem fireParticle;
    PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireParticle.Play();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

  /*   void OnParticleCollision(GameObject other)
    {
        Debug.Log("playerDieByFire: " + playerMovement.playerDieByFire);
        if (other.gameObject.tag == "Player" && fireParticle.isEmitting)
        {
            playerMovement.playerDieByFire = true;
        }
    } */

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && fireParticle.isEmitting)
        {
            playerMovement.State = PlayerMovement.States.Die;
            //playerMovement.CheckDie();
            //playerMovement.playerDieByFire = true;

            Debug.Log("playerDieByFire in FireTileManager: " + playerMovement.playerDieByFire + "playerMovement.State: " + playerMovement.State);
        }
    }

}
