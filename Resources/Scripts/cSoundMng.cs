using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSoundMng : MonoBehaviour
{
    //0:일반, 1:이동, 2:공격, 3:타일, 4: 타일, 5:아이템, 6:가방, 7:BGM
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

    public void playAxeEffect(bool isCharge)
    {
        //일반공격
        if(isCharge.Equals(false))
        {
            if (_as[3].clip != _clips[2])
                _as[3].clip = _clips[2];
        }
        //강화공격
        else
        {
            if (_as[3].clip != _clips[3])
                _as[3].clip = _clips[3];
        }
        
        _as[3].volume = 1.0f;
        
        _as[3].loop = false;
        _as[3].Play();
    }

    public void playTileEffect()
    {
        if (_as[4].clip != _clips[4])
            _as[4].clip = _clips[4];

        _as[4].volume = 0.5f;
        _as[4].pitch = 1.2f;
        _as[4].loop = false;
        _as[4].Play();
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
    
    public void PlayItemEffect(byte pNum)
    {
        _as[5].Stop();
        switch (pNum)
        {
            case 1:
                _as[5].volume = 0.7f;
                _as[5].clip = _clips[5];
                _as[5].loop = false;
                _as[5].Play();
                break;

            case 2:
                _as[5].volume = 1.0f;
                _as[5].clip = _clips[6];
                _as[5].loop = false;
                _as[5].Play();
                break;
        }
    }

    public void PlayBag(bool isOn)
    {
        _as[6].Stop();

        if (isOn.Equals(true))
        {
            _as[6].clip = _clips[7];
            _as[6].loop = false;
            _as[6].Play();
        }
        else
        {
            _as[6].clip = _clips[8];
            _as[6].loop = false;
            _as[6].Play();
        }
    }
}
