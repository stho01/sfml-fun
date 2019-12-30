using SFML.System;

namespace FireWorks
{
    public class ExplosionUpdater
    {
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private readonly Game _game;

        //**********************************************************
        //** ctor
        //**********************************************************

        public ExplosionUpdater(Game game)
        {
            _game = game;
        }

        //**********************************************************
        //** props
        //**********************************************************
        
        public Vector2f AirResistance { get; set; } = new Vector2f(2f, 2f);

        //**********************************************************
        //** methods:
        //**********************************************************

        public void Update(Explosion explosion)
        {
            foreach (var explosionParticle in explosion.Particles)
            {
                
            }
        }
    }
}