using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;

namespace Aircraft_War
{
    class EnemyBullet:BulletFather

    {
         private static Image imgHero = Resources.bullet1;

         public EnemyBullet(PlaneFather pf, int speed, int power)
            : base(pf, imgHero, speed, power)
        {

        }
    }
}
