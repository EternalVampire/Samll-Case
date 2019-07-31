using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Drawing;

namespace Aircraft_War
{
    class HeroBullet:BulletFather
    {
        private static Image imgHero = Resources.bullet2;

        public HeroBullet(PlaneFather pf, int speed, int power)
            : base(pf, imgHero, speed, power)
        {

        }

        
    }
}
