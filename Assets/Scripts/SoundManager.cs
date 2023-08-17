using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //BGM ������
    public enum EBgm
    {
        BGM_CONNECTION,
        BGM_LOBBY,
        BGM_GAME
    }

    //SFX ������
    public enum ESfx
    {
        SFX_BUTTON,
        SFX_JUMP,
    }

    //bgm audio clip ���� �� �ִ� �迭
    public AudioClip[] bgms;

    public AudioClip[] sfxs;

    //bgm �÷����ϴ� audioSource
    public AudioSource audioBgm;

    //sfx �÷����ϴ� audioSource
    public AudioSource audioSfx;

    //���� ���� static ����
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //BGM player
    public void PlayBGM(EBgm bgmIdx)
    {
        //�÷����� bgm ����
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }

    public void PlaySFX(ESfx sfxIdx)
    {
        //�÷��� �� sfx ����
        audioSfx.PlayOneShot(sfxs[(int)sfxIdx]);
    }
}