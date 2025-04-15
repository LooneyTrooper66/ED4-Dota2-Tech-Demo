using UnityEngine;

public class RespawnHero : MonoBehaviour
{
    public GameObject player;
    Movement heroMovement;


    private void Start()
    {
        heroMovement = player.GetComponent<Movement>();
    }

    public void RespawnPlayer()
    {
        player.SetActive(false);
        player.transform.position = new Vector3(-42.67f, 2.5f, -44.69f);
        player.SetActive(true);
        heroMovement.agent.SetDestination(new Vector3(-42.67f, 2.5f, -44.69f));
    }
}
