using UnityEngine;
using System.Collections;

public class RespawnHero : MonoBehaviour
{
    public GameObject player;
    Movement heroMovement;
    private float respawnTime;


    private void Start()
    {
        heroMovement = player.GetComponent<Movement>();
    }

    public void RespawnPlayer(float currentLvl)
    {
        respawnTime = currentLvl * 4;
        player.SetActive(false);
        player.transform.position = new Vector3(-42.67f, 2.5f, -44.69f);
        StartCoroutine(RespawnTimer(respawnTime));
    }

    IEnumerator RespawnTimer(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        player.SetActive(true);
        heroMovement.agent.SetDestination(new Vector3(-42.67f, 2.5f, -44.69f));
    }
}
