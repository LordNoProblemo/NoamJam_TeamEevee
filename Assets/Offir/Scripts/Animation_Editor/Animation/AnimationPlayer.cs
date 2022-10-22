using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationPlayer : MonoBehaviour
{
    [Header("Settings:")]
    [SerializeField] private bool _isLooping = true;
    [SerializeField] private bool _playOnAwake = true;
    [SerializeField] private string _triggerTag = "Player";
    [Space(10)]
    [TextArea] [SerializeField] private string Explanation = "Add an animation clip to the animation component." +
                                                             "\n * Animation can be played in the following ways:" +
                                                             "\n * Automatically with the play on awake option." +
                                                             "\n * By any trigger you add to this object using the trigger tag." +
                                                             "\n * through a trigger event from another script." +
                                                             "\n \n Note: do not change the Animation component options."; 

    private Animation _animation;

    private void Reset()
    {
        GetAnimationComponentIfNeeded();
    }

    private void Awake()
    {
        Init();
        if (_playOnAwake)
        {
            PlayAnimation();
        }
    }
    private void Init()
    {
        GetAnimationComponentIfNeeded();
        var clip = _animation.clip;
        if (clip == null)
        {
            return;
        }
        
        clip.legacy = true;
        clip.wrapMode = _isLooping ? WrapMode.Loop : WrapMode.Default;
    }
    
    private void GetAnimationComponentIfNeeded()
    {
        _animation ??= GetComponent<Animation>();
        _animation.playAutomatically = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_triggerTag))
        {
            PlayAnimation();
        }
    }
    
    //Public to be called from unity events in the inspector
    public void PlayAnimation()
    {
        if (_animation.clip == null)
        {
            Debug.LogError("AnimationPlayer PlayAnimation Error: No clip available in the animation component");
            return;
        }
        
        _animation.Play();
    }
}
