/* This source is provided under the GNU AGPLv3  license. You are free to modify and distribute this source and any containing work (such as sound files) provided that:
* - You make available complete source code of modifications, even if the modifications are part of a larger project, and make the modified work available under the same license (GNU AGPLv3).
* - You include all copyright and license notices on the modified source.
* - You state which parts of this source were changed in your work
* Note that containing works (such as SharpDX) may be available under a different license.
* Copyright (C) Munawar Bijani
*/
using System;
using SharpDX.DirectSound;
using BPCSharedComponent.VectorCalculation;
using BPCSharedComponent.ExtendedAudio;
namespace TDV
{
	public class SAMMissile : WeaponBase
	{
		private SecondarySoundBuffer launch;
		private SecondarySoundBuffer Hit;
		private SecondarySoundBuffer moveSound;
		public SAMMissile(Weapons w)
			: base(w, "p" + (int)WeaponTypes.samMissile)
		{
			weapon.decreaseAmmunitionFor(WeaponTypes.samMissile);
			neutralizeSpeed(3000.0);
			launch = loadSound(soundPath + "sam1.wav");
			moveSound = DSound.LoadSound3d(DSound.SoundPath + "\\bsg2.wav");
			setSpan(0.20, 0.20);
			followTarget = false;
		}

		public override void free()
		{
			DSound.unloadSound(ref launch);
			DSound.unloadSound(ref Hit);
			DSound.unloadSound(ref moveSound);
		}

		public override void lockOn(Projector target)
		{
			origTarget = target;
		}

		public override void onTick()
		{
			if (isFinished())
			{
				fireDisposeEvent();
				return;
			}
			if (finished && performing)
			{
				//The weapon is done doing what it needs to do, but a sound is still playing.
				//Do not free this weapon until the sound is done playing.
				performing = (Hit != null && DSound.isPlaying(Hit)) || (expl != null && DSound.isPlaying(expl));
				return;
			}

			performing = true;
			base.onTick();
			playSound3d(moveSound, false, false);
			if (inFiringRange())
			{
				moveSound.Stop();
				Hit = target.loadSound(target.soundPath + "bsg" + Common.getRandom(3, 4) + ".wav");
				target.playSound(Hit, true, false);
				fireHitEvent(target, Common.getRandom(100, 200));
				finished = true;
				return;
			}

			if (!DSound.isPlaying(moveSound))
			{
				finished = true;
				explode();
				performing = (Hit != null && DSound.isPlaying(Hit)) || (expl != null && DSound.isPlaying(expl));
			}
		}

		public override void use()
		{
			playSound(launch, true, false);
			playSound3d(moveSound, true, false);
			z = origTarget.z;
			direction = Degrees.GetDegreesBetween(x, y, origTarget.x, origTarget.y);
		}


		public override void serverSideHit(Projector target, int remainingDamage)
		{
			throw new NotImplementedException();
		}
	}
}
