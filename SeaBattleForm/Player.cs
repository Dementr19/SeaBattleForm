using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleForm
{
    public abstract class Player
    {
        protected readonly Dictionary<Point, ShotResult> PastShots;
        private bool _canShot;

        protected Player(string name)
        {
            Name = name;
            PastShots = new Dictionary<Point, ShotResult>();
        }

        public string Name { get; set; }

        public virtual void Shoot()
        {
            _canShot = true;
            var handler = MyTurn;
            if (handler != null)
                handler(this, new EventArgs());
        }

        public virtual void Reset()
        {
            PastShots.Clear();
            _canShot = false;
        }

        protected void ShotTargetChosen(int x, int y)
        {
            if (!_canShot)
                return;

            _canShot = false;

            var shooting = Shooting;
            if (shooting == null)
                return;

            var eventArgs = new ShootingEventArgs(x, y);
            shooting(this, eventArgs);
            AddShotResult(x, y, eventArgs.Result);

            var shot = Shot;
            if (shot != null)
                shot(this, eventArgs);

        }

        protected virtual void AddShotResult(int x, int y, ShotResult result)
        {
            PastShots[new Point(x, y)] = result;
        }

        public event EventHandler<ShootingEventArgs> Shooting;
        public event EventHandler<ShootingEventArgs> Shot;
        public event EventHandler MyTurn;
    }
}