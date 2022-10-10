using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectable : MonoBehaviour, IEffectable
{
    private StatusEffectData _data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_data != null){
            HandleEffect();
        }
        
    }

    private GameObject _effectParticles;
    public void ApplyEffect(StatusEffectData _data){
        RemoveEffect();
        this._data = _data;
        _effectParticles =  Instantiate(_data.effectParticles, transform);
    }

    
    private float _currentEffectTime = 0f;
    private float _nextTickTime = 0f;

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
            // currentHealth += _data.healthOverTime

        } 

    }

}
