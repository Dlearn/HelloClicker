using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.   
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.

    // Music
    public AudioClip AmbientMusic, FightMusic;
    public AudioClip WinMusic, LoseMusic;

    // Sound Effects
    public AudioClip Mace1, Mace2, Slash2, Slash3, Slash4;
    public AudioClip blockSfx;
    public const float MACELOW = 1.4f, MACEHIGH = 1.9f;
    public const float SLASHLOW = 1.9f, SLASHHIGH = 2.4f;
    public AudioClip RedKnightAtk1, RedKnightAtk2;
    public const float REDKNIGHTLOW = 2.5f, REDKNIGHTHIGH = 3.5f;
    public AudioClip SmailAtk1, SmailAtk2, SmailAtk3, SmailAtk4, SmailAtk5;
    public const float SMAILLOW = 18.0f, SMAILHIGH = 20.0f;
    public AudioClip LianHwaAtk1, LianHwaAtk2, LianHwaAtk3, LianHwaAtk4, LianHwaAtk5;
    public const float LIANHWALOW = 7.2f, LIANHWAHIGH = 7.7f;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAmbientMusic()
    {
        if (musicSource.clip == AmbientMusic) return;
        musicSource.Stop();
        musicSource.clip = AmbientMusic;
        musicSource.Play();
    }

    public void PlayFightMusic()
    {
        musicSource.Stop();
        musicSource.clip = FightMusic;
        musicSource.Play();
    }

    public void PlayWinMusic()
    {
        musicSource.Stop();
        musicSource.clip = WinMusic;
        musicSource.Play();
    }

    public void PlayLoseMusic()
    {
        musicSource.Stop();
        musicSource.clip = LoseMusic;
        musicSource.Play();
    }

    public void MaceAttack()
    {
        int randSoundClip = Random.Range(0, 2);
        float volScale = Random.Range(MACELOW, MACEHIGH);
        switch (randSoundClip)
        {
            case 0:
                efxSource.PlayOneShot(Mace1, volScale);
                break;
            case 1:
                efxSource.PlayOneShot(Mace2, volScale);
                break;
        }
    }

    public void SwordAttack()
    {
        int randSoundClip = Random.Range(0, 3);
        float volScale = Random.Range(SLASHLOW, SLASHHIGH);
        switch (randSoundClip)
        {
            case 0:
                efxSource.PlayOneShot(Slash2, volScale);
                break;
            case 1:
                efxSource.PlayOneShot(Slash3, volScale);
                break;
            case 2:
                efxSource.PlayOneShot(Slash4, volScale);
                break;
        }
    }

    public void Block()
    {
        efxSource.PlayOneShot(blockSfx);
    }

    public void RedKnightAttack()
    {
        int randSoundClip = Random.Range(0, 2);
        float volScale = Random.Range(REDKNIGHTLOW, REDKNIGHTHIGH);
        switch (randSoundClip)
        {
            case 0:
                efxSource.PlayOneShot(RedKnightAtk1, volScale);
                break;
            case 1:
                efxSource.PlayOneShot(RedKnightAtk2, volScale);
                break;
        }
    }

    public void SmailAttack()
    {
        int randSoundClip = Random.Range(0, 5);
        float volScale = Random.Range(SMAILLOW, SMAILHIGH);
        switch (randSoundClip)
        {
            case 0:
                efxSource.PlayOneShot(SmailAtk1, volScale);
                break;
            case 1:
                efxSource.PlayOneShot(SmailAtk2, volScale);
                break;
            case 2:
                efxSource.PlayOneShot(SmailAtk3, volScale);
                break;
            case 3:
                efxSource.PlayOneShot(SmailAtk4, volScale);
                break;
            case 4:
                efxSource.PlayOneShot(SmailAtk5, volScale);
                break;
        }
    }

    public void LianHwaAttack()
    {
        int randSoundClip = Random.Range(0, 5);
        float volScale = Random.Range(LIANHWALOW, LIANHWAHIGH);
        switch (randSoundClip)
        {
            case 0:
                efxSource.PlayOneShot(LianHwaAtk1, volScale);
                break;
            case 1:
                efxSource.PlayOneShot(LianHwaAtk2, volScale);
                break;
            case 2:
                efxSource.PlayOneShot(LianHwaAtk3, volScale);
                break;
            case 3:
                efxSource.PlayOneShot(LianHwaAtk4, volScale);
                break;
            case 4:
                efxSource.PlayOneShot(LianHwaAtk5, volScale);
                break;
        }
    }
}