﻿using DarkBestiary.Messaging;
using UnityEngine;

namespace DarkBestiary.Rewards
{
    public abstract class Reward
    {
        public event Payload<Reward> Prepared;
        public event Payload<Reward> Claimed;

        public void Prepare(GameObject entity)
        {
            OnPrepare(entity);
            Prepared?.Invoke(this);
        }

        public void Claim(GameObject entity)
        {
            OnClaim(entity);
            Claimed?.Invoke(this);
        }

        protected abstract void OnPrepare(GameObject entity);

        protected abstract void OnClaim(GameObject entity);
    }
}