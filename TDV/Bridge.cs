/* This source is provided under the GNU AGPLv3  license. You are free to modify and distribute this source and any containing work (such as sound files) provided that:
* - You make available complete source code of modifications, even if the modifications are part of a larger project, and make the modified work available under the same license (GNU AGPLv3).
* - You include all copyright and license notices on the modified source.
* - You state which parts of this source were changed in your work
* Note that containing works (such as SharpDX) may be available under a different license.
* Copyright (C) Munawar Bijani
*/
using System;
using System.Threading;
using BPCSharedComponent.ExtendedAudio;
namespace TDV
{
	public class Bridge : MissionObjectBase
	{
		public Bridge(double x, double y)
			: base("b", null)
		{
			this.x = x;
			this.y = y;
			setSpan(0.0284090909, 0.5);
			explodeString = soundPath + "d2.wav";
			setDamagePoints(8000);
		}

		public Bridge()
			: this(0.0, 0.0)
		{ }

		public override void move()
		{
			performDeaths();
			if (readyToTerminate())
			{
				isProjectorStopped = true;
				return;
			}
		}

	}
}
