using AxGrid.Base;
using AxGrid.Model;
using LootBox.Models;
using UnityEngine;

namespace LootBox.Systems
{
    public class ParticleSystemController : MonoBehaviourExtBind
    {
        [SerializeField] private ParticleSystem _particleFX;

        [Bind(LootBoxSignals.ViewSpinStopped)]
        private void OnSpinStopped()
        {
            if (_particleFX == null)
                return;

            _particleFX.Play();
        }
    }
}