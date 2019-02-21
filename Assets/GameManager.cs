using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

    public CharacterMovement player;
    public PlayerHp playerHP;
    public CameraShake cameraShake;
    public CameraFollow cameraFollow;
    public GameObject currentActiveCheckPoint;
    public Animator blackFadeAnim;

    public void RestartScene()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    /*public void ResetFromCheckPoint()
    {
        player.transform.position = currentActiveCheckPoint.transform.position;
        player.transform.rotation = currentActiveCheckPoint.transform.rotation;
        playerHP.currentHP = playerHP.maxHP;
        cameraFollow.ResetFuckedUpCamera();
    }*/

    void SetPlayersPosition(Vector3 pos, Quaternion rot)
    {
        player.transform.position = pos;
        player.transform.rotation = rot;
        cameraFollow.ResetFuckedUpCamera();
    }

    void ResetPlayersHP()
    {
        playerHP.currentHP = playerHP.maxHP;
    }

    public IEnumerator ResetFromCheckPoint()
    {
        blackFadeAnim.SetTrigger("fadeIn");
        player.takingJumpInput = false;
        player.takingMoveInput = false;
        yield return new WaitForSeconds(0.5f);
        SetPlayersPosition(currentActiveCheckPoint.transform.position, currentActiveCheckPoint.transform.rotation);
        ResetPlayersHP();
        //player.StopPlayersMovement();
        player.RB.isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(playerHP.SetInvincibleForTime(playerHP.invincibilityTime));
        player.RB.isKinematic = false;
        blackFadeAnim.SetTrigger("fadeOut");
        player.takingJumpInput = true;
        player.takingMoveInput = true;
    }

}