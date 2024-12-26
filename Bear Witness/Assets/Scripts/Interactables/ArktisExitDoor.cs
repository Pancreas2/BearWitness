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

    [SerializeField] private Item nullTool;

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
            int index = -1;
            index = GameManager.instance.tools.IndexOf(icePick);
            if (index > -1) GameManager.instance.tools[index] = nullTool;

            index = -1;
            index = GameManager.instance.currentItems.IndexOf(icePick);
            if (index > -1) GameManager.instance.currentItems[index] = nullTool;

            GameUI_Controller.instance.Reload();
        }
        GameManager.instance.hourglassFill = GameManager.instance.hourglassCapacity;
        FindObjectOfType<LevelLoader>().LoadNextLevel(base.destination);
    }
}

