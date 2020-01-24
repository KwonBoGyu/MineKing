using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSoundMng : MonoBehaviour
{
    public AudioSource[] _as;
    public AudioClip[] _clips;

    public void playRunningEffect()
    {
        if (_as[2].isPlaying.Equals(true))
            return;
        if (_as[2].clip != _clips[0])
            _as[2].clip = _clips[0];

        _as[2].volume = 0.3f;
        _as[2].pitch = 0.95f;
        _as[2].loop = false;
        _as[2].Play();
    }
    public void StopRunningEffect()
    {
        _as[2].Stop();
    }

    public void playAxeEffect()
    {
        
        if (_as[3].clip != _clips[2])
            _as[3].clip = _clips[2];

        _as[3].volume = 1.0f;
        
        _as[3].loop = false;
        _as[3].Play();
    }

    public void playEffect(int pNum)
    {
        _as[1].Stop();
           
        switch (pNum)
        {
            case 0:
                _as[1].loop = false;
                break;
        }

        if (_as[1].clip != _clips[pNum])
            _as[1].clip = _clips[pNum];

        _as[1].Play();
    }

    public void StopEffect()
    {
        _as[1].Stop();
    }
    
}
