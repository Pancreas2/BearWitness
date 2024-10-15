using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArktisExitDoor : Door
{
    public Item icePick;
    public Image crossfade;

    [SerializeField] private Animator cutsceneAnimator;

    [SerializeField] private List<Sprite> runes;
    [SerializeField] private Image runeDisplay;

    public override void ChangeRooms()
    {
        int num = GameManager.instance.loopNumber % 9;
        runeDisplay.sprite = runes[num];
        FindObjectOfType<PlayerMovement>().frozen = true;
        cutsceneAnimator.SetTrigger("Start");
        StartCoroutine(StartRun());
    }

    IEnumerator StartRun()
    {
        GameUI_Controller.instance.HideAll();
        yield return new WaitForSeconds(8.5f);
        GameManager.instance.StartRun();
        if (GameManager.instance.loopNumber == 0)
        {
            GameManager.instance.tools.Remove(icePick);
        }
        GameManager.instance.hourglassFill = 900f;
        FindObjectOfType<LevelLoader>().LoadNextLevel(base.destination);
    }
}
