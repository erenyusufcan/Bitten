using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource machineGunShootAS;
    public AudioSource shotGunShootAS;
    public AudioSource zombieImpactAS;
    public AudioSource zombieSoundEffectAS;
    public AudioSource WarningTimeSoundAS;
    public AudioSource collectable;
    public AudioSource grenadeExplosionAS;
    public AudioSource weaponChange; 
    public AudioSource spellCastAS;
    public AudioSource spellImapctAS;
    public AudioSource reloadAS;
    public List<AudioSource> gameMusics;
    public List<AudioSource> levelWinAS;
    public List<AudioSource> levelLoseAS;    
    public List<AudioSource> dashAS;

    private void Awake()
    {
        Instance = this;    
    }
    public void PlayReloadAS()
    {
        reloadAS.Play();
    }
    public void PlaySpellCastAS()
    {
        spellCastAS.Play();
    }
    public void PlaySpellImpactAS()
    {
        spellImapctAS.Play();
    }
    public void PlayMachineGunShootAS()
    {
        machineGunShootAS.Play();
    }
    public void PlayShotGunShootAS()
    {
        shotGunShootAS.Play();
    }
    public void PlayWarningTimeSoundAS()
    {
        WarningTimeSoundAS.Play();
    }   
    public void PlayCollectableAS()
    {
        collectable.Play();
    }
    public void PlayImpactAS()
    {
        zombieImpactAS.Play();
    }
    public void PlayZombieSoundEffectAS()
    {
        zombieSoundEffectAS.Play();
    }
    public void PlayChangeWeaponAS()
    {
        weaponChange.Play();
    }
    public void PlayLevelWinAS()
    {
        StartCoroutine(PlayLevelWinCoroutine());
    }   
    IEnumerator PlayLevelWinCoroutine()
    {
        int randomIndex = Random.Range(0, levelWinAS.Count);
        levelWinAS[randomIndex].Play();
        yield return new WaitForSeconds(4f);
        StopLevelWinAS();   
    }  
    public void StopLevelWinAS()
    {
        foreach (var AS in levelWinAS)
        {
            AS.Stop();
        }
    }
    public void PlayLevelLoseAS()
    {
        int randomIndex = Random.Range(0, levelLoseAS.Count);
        levelLoseAS[randomIndex].Play();
    }
    public void StopLevelLoseAS()
    {
        foreach (var AS in levelLoseAS)
        {
            AS.Stop();
        }
    }   
    public void PlayDashAS()
    {
        int rand = Random.Range(0, dashAS.Count);   
        dashAS[rand].Play();
    }

    public void PlayGameMusic()
    {
        int randomIndex = Random.Range(0, gameMusics.Count);
        gameMusics[randomIndex].Play();
    }
    public void StopGameMusic()
    {
        foreach (var music in gameMusics)
        {
            music.Stop();
        }
    }
    public void PlayGrenadeExplosionAS()
    {
        grenadeExplosionAS.Play();
    }

   
}
