using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace CloseCalls
{
    public class CloseCalls : Script
    {
        Player player = Game.Player;
        Ped ped = Game.Player.Character;

        public CloseCalls()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {
            Vehicle closeVehicle = GTA.World.GetClosestVehicle(ped.Position, 50);
            closeVehicle.Explode();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X)
            {

            }
        }

    }
}
