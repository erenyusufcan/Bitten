using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    public CameraFollow cameraFollow;
    public ParticleSystem ImpactParticle;
    public ParticleSystem ZombieImpactParticle;
    public ParticleSystem PotionParticle;
    public ParticleSystem ZombieDeath;
    public ParticleSystem GrenadeExplosionParticle;
    public ParticleSystem EnemySpellImpactParticle;
    public DamageText damageText;

    private void Awake()
    {
        Instance = this;    
    }

    public void PlayImpactParticle(Vector3 pos)
    {
        var ýmpact = Instantiate(ImpactParticle);
        ýmpact.transform.position = pos;    
        ýmpact.Play();  
    }
    public void PlayEnemySpellImpact(Vector3 pos)
    {
        var ýmpact = Instantiate(EnemySpellImpactParticle);
        ýmpact.transform.position = pos;
        ýmpact.Play();
    }
    public void PlayZombieImpactParticle(Vector3 pos,Vector3 dir)
    {
        var ýmpact = Instantiate(ZombieImpactParticle);
        ýmpact.transform.position = pos;
        ýmpact.transform.LookAt(pos + dir);
        ýmpact.Play();  
        Destroy(ýmpact.gameObject, 1f); 
    }
    public void PlayPotionParticle(Vector3 pos)
    {
        var ýmpact = Instantiate(PotionParticle);
        ýmpact.transform.position = pos + Vector3.up;
        ýmpact.Play();
    }
    public void PlayZombieDeath(Vector3 pos)
    {
        var ýmpact = Instantiate(ZombieDeath);
        ýmpact.transform.position = pos;
        ýmpact.Play();
        Destroy(ýmpact.gameObject, 1f);
    }
    public void PlayDamageText(Vector3 pos,int damage)
    {
        var text = Instantiate(damageText);
        text.transform.position = pos + (Vector3.up *.2f);
        text.SetDamageText(damage);
        Destroy(text.gameObject,.5f);    
    }
    public void PlayGrenadeExplosion(Vector3 pos)
    {
        var ýmpact = Instantiate(GrenadeExplosionParticle);
        ýmpact.transform.position = pos;
        ýmpact.Play();
        Destroy(ýmpact.gameObject, 1.5f);
        cameraFollow.ShakeCamera(.5f,1f);
    }   


}
