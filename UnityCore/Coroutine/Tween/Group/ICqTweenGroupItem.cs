using System;
using UnityEngine;
using UnityCore;

public interface ICqTweenGroupItem
{
    TweenMode Mode {  set; }
    bool PlayAndDo(Action OnComplete = null);
    void Play();
    void Stop();
    void Immediately();
    void SetCurrentByStart();

    void SetCurrentByEnd();

    void SetStart();
    void SetEnd();
    void SetCurrentStartOrEnd(bool isStart);

    bool isPlaying { get; }
}