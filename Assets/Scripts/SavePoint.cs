using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // Start is called before the first frame update
    Animator Animator;
    void Start()
    {
        Animator = GetComponent<Animator>();
        var setting = AnimationUtility.GetAnimationClipSettings(FindAnimation(Animator, "heal"));
        setting.loopTime = false;
        AnimationUtility.SetAnimationClipSettings(FindAnimation(Animator, "heal"), setting);
        Animator.Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>().ChangeSavePoint(gameObject.transform);
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>().heal(true,0);// heal full
            Animator.Play("heal");
        }
        
    }
    public AnimationClip FindAnimation(Animator animator, string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
}
