using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ArktisExitDoor : Door
{
    public Item icePick;
    public Image crossfade;

    [SerializeField] private Animator cutsceneAnimator;

    [SerializeField] private List<Sprite> runes;
    [SerializeField] private Image runeDisplay;

    [SerializeField] private Item nullTool;

    [SerializeField] private CutsceneTrigger cutscene;

    public override void ChangeRooms()
    {
        int num = GameManager.instance.loopNumber % 9;
        runeDisplay.sprite = runes[num];
        FindObjectOfType<PlayerMovement>().frozen = true;
        cutscene.TriggerCutscene();
        //cutsceneAnimator.SetTrigger("Start");
        StartCoroutine(StartRun());
    }

    IEnumerator StartRun()
    {
        GameUI_Controller.instance.HideAll();
        yield return new WaitForSeconds(4f);
        GameManager.instance.StartRun();

        GameManager.instance.hourglassFill = GameManager.instance.hourglassCapacity;
        FindObjectOfType<LevelLoader>().LoadNextLevel(base.destination);
    }
}

