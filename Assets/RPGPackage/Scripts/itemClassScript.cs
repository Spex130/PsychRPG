using System;
namespace AssemblyCSharp
{
	public class itemClassScript
	{
		public string name;
		public string effect;
		public int numEffect;
		public float randomDropPercent;


		
		public itemClassScript (string name, string effect, int numEffect, float randomDropPercent)
		{
			//name = "Health Potion;
			//effect = "Health Restore"
			//numEffect = +30;
			//randomDropPercent = 0.05f;
			this.name = name;
			this.effect = effect;
			this.numEffect = numEffect;
			this.randomDropPercent = randomDropPercent;

		}
		public void useItem(characterScript character) {
			if (this.effect == "Health Restore") {
				character.curHealth = character.curHealth + this.numEffect;
				if (character.curHealth > character.maxHealth) {
					character.curHealth = character.maxHealth;
				}
			}
			if (this.effect == "Magic Restore") {
				character.mag = character.mag + numEffect;
			}
		}

	}
}

