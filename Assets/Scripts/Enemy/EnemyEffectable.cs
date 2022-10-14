using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectable : MonoBehaviour, IEffectable
{
    EnemyManager enemyManager;
    IHealthComponent healthComponent;
    private StatusEffectData _data;

    private GameObject _effectParticles;

    private float _currentEffectTime = 0f;
    private float _nextTickTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        healthComponent = GetComponent<IHealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_data != null) HandleEffect(); 
    }

    public void ApplyEffect(StatusEffectData _data){
        RemoveEffect();
        this._data = _data;
        _effectParticles =  Instantiate(_data.effectParticles, transform);
    }

    public void RemoveEffect(){
        _data = null;
        _currentEffectTime = 0;
        _nextTickTime = 0; 
        if(_effectParticles != null) Destroy(_effectParticles);
    }

    public void HandleEffect(){
        _currentEffectTime += Time.deltaTime;
        if(_currentEffectTime >= _data.lifetime) RemoveEffect();
        
        if(_data == null) return;
        if(_data.healthOverTime != 0 && _currentEffectTime > _nextTickTime + _data.tickSpeed){
            _nextTickTime += _data.tickSpeed; 
            healthComponent.AlterHealth(_data.healthOverTime);
        } 

    }

}
