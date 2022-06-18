using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class KnightStatueAnim : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking;
    public string currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = "Idle";
        SetKnightState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void SetKnightState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(idle, true, 1f);
        }
    }


}
