using System.Collections.Generic;
using UnityEngine;

namespace CharacterViewElements
{
    public class EffectsController
    {
        private Transform _transform;
        private Dictionary<EffectInfo, GameObject> _appliedEffects 
            = new Dictionary<EffectInfo, GameObject>();

        public EffectsController(Transform transform)
        { 
            _transform = transform;
        }

        public void TryAddEffect(EffectInfo info)
        {
            if (_appliedEffects.ContainsKey(info))
            {
                return;
            }

            if (info.Smoke == null)
            {
                return;
            }

            GameObject effect = GameObject.Instantiate(info.Smoke, _transform);
            _appliedEffects.Add(info, effect);
        }

        public void TryRemoveEffect(EffectInfo info)
        {
            if (_appliedEffects.ContainsKey(info))
            {
                GameObject.Destroy(_appliedEffects[info]);
                _appliedEffects.Remove(info);
            }
        }

        public void RemoveAllEffects()
        {
            foreach ((EffectInfo info, GameObject effect) in _appliedEffects)
            {
                GameObject.Destroy(_appliedEffects[info]);
            }

            _appliedEffects.Clear();
        }
    }
}