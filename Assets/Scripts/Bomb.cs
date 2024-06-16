using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.localScale = Vector2.one;
        animator.gameObject.SetActive(false);
    }
}
