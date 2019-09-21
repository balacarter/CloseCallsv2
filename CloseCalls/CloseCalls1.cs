using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace CloseCalls
{

    public class CloseCalls : Script
    {

        string ModName   = "CloseCalls";
        string Developer = "scriptHijo";
        string Version   = "1.0";
        bool firstTime = true;

        

        Player player = Game.Player; 
        Ped ped = Game.Player.Character;

        Vehicle prevVehicle = null;
        float prevDistance = float.MaxValue;
        float minDistance = float.MaxValue;
        int streak = 0;

        public CloseCalls()
        {
            this.Tick += onTick;
            Interval = 1;
        }

        private void onTick(object sender, EventArgs e)
        {
            if(firstTime)
            {
                UI.Notify(ModName + " " + Version + " by " + Developer + " Loaded");
                firstTime = false;
            }

            if(ped.IsInVehicle())
            {
                displayScore();
                Vehicle[] closeVehicles = GTA.World.GetNearbyVehicles(ped.Position, 10);
                Vehicle xVehicle = getClosestVehicle(closeVehicles);
                

                if (xVehicle != null)
                {
                    float distance;
                    if(GTA.World.GetDistance(ped.Position, xVehicle.Position) < GTA.World.GetDistance(ped.CurrentVehicle.Position, xVehicle.Position))
                        distance = GTA.World.GetDistance(ped.Position, xVehicle.Position);
                    else distance = GTA.World.GetDistance(ped.CurrentVehicle.Position, xVehicle.Position);

                    bool collision = ped.CurrentVehicle.IsTouching(xVehicle);

                    if (collision)
                    {
                        if (streak > 0)
                            UI.Notify("Collision, streak ended.");
                        streak = 0;
                        prevDistance = float.MaxValue;
                        prevVehicle = xVehicle;
                        
                    }
                    if(xVehicle != prevVehicle)
                    {
                        if (distance <= prevDistance)
                        {
                            prevDistance = distance;

                        }
                        else if (prevDistance <= 3)
                        {
                            UI.Notify("CLOSE CALL!: " + prevDistance.ToString("F"));
                            prevVehicle = xVehicle;
                            if (prevDistance < minDistance)
                            {
                                minDistance = prevDistance;
                            }
                            prevDistance = float.MaxValue;
                            streak++;

                        }
                    }
                    

                }
                
            }
            else
            {
                streak = 0;
            }

            
        }

        private Vehicle getClosestVehicle(Vehicle[] v)
        {
            float min = float.MaxValue;
            Vehicle minV = null;
            foreach(Vehicle x in v)
            {
                if(x != ped.CurrentVehicle)
                {
                    float distance = GTA.World.GetDistance(ped.CurrentVehicle.Position, x.Position);
                    if(distance < min)
                    {
                        min = distance;
                        minV = x;
                    }
                }
            }
            return minV;
        }

        public void displayScore()
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "STRING");
            if(minDistance > 3)
                Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, "Nearest miss: " + "-" + ", Streak: " + streak);
            else
                Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, "Nearest miss: " + minDistance + ", Streak: " + streak);
            Function.Call(Hash._0x238FFE5C7B0498A6﻿, 0, 0, 1, -1);
        }
    }
}
